using Newtonsoft.Json;

namespace BackstopSampleAppForDocumentUpload.DataObjects
{
    public class ActivityTag
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
