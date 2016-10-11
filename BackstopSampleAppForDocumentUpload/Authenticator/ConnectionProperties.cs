using System;
using System.Security;

namespace BackstopSampleAppForDocumentUpload.Authenticator
{
    public class ConnectionProperties
    {
        public static Uri Uri { get; set; }

        public static string DefaultUrl { get; set; } = "https://auth-prod.backstopsolutions.com";
        public static string Username { get; set; }

        public static SecureString Password { get; set; }
        public static SecureString Token { get; set; }
        public static bool HasLoggedIn { get; set; }
        public static object UserAgent { get; } = "BackstopSampleApp";
        public static int Timeout { get; } = 3600000;

        static ConnectionProperties()
        {
            Password = null;
            Username = null;
            Token = null;
            Uri = new Uri(DefaultUrl);
        }
    }
}
