using System;

namespace PiholeDashboard.Models
{
    public class PiHoleConfig
    {
        public string Uri { get; set; }
        public string ApiKey { get; set; }

        public PiHoleConfig()
        {
            Uri = "example.com:80";
            ApiKey = "xxxx";
        }
    }
}