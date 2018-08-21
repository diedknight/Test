using System;
using System.Collections.Generic;
using System.Text;

namespace PopualerSearchIndexBuilder
{
    public class RetailerVotesSum
    {
        public int ID { get; set; }
        public int RetailerID { get; set; }
        public int RetailerRatingSum { get; set; }
        public int RetailerTotalRatingVotes { get; set; }
    }
}