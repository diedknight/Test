using System;
using System.Collections.Generic;
using System.Text;

namespace PopualerSearchIndexBuilder
{
    public class CskStoreRetailertracker
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int ProductId { get; set; }
        public DateTime ClickTime { get; set; }
    }
}
