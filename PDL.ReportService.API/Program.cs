using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using PDL.FIService.Api.HealthCheck;
using PDL.ReportService.API.Extensions;
using PDL.ReportService.Logics.Credentials;
using PDL.ReportService.Logics.Helper;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var otherPodUrl = config["PodUrl"];
bool isLive = builder.Configuration.GetValue<bool>("isliveDb");
string val = builder.Configuration.GetValue<string>("encryptSalts:dbval");
string salt = builder.Configuration.GetValue<string>("encryptSalts:dbName");
var conMgr = new CredManager(config);

// Add services to the container.
builder.Services.AddJWTTokenServices(builder.Configuration);
// Add HTTP client for pinging other pods
builder.Services.AddHttpClient();
// Add health check services
// Register health checks
builder.Services.AddHealthChecks()
    .AddCheck("sql_ado_check", new SqlConnectionHealthCheck(conMgr.getConnectionString(Helper.Decrypt(val, salt), isLive)))
    .AddCheck("other_pod_check", new HttpEndpointHealthCheck(otherPodUrl, builder.Services.BuildServiceProvider().GetRequiredService<IHttpClientFactory>()));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#region Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
        }
    });
});
#endregion
#region Dependency
builder.Services.RegisterRepository();
#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = ""; // Swagger will be available at /swagger
    });
} 
app.UseHttpsRedirection();
app.UseCors(builder =>
{
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});
app.UseAuthorization();

app.MapControllers();
#region// Map health check endpoint
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";


        var result = System.Text.Json.JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description
            }),
            totalDuration = report.TotalDuration.TotalSeconds
        });

        await context.Response.WriteAsync(result);
    }
});
#endregion
app.Run();
