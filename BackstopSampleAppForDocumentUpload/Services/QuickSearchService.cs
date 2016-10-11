using BackstopSampleAppForDocumentUpload.Authenticator;
using BackstopSampleAppForDocumentUpload.DataObjects;
using Newtonsoft.Json;
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
    public class QuickSearchService : ServiceBase
    {        
        public QuickSearchService() : base("QuickSearchService")
        {
        }

        public async Task<ServiceResponse<List<Suggestion>>> SearchAsync(string term, int take)
        {
            return await SearchAsync(term, take, CancellationToken.None);
        }

        public async Task<ServiceResponse<List<Suggestion>>> SearchAsync(string term, int take, CancellationToken cancelToken)
        {

            var quickSearchResponse = new ServiceResponse<List<Suggestion>> { Data = new List<Suggestion>() };

            var request = new RestRequest("quicksearch");
            request.AddParameter("searchText", term);
            request.AddParameter("take", take);
            request.AddParameter("metaDataType", "mobile");

            var restResponse = await ExecuteTaskAsync(request);
            quickSearchResponse.StatusCode = restResponse.StatusCode;
            quickSearchResponse.ErrorMessage = restResponse.ErrorMessage;

            if (!string.IsNullOrEmpty(restResponse.Content) && restResponse.StatusCode == HttpStatusCode.OK)
                quickSearchResponse.Data = JsonConvert.DeserializeObject<List<Suggestion>>(restResponse.Content);
            return quickSearchResponse;
        }
        
    }
}
