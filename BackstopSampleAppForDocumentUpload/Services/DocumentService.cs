using BackstopSampleAppForDocumentUpload.Authenticator;
using BackstopSampleAppForDocumentUpload.DataObjects;
using BackstopSampleAppForDocumentUpload.Extensions;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BackstopSampleAppForDocumentUpload.Services
{
    public class DocumentService : ServiceBase
    {
        public DocumentService() : base("DocumentService")
        {
           
        }

        /// <summary>
        /// This actually adds to the AttachTo field of the document.
        /// </summary>
        /// <param name="docId"></param>
        /// <param name="relatedEntities"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<string>> AddRelationAsync(string docId, IEnumerable<Entity> relatedEntities)
        {
            return await AddRelationAsync(docId, relatedEntities, CancellationToken.None);
        }

        public async Task<ServiceResponse<string>> AddRelationAsync(string docId, IEnumerable<Entity> relatedEntities, CancellationToken cancelToken)
        {
            var entities = relatedEntities.Select(x => new { entityId = x.Id, entityType = x.EntityType });
            
            var request = new RestRequest(string.Concat("documents/", docId, "/relations")) { Method = Method.PUT };
            request.AddParameter("application/json", JsonConvert.SerializeObject(entities), ParameterType.RequestBody);
            request.AddHeader("Content-Type", "application/json");

            var restResponse = await ExecuteTaskAsync<string>(request);
            return restResponse.ToServiceResponse();
        }

        public async Task<ServiceResponse<int>> CreateDocumentAsync(DocumentUpload documentUpload)
        {
            return await CreateDocumentAsync(documentUpload, CancellationToken.None);
        }

        public async Task<ServiceResponse<int>> CreateDocumentAsync(DocumentUpload documentUpload, CancellationToken cancelToken)
        {
           
            var request = new RestRequest("documents/upload") { Method = Method.POST };

            var backstopSerializationSettings = new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd'T'HH:mm:ss.FFFzzz"
            };

            request.AddParameter("metadata", JsonConvert.SerializeObject(documentUpload, backstopSerializationSettings), ParameterType.GetOrPost);
            request.AddFile("file", documentUpload.Bytes, documentUpload.FileName);

            var restResponse = await ExecuteTaskAsync<Uri>(request);

            int id;
            int.TryParse(restResponse.ExtractResourceUri()?.Segments.Last(), out id);
            var serviceResponse = new ServiceResponse<int>
            {
                StatusCode = restResponse.StatusCode,
                ErrorMessage = restResponse.ErrorMessage,
                Data = id
            };

            return serviceResponse;
        }
        
    }
}
