using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV2.InsureeModule.Helpers
{
    public static class Extensions
    {
        public static decimal? ToNullableDecimal(this string s)
        {
            decimal d;
            if (decimal.TryParse(s, out d)) return d;
            return null;
        }

        public static float? ToNullableFloat(this string s)
        {
            float f;
            if (float.TryParse(s, out f)) return f;
            return null;
        }
    }
}
