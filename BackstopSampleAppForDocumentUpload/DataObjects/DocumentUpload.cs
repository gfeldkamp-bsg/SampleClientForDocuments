using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BackstopSampleAppForDocumentUpload.DataObjects
{
    public class DocumentUpload
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "effectiveDate")]
        public DateTime EffectiveDate { get; set; }

        [JsonProperty(PropertyName = "activityTags")]
        public IEnumerable<ActivityTag> ActivityTags { get; set; }

        [JsonProperty(PropertyName = "permissionTag")]
        public string PermissionTag { get; set; }

        [JsonProperty(PropertyName = "author")]
        public User Author { get; set; }

        [JsonProperty(PropertyName = "filename")]
        public string FileName { get; set; }

        [JsonProperty(PropertyName = "links")]
        public IEnumerable<Entity> Links { get; set; }

        [JsonProperty(PropertyName = "attachedTo")]
        public AttachedTo AttachedTo { get; set; }

        [JsonIgnore]
        public byte[] Bytes { get; set; }
    }
}
