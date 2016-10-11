namespace BackstopSampleAppForDocumentUpload.DataObjects
{
    public class Suggestion
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string EntityType { get; set; }
        public string HasPermission { get; set; }
        public string OtherId { get; set; }
        public object ExtraInformation { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public Entity ToEntity()
        {
            return new Entity { EntityType = EntityType, Id = Id.GetValueOrDefault(), Name = Name };
        }

        public BackstopEntity ToBackstopEntity()
        {
            return new BackstopEntity
            {
                Id = Id.GetValueOrDefault(),
                EntityName = Name,
                EntityType = EntityType
            };
        }
    }
}
