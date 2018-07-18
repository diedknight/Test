using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProductsForPopularSearch
{
    class BaseData
    {
        string id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        string value;

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public bool NeedCheck { get; set; }
    }
}
