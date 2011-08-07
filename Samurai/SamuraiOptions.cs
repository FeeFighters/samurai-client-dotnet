using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Samurai
{
    /// <summary>
    /// Represents connection options.
    /// </summary>
    public class SamuraiOptions
    {
        /// <summary>
        /// Gets or sets API base url.
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// Gets or sets merchant key for connection.
        /// </summary>
        public string MerchantKey { get; set; }

        /// <summary>
        /// Gets or sets merchant password for connection.
        /// </summary>
        public string MerchantPassword { get; set; }

        /// <summary>
        /// Gets or sets processor token for connection.
        /// </summary>
        public string ProcessorToken { get; set; }

        /// <summary>
        /// Merges this options with another. Properties of this options which are not null
        /// will not be merged (overrided).
        /// </summary>
        /// <param name="options">Options to merge with.</param>
        /// <returns>this options merged with another.</returns>
        public SamuraiOptions ReverseMerge(SamuraiOptions options)
        {
            Site = Site ?? options.Site;
            MerchantKey = MerchantKey ?? options.MerchantKey;
            MerchantPassword = MerchantPassword ?? options.MerchantPassword;
            ProcessorToken = ProcessorToken ?? options.ProcessorToken;

            return this;
        }
    }
}
