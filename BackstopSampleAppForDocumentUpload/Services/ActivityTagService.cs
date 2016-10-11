using BackstopSampleAppForDocumentUpload.DataObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BackstopSampleAppForDocumentUpload.Services
{
    public class ActivityTagService : ServiceBase
    {
        public ActivityTagService() : base("ActivityTagService")
        {
        }

        public async Task<ServiceResponse<List<ActivityTag>>> GetActivityTagsAsync()
        {

            var request = new RestRequest("activitytags");

            var restResponse = await ExecuteTaskAsync(request);
            dynamic jsonResponse = JsonConvert.DeserializeObject(restResponse.Content);
            var tagsJArray = jsonResponse.tags as JArray;
            var tags = tagsJArray?.ToObject<List<ActivityTag>>();
            var serviceResponse = new ServiceResponse<List<ActivityTag>>()
            {
                Data = tags,
                StatusCode = restResponse.StatusCode,
                ErrorMessage = restResponse.ErrorMessage
            };

            return serviceResponse;
        }

    }
}
