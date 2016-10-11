using Newtonsoft.Json;

namespace BackstopSampleAppForDocumentUpload.DataObjects
{
    public class User
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "userName")]
        public string UserName { get; set; }
        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }
        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }
        [JsonProperty(PropertyName = "emailAddress")]
        public string EmailAddress { get; set; }

        [JsonIgnore]
        public string FullName => $"{FirstName} {LastName}";
    }
}
