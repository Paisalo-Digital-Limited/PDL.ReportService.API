using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PDL.ReportService.Repository.Repository;
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

            Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = bindJwtSettings.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(bindJwtSettings.IssuerSigningKey)),
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

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        string userIdClaim = context.Principal?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                        string tokenVersionClaim = context.Principal?.FindFirst("tokenVersion")?.Value;
                        string nameClaim = context.Principal?.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
                        string emailClaim = context.Principal?.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

                        if (string.IsNullOrEmpty(userIdClaim) || string.IsNullOrEmpty(tokenVersionClaim))
                        {
                            context.Fail("Invalid token claims.");
                            context.HttpContext.Items["JwtError"] = "Invalid token claims.";
                            return;
                        }

                        long userId = long.Parse(userIdClaim);
                        int tokenVersionInToken = int.Parse(tokenVersionClaim);

                        var userRepo = context.HttpContext.RequestServices.GetRequiredService<UserRepository>();
                        var user = await userRepo.GetByIdAsync(userId);

                        if (user == null)
                        {
                            context.Fail("Invalid user: User not found.");
                            context.HttpContext.Items["JwtError"] = "Invalid user: User not found.";
                        }
                        else if (user.Id.ToString() != userIdClaim)
                        {
                            context.Fail("Invalid user: ID mismatch.");
                            context.HttpContext.Items["JwtError"] = "Invalid user: ID mismatch.";
                        }
                        else if (!string.Equals(user.Email, emailClaim, StringComparison.OrdinalIgnoreCase))
                        {
                            context.Fail("Invalid user: Email mismatch.");
                            context.HttpContext.Items["JwtError"] = "Invalid user: Email mismatch.";
                        }
                        else if (!string.Equals(user.Name, nameClaim, StringComparison.OrdinalIgnoreCase))
                        {
                            context.Fail("Invalid user: Name mismatch.");
                            context.HttpContext.Items["JwtError"] = "Invalid user: Name mismatch.";
                        }
                        else if (user.TokenVersion != tokenVersionInToken)
                        {
                            context.Fail("Token has been invalidated due to password change.");
                            context.HttpContext.Items["JwtError"] = "Token has been invalidated due to password change.";
                        }
                    },

                    OnChallenge = async context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        // Read message from HttpContext if available
                        string errorMessage = context.HttpContext.Items.ContainsKey("JwtError")
                            ? context.HttpContext.Items["JwtError"]?.ToString()
                            : context.ErrorDescription ?? "Your session has expired or token is invalid.";

                        var errorResponse = new
                        {
                            error = "Unauthorized",
                            message = errorMessage
                        };

                        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
                    }
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
