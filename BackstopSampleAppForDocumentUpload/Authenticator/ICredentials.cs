using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace BackstopSampleAppForDocumentUpload.Authenticator
{
    public interface ICredentials
    {
        string FullUsername { get; set; }
        string Username { get; set; }
        SecureString Password { get; set; }
        SecureString Token { get; set; }
        string ConnectionUrl { get; set; }
        void StoreCredentials();
        bool ParseFullUsername();
    }
}
