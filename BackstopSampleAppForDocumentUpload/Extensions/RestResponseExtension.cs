using BackstopSampleAppForDocumentUpload.Services;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace BackstopSampleAppForDocumentUpload.Extensions
{
    public static class RestResponseExtensions
    {
        
        public static Uri ExtractResourceUri(this IRestResponse restResponse)
        {
            var url = restResponse.Headers.FirstOrDefault(h => h.Name == "Location")?.Value as string;
            return url != null ? new Uri(url) : null;
        }

        public static ServiceResponse<T> ToServiceResponse<T>(this IRestResponse<T> restResponse)
        {
            var response = new ServiceResponse<T>()
            {
                Data = restResponse.Data,
                ErrorMessage = restResponse.ErrorMessage,
                StatusCode = restResponse.StatusCode,
                Headers = GetHeaderDictionary(restResponse)
            };

            return response;
        }

        public static bool IsValid<T>(this ServiceResponse<T> response)
        {
            if (response?.StatusCode == HttpStatusCode.OK || response?.StatusCode == HttpStatusCode.Created)
            {
                return true;
            }

            var retVal = response?.StatusCode == HttpStatusCode.OK;
            if (!string.IsNullOrWhiteSpace(response?.ErrorMessage))
            {
                retVal = false;
            }

            return retVal;
        }

        public static IDictionary<string, string> GetHeaderDictionary(this IRestResponse response)
        {
            return response.Headers.ToDictionary(param => param.Name, param => param.Value.ToString());
        }
    }
}
