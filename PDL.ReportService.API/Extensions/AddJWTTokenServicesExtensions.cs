using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

namespace PDL.ReportService.API.Extensions
{
    public static class AddJWTTokenServicesExtensions
    {
        public static void AddJWTTokenServices(this IServiceCollection Services, IConfiguration Configuration)
        {
            // Add Jwt Setings
            var bindJwtSettings = new JwtSettings();
            Configuration.Bind("JsonWebTokenKeys", bindJwtSettings);
            Services.AddSingleton(bindJwtSettings);


            Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();

                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";

                        var result = JsonSerializer.Serialize(new
                        {
                            success = false,
                            message = "Unauthorized access. Please login again."
                        });

                        return context.Response.WriteAsync(result);
                    }
                };

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = bindJwtSettings.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(bindJwtSettings.IssuerSigningKey)),
                    ValidateIssuer = bindJwtSettings.ValidateIssuer,
                    ValidIssuer = bindJwtSettings.ValidIssuer,
                    ValidateAudience = bindJwtSettings.ValidateAudience,
                    ValidAudience = bindJwtSettings.ValidAudience,
                    //RequireExpirationTime = bindJwtSettings.RequireExpirationTime,
                    //ValidateLifetime = bindJwtSettings.RequireExpirationTime,
                    //ClockSkew = TimeSpan.FromDays(1),
                    RequireExpirationTime = true, // ensures 'exp' claim must be present
                    ValidateLifetime = true,      // actually checks token expiration
                    ClockSkew = TimeSpan.Zero     // or keep minimal (default is 5 mins)
                };
            });
        }
        public class JwtSettings
        {
            public bool ValidateIssuerSigningKey { get; set; }
            public string IssuerSigningKey { get; set; }
            public bool ValidateIssuer { get; set; } = true;
            public string ValidIssuer { get; set; }
            public bool ValidateAudience { get; set; } = true;
            public string ValidAudience { get; set; }
            public bool RequireExpirationTime { get; set; }
            public bool ValidateLifetime { get; set; } = true;
        }

    }
}
