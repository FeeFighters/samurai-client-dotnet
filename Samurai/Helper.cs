using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Samurai
{
    public static class Helper
    {
        public static string DecimalToString(decimal value)
        {
            return value.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
