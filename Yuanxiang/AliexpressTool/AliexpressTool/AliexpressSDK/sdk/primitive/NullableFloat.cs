using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.alibaba.openapi.client.primitive
{
    class NullableFloat : NullablePrimitiveObject
    {
        public NullableFloat(float value)
        {
            this.value = value;
            this.isNull = false;
        }

        float value;
        private Boolean isNull;

        public Boolean IsNull
        {
            get { return isNull; }
        }

        public void setValue(float value)
        {
            this.value = value;
            this.isNull = false;
        }

        public float getValue()
        {
            return this.value;
        }
    }
}
