using Newtonsoft.Json;

namespace BackstopSampleAppForDocumentUpload.DataObjects
{
    public class Entity
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "entityType")]
        public string EntityType { get; set; }

        public Suggestion ToSuggestion()
        {
            return new Suggestion { EntityType = EntityType, Id = Id, Name = Name };
        }
    }
}
