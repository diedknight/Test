using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMeCommon.Data
{
    public class ReviewCommunicationData
    {
        public int Id { get; set; }
        public int ReviewId { get; set; }
        public bool Sender { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Attachment { get; set; }
    }
}
