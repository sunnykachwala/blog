namespace Np.Admin.WebApi.Utilities
{
    using System.Net;
    using System.Numerics;
    public class IPRangeChecker
    {
        public static bool IsInRange(string clientIp, string startIp, string endIp)
        {
            BigInteger ipNumber = IPToBigInteger(IPAddress.Parse(clientIp));
            BigInteger startIpNumber = IPToBigInteger(IPAddress.Parse(startIp));
            BigInteger endIpNumber = IPToBigInteger(IPAddress.Parse(endIp));

            return ipNumber >= startIpNumber && ipNumber <= endIpNumber;
        }   

        private static BigInteger IPToBigInteger(IPAddress ip)
        {
            byte[] bytes = ip.GetAddressBytes();
            // Ensure the IP address is in big-endian format
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return new BigInteger(bytes.Concat(new byte[] { 0 }).ToArray()); // Convert to BigInteger
        }
    }
}
