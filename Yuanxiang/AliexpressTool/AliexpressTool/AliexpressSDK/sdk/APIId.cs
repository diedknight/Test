using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace com.alibaba.openapi.client
{
    class APIId
    {
        private String namespaceValue;

        public String NamespaceValue
        {
            get { return namespaceValue; }
            set { namespaceValue = value; }
        }
        private String name;

        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        private int version;

        public int Version
        {
            get { return version; }
            set { version = value; }
        }

       
    }
}
