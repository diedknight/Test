using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.alibaba.openapi.client.primitive
{
    class NullableDateTime : NullablePrimitiveObject
    {
        public NullableDateTime(DateTime value)
        {
            this.value = value;
            this.isNull = false;
        }

        DateTime value;
        private Boolean isNull;

        public Boolean IsNull
        {
            get { return isNull; }
        }

        public void setValue(DateTime value)
        {
            this.value = value;
            this.isNull = false;
        }

        public DateTime getValue()
        {
            return this.value;
        }
    }
}
