using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.alibaba.openapi.client.primitive
{
    class NullableLong : NullablePrimitiveObject
    {
        public NullableLong(long value)
        {
            this.value = value;
            this.isNull = false;
        }

        long value;
        private Boolean isNull;

        public Boolean IsNull
        {
            get { return isNull; }
        }

        public void setValue(long value)
        {
            this.value = value;
            this.isNull = false;
        }

        public long getValue()
        {
            return this.value;
        }
    }
}
