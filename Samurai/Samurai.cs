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
        public static readonly string DefaultSite = "https://api.ubergateway.com/v1/";
        public static readonly SamuraiOptions DefaultOptions = new SamuraiOptions() { Site = DefaultSite };

        private static SamuraiOptions _options = DefaultOptions;

        /// <summary>
        /// Gets or sets connection options.
        /// </summary>
        public static SamuraiOptions Options
        {
            get { return _options; }
            set
            {
                _options = (value ?? new SamuraiOptions()).ReverseMerge(DefaultOptions);
                // Samurai::Base.setup_site!
            }
        }

        public static string Site
        {
            get { return _options.Site; }
        }

        public static string MerchantKey
        {
            get { return _options.MerchantKey; }
        }

        public static string MerchantPassword
        {
            get { return _options.MerchantPassword; }
        }

        public static string ProcessorToken
        {
            get { return _options.ProcessorToken; }
        }
    }
}
