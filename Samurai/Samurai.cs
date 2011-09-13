using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Samurai
{
    /// <summary>
    /// Represents main API.
    /// </summary>
    public static class Samurai
    {
        public static readonly string DefaultSite = "https://api.samurai.feefighters.com/v1/";
        public static readonly SamuraiOptions DefaultOptions = new SamuraiOptions() { Site = DefaultSite };

        private static SamuraiOptions _options = DefaultOptions;

        /// <summary>
        /// Gets or sets connection options.
        /// </summary>
        public static SamuraiOptions Options
        {
            get { return _options; }
            set { _options = (value ?? new SamuraiOptions()).ReverseMerge(DefaultOptions); }
        }

        /// <summary>
        /// Gets or sets site for API calls.
        /// </summary>
        public static string Site
        {
            get { return _options.Site; }
        }

        /// <summary>
        /// Gets or sets merchant key.
        /// </summary>
        public static string MerchantKey
        {
            get { return _options.MerchantKey; }
        }

        /// <summary>
        /// Gets or sets merchant password.
        /// </summary>
        public static string MerchantPassword
        {
            get { return _options.MerchantPassword; }
        }

        /// <summary>
        /// Gets or sets processor token.
        /// </summary>
        public static string ProcessorToken
        {
            get { return _options.ProcessorToken; }
        }
    }
}
