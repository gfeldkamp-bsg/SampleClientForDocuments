using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackstopSampleAppForDocumentUpload.DataObjects
{
    public class Document
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Filename { get; set; }

        public string Description { get; set; }

        public DateTime EffectiveDate { get; set; }

        public List<ActivityTag> ActivityTags { get; set; } = new List<ActivityTag>();

        public string PermissionTag { get; set; }

        public List<Link> Links { get; set; } = new List<Link>();
        
    }
}
