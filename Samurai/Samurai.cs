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
        public static readonly string DefaultSite = "https://samurai.feefighters.com/v1/";
        public static readonly SamuraiOptions DefaultOptions = new SamuraiOptions() { Site = DefaultSite };

        private static SamuraiOptions _options = new SamuraiOptions();

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
    }
}
