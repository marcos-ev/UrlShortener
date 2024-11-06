using System;

namespace urlshorter.Models
{
    public class AccessLog
    {
        public int Id { get; set; }
        public int UrlId { get; set; }
        public DateTime AccessedAt { get; set; } = DateTime.UtcNow;
        public string DeviceType { get; set; } = string.Empty;
        public string OS { get; set; } = string.Empty;
        public string Browser { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;

        public Url Url { get; set; } 
    }
}
