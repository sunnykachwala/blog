namespace Np.Admin.WebApi.Utilities
{
    using Microsoft.AspNetCore.Http;
    using System.Net;

    public class CommonHelper
    {
        public static string GetIPAddress(HttpContext context)
        {
            string ipAddressStr = context.Connection.RemoteIpAddress.ToString();
            // If your application is behind a proxy or a load balancer,
            // check for the X-Forwarded-For header to get the client's real IP.
            if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                ipAddressStr = context.Request.Headers["X-Forwarded-For"];
                // The X-Forwarded-For header can contain a comma-separated list of IP addresses.
                // The client's IP address is usually the first one in the list.
                ipAddressStr = ipAddressStr.Split(',')[0].Trim();
            }
            // allow localhost test to skip token login
            if (ipAddressStr == "::1")
            {
                ipAddressStr = "127.0.0.1:7262";
            }
            // remove port number
            if (ipAddressStr.Contains(':'))
                ipAddressStr = ipAddressStr.Split(':')[0];

            IPAddress ipAddress = IPAddress.Parse(ipAddressStr);

            return ipAddress.ToString();

        }
    }
}
