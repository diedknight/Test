using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.alibaba.openapi.client.primitive
{
    class NullableChar : NullablePrimitiveObject
    {
        public NullableChar(char value)
        {
            this.value = value;
            this.isNull = false;
        }

        char value;
        private Boolean isNull = true;

        public Boolean IsNull
        {
            get { return isNull; }
        }

        public void setValue(char value)
        {
            this.value = value;
            this.isNull = false;
        }

        public char getValue()
        {
            return this.value;
        }
    }
}
