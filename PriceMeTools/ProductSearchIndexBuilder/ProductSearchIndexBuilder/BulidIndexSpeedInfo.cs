using System;

namespace ProductSearchIndexBuilder
{
    public class BulidIndexSpeedInfo
    {
        public DateTime StartReadDBTime;
        public DateTime EndReadDBTime;
        public DateTime StartWriteIndexTime;
        public DateTime EndWriteIndexTime;
    }
}