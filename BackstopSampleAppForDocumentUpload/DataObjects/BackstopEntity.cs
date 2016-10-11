using Newtonsoft.Json;
using System;

namespace BackstopSampleAppForDocumentUpload.DataObjects
{
    [JsonObject(MemberSerialization.OptIn)]
    public class BackstopEntity
    {
        public const string UnknownEntityName = "(Unknown)";

        [JsonIgnore]
        public int? Id { get; set; }

        public string EntityType { get; set; }

        private string _entityId;
        [JsonProperty(PropertyName = "id")]
        public string EntityId
        {
            get
            {
                return _entityId;
            }
            set
            {
                _entityId = value;
                ParseEntityString(_entityId);
            }
        }

        private string _entityName;
        [JsonProperty(PropertyName = "name")]
        public string EntityName
        {
            get
            {
                return _entityName ?? UnknownEntityName;
            }
            set
            {
                _entityName = value;
            }
        }

        public BackstopEntity() { }

        public BackstopEntity(string rep)
        {
            ParseEntityString(rep);
        }

        public void ParseEntityString(string entityString)
        {
            entityString = entityString.TrimStart('|');
            var parts = entityString.Split('|');
            int output;

            if (parts.Length == 1 && int.TryParse(entityString, out output))
            {
                Id = output;
            }
            else if (parts.Length == 2 && int.TryParse(parts[1], out output))
            {
                EntityType = parts[0];
                Id = output;
            }
            else if (parts.Length == 3 && int.TryParse(parts[1], out output))
            {
                EntityType = parts[0];
                Id = output;
                EntityName = parts[2];
            }
            else
            {
                throw new ArgumentException($"Invalid entity Id. Unable to parse. {_entityId}");
            }
        }

        public Suggestion ToSuggestion()
        {
            return new Suggestion()
            {
                EntityType = EntityType,
                Id = Id,
                Name = EntityName
            };
        }

        public override string ToString()
        {
            return EntityName == null ? $"{EntityType}|{Id}" : $"{EntityType}|{Id}|{EntityName}";
        }
    }
}
