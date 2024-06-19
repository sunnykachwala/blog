namespace Admin.Np.WebApi.Middleware
{
    using Admin.Np.WebApi.Model;
    using System.Net;
    using System.Security.Claims;
    using System.Text.Json;

    public class ApiResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var email = string.Empty;
            if (context.User.Identity is ClaimsIdentity identity)
            {
                email = identity.FindFirst(ClaimTypes.Name)?.Value;
            }
            context.Items["email"] = email;

            var originalResponseBody = context.Response.Body;

            if (context.Request.Path == "/api/Authenticate/generate-qr")
            {
                await _next(context);
                if (!context.Response.ContentType.Contains("application/json; charset=utf-8"))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        // Set the response body to the memory stream
                        context.Response.Body = memoryStream;

                        // Do something with the response if needed
                        // Access or manipulate the response content here before sending it back

                        // Copy the content of the memory stream to the original response body
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        await memoryStream.CopyToAsync(originalResponseBody);
                        return;
                    }
                }
            }

            using var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;
            await _next(context);
            context.Response.Body = originalResponseBody;

            await BuildApiResponse(context, responseBodyStream);
        }

        private async Task BuildApiResponse(HttpContext context, MemoryStream responseBodyStream)
        {
            var response = context.Response;
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            var responseContent = await new StreamReader(responseBodyStream).ReadToEndAsync();
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            ApiResponse<object>? responseData = null;
            if (response.StatusCode == (int)HttpStatusCode.OK)
            {
                if (!string.IsNullOrWhiteSpace(responseContent))
                {

                    responseData = new ApiResponse<object>
                    {
                        StatusCode = response.StatusCode,
                        Message = "Success",
                        Data = JsonSerializer.Deserialize<dynamic>(responseContent),

                    };
                }
            }
            else
            {
                string res = "Error";
                if (!string.IsNullOrWhiteSpace(responseContent)) res = responseContent.ToString();
                responseData = new ApiResponse<object>
                {
                    StatusCode = response.StatusCode,
                    ErrorMessage = res,
                    Data = string.IsNullOrWhiteSpace(responseContent) ? "" : JsonSerializer.Deserialize<dynamic>(responseContent),
                };
            }

            context.Response.ContentType = "application/json";
            var json = JsonSerializer.Serialize(responseData, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            context.Response.ContentLength = System.Text.Encoding.UTF8.GetByteCount(json);
            await context.Response.WriteAsync(json);
        }
    }
}
