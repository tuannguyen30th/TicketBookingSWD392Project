using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Configuration
{
    public class ConfigurationModel
    {
        public class GoogleConfiguration
        {
            public string? ClientID { get; set; }
            public string? ClientSecret { get; set; }
        }
        public class FirebaseConfiguration
        {
            public string? ApiKey { get; set; }
            public string? AuthEmail { get; set; }
            public string? AuthPassword { get; set; }
            public string? Bucket { get; set; }

        }
        public class RedisCacheConfiguration
        {
            public string RedisCacheConnection { get; set; }
        }
        public class FirebaseAdminSDK
        {
            public string Type { get; set; }
            public string ProjectId { get; set; }
            public string PrivateKeyId { get; set; }
            public string PrivateKey { get; set; }
            public string ClientEmail { get; set; }
            public string ClientId { get; set; }
            public string AuthUri { get; set; }
            public string TokenUri { get; set; }
            public string AuthProviderX509CertUrl { get; set; }
            public string ClientX509CertUrl { get; set; }
            public string UniverseDomain { get; set; }
        }
    }
}