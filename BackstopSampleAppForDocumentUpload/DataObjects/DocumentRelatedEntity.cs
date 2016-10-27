using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackstopSampleAppForDocumentUpload.DataObjects
{
    public class DocumentRelatedEntity
    {
        public int DocumentId { get; set; }
        public int RelatedEntityId { get; set; }
        public string RelatedEntityType { get; set; }
    }
}
