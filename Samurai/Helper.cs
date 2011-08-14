using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Samurai
{
    /// <summary>
    /// Contains some helper methods.
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Converts decimal to string that is acceptable by Samurai API. 
        /// </summary>
        /// <param name="value">String to convert.</param>
        /// <returns>a string value that is acceptable by Samurai API.</returns>
        public static string DecimalToString(decimal value)
        {
            return value.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts string representation of transaction type into TransactionType value.
        /// </summary>
        /// <param name="value">String representation of transaction type.</param>
        /// <returns>TransactionType value.</returns>
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
