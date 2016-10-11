using BackstopSampleAppForDocumentUpload.Authenticator;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BackstopSampleAppForDocumentUpload.Services
{
    public abstract class ServiceBase
    {
        private const string BaseRestUrl = "backstop/rest";
        private RestClient _restClient;

        private RestClient ServiceRestClient
        {
            get
            {
                _restClient.BaseUrl = new Uri(BuildBaseUrl());
                return _restClient;
            }
            set { _restClient = value; }
        }

        protected ServiceBase(string source)
        {
            ServiceRestClient = new RestClient
            {
                UserAgent = $"{ConnectionProperties.UserAgent};Backstop-RequestSource={source}",
                Timeout = ConnectionProperties.Timeout,
                Authenticator = new BackstopAuthenticator()
            };
        }

        protected async Task<IRestResponse> ExecuteTaskAsync(IRestRequest request)
        {
            var restResponse = await ServiceRestClient.ExecuteTaskAsync(request);
            return restResponse;
        }

        protected async Task<IRestResponse<T>> ExecuteTaskAsync<T>(IRestRequest request)
        {
            var client = ServiceRestClient;
            var restResponse = await client.ExecuteTaskAsync<T>(request);
            return restResponse;
        }
        private string BuildBaseUrl()
        {
            var path = ConnectionProperties.Uri.AbsoluteUri;
            path = path.TrimEnd('/');
            var fullPath = string.Concat(path, '/', BaseRestUrl);
            return fullPath;
        }
    }
}
