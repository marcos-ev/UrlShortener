using Microsoft.AspNetCore.Http;

namespace urlshorter.Helpers
{
    public static class DeviceHelper
    {
        public static (string DeviceType, string OS, string Browser) GetDeviceInfo(HttpRequest request)
        {
            var userAgent = request.Headers["User-Agent"].ToString().ToLower();

            string deviceType = userAgent.Contains("mobile") ? "Mobile" : "Desktop";
            string os = userAgent.Contains("windows") ? "Windows" : userAgent.Contains("mac") ? "MacOS" : "Unknown";
            string browser = userAgent.Contains("chrome") ? "Chrome" : userAgent.Contains("firefox") ? "Firefox" : "Unknown";

            return (deviceType, os, browser);
        }
    }
}
