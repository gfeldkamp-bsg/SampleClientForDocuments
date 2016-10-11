using Newtonsoft.Json;

namespace BackstopSampleAppForDocumentUpload.DataObjects
{
    public class AttachedTo
    {
        [JsonProperty(PropertyName = "entityId")]
        public int EntityId { get; set; }
        [JsonProperty(PropertyName = "entityType")]
        public string EntityType { get; set; }
    }
}
