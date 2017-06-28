using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.alibaba.openapi.client.primitive
{
    class NullableInt : NullablePrimitiveObject
    {
        public NullableInt(int value)
        {
            this.value = value;
            this.isNull = false;
        }

        int value;
        private Boolean isNull;

        public Boolean IsNull
        {
            get { return isNull; }
        }

        public void setValue(int value)
        {
            this.value = value;
            this.isNull = false;
        }

        public int getValue()
        {
            return this.value;
        }
    }
}
