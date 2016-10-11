using RestSharp;
using System.Linq;
using System.Threading.Tasks;

namespace BackstopSampleAppForDocumentUpload.Services
{
    public class LoginService : ServiceBase
    {
        public LoginService() : base("LoginService") { }

        public async Task<bool> CheckLoggedInAsync()
        {
            var restResponse = await ExecuteTaskAsync(new RestRequest("loggedIn"));
            return restResponse.Content == "true";
        }

        public async Task<ServiceResponse<string>> LoginAsync()
        {
            var request = new RestRequest("login") { Method = Method.POST };
            var restResponse = await ExecuteTaskAsync(request);

            return new ServiceResponse<string>()
            {
                Data = restResponse.Content,
                ErrorMessage = restResponse.ErrorMessage,
                StatusCode = restResponse.StatusCode,
                Headers = restResponse.Headers.ToDictionary(param => param.Name, param => param.Value.ToString())
            };
        }
    }
}
