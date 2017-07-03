using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace PriceMeCommon.Extend {
    public static class DecimalExtend {
        public static string Format( this decimal dec, CultureInfo cul, string format) {
            return dec.ToString(format, cul);
        }

        public static string Format(this decimal dec, CultureInfo cul) {
            return dec.Format(cul, "C2");
        }
    }
}
