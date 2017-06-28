using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.alibaba.openapi.client.primitive
{
    class NullableDouble : NullablePrimitiveObject
    {
        public NullableDouble(double value)
        {
            this.value = value;
            this.isNull = false;
        }

        double value;
        private Boolean isNull;

        public Boolean IsNull
        {
            get { return isNull; }
        }

        public void setValue(double value)
        {
            this.value = value;
            this.isNull = false;
        }

        public double getValue()
        {
            return this.value;
        }
    }
}
