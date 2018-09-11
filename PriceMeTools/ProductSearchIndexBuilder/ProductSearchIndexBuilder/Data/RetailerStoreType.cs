using System;

namespace ProductSearchIndexBuilder.Data
{
    public class RetailerStoreType
    {
        public byte RetailerStoreTypeID { get; set; }
        public string StoreTypeName { get; set; }
        public string Description { get; set; }
        public Nullable<DateTime> CreatedOn { get; set; }
    }
}