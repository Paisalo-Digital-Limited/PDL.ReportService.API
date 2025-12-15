using Microsoft.AspNetCore.Mvc.Controllers;
using Newtonsoft.Json;
using PDL.ReportService.Interfaces.Interfaces;
using Renci.SshNet.Messages;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace PDL.ReportService.API.Middleware
{
    public class UserApiEndpointMiddleware
    {
        private readonly RequestDelegate _next;

        public UserApiEndpointMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            IUserApiEndpointMappingRepository userApiRepo)
        {
            // 1️⃣ USER ID (Null allowed & never block)
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            int? userId = null;
            if (!string.IsNullOrEmpty(userIdClaim))
            {
                if (int.TryParse(userIdClaim, out int parsed))
                {
                    userId = parsed;
                }
            }

            // ❗ If no userId → skip and continue
            if (userId == null)
            {
                await _next(context);
                return;
            }

            // 2️⃣ Controller + Action
            var endpoint = context.GetEndpoint();
            var descriptor = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>();

            string controllerName = descriptor != null
                ? Regex.Replace(descriptor.ControllerName ?? "", @"\s+", "")
                : null;

            string functionName = descriptor != null
                ? Regex.Replace(descriptor.ActionName ?? "", @"\s+", "")
                : null;
            string serviceName = descriptor?.ControllerTypeInfo?.Assembly?.GetName()?.Name;
            // ❗ If controllerName or functionName missing → skip
            if (controllerName == null || functionName == null)
            {
                await _next(context);
                return;
            }

            // 3️⃣ SERVICE NAME
            //string serviceName = await GetServiceNameFromRequest(context);

            // ❗ Missing serviceName → skip
            if (string.IsNullOrWhiteSpace(serviceName))
            {
                await _next(context);
                return;
            }

            // 4️⃣ DB Check (even if unauthorized → do NOT block)
            await userApiRepo.IsUserMappedToEndpointAsync(
                userId.Value,
                controllerName,
                functionName,
                serviceName
            );

            // 5️⃣ Always continue API
            await _next(context);
        }

        private async Task<string> GetServiceNameFromRequest(HttpContext context)
        {
            string serviceName = null;

            // ✅ GET request → read from query string
            if (context.Request.Method == HttpMethods.Get)
            {
                serviceName = context.Request.Query["serviceName"].ToString();
                return serviceName; // ✅ stored before return
            }

            context.Request.EnableBuffering();
            var reader = new StreamReader(context.Request.Body, leaveOpen: true);
           // using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
            string body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            if (!string.IsNullOrWhiteSpace(body))
            {
                try
                {
                    dynamic data = JsonConvert.DeserializeObject(body);
                    return data?.serviceName;
                }
                catch { }
            }

            return null;
        }
    }
}
