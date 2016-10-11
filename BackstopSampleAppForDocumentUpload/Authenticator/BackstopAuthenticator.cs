using BackstopSampleAppForDocumentUpload.Extensions;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace BackstopSampleAppForDocumentUpload.Authenticator
{
    public class BackstopAuthenticator : IAuthenticator
    {
        public void Authenticate(IRestClient client, IRestRequest request)
        {
            var password = ConnectionProperties.Token ?? ConnectionProperties.Password;
            var base64Token =
                Convert.ToBase64String(
                    Encoding.UTF8.GetBytes($"{ConnectionProperties.Username}:{password.ToInsecureString()}"));
            var authHeader = $"Basic {base64Token}";

            if (!IsHeaderSet(request, "Authorization"))
            {
                request.AddParameter("Authorization", authHeader, ParameterType.HttpHeader);
            }
            if (ConnectionProperties.Token != null && !IsHeaderSet(request, "token"))
            {
                request.AddParameter("token", "true", ParameterType.HttpHeader);
            }
            if (!IsHeaderSet(request, "X-Backstop-Token-Purpose"))
            {
                request.AddParameter("X-Backstop-Token-Purpose", "API", ParameterType.HttpHeader);
            }
        }

        private static bool IsHeaderSet(IRestRequest request, string header)
        {
            return request.Parameters.Any(p => p.Name.Equals(header, StringComparison.OrdinalIgnoreCase));
        }
    }
}
