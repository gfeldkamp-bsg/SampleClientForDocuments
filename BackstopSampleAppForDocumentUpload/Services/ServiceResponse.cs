using System.Collections.Generic;
using System.Net;

namespace BackstopSampleAppForDocumentUpload.Services
{
    public class ServiceResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public T Data { get; set; }
    }
}
