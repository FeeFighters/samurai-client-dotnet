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

        public static TransactionType StringToTransactionType(string value)
        {
            switch (value.ToLowerInvariant())
            {
                case "purchase": return TransactionType.Purchase;
                case "authorize": return TransactionType.Authorize;
                case "capture": return TransactionType.Capture;
                case "void": return TransactionType.Void;
                case "credit": return TransactionType.Credit;
            }

            throw new ArgumentException("given string can't be converted into transaction type.");
        }
    }
}
