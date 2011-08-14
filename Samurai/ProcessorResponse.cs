using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Samurai
{
    /// <summary>
    /// Represents processor response.
    /// </summary>
    public class ProcessorResponse : SamuraiBase
    {
        /// <summary>
        /// Gets or sets a value that indicates whether the operation is succeed.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets a list of messages.
        /// </summary>
        public List<Message> Messages { get; set; }

        /// <summary>
        /// Gets or sets gateway data.
        /// </summary>
        public string GatewayData { get; set; }

        /// <summary>
        /// Gets or sets AVS result code.
        /// </summary>
        public string AvsResultCode { get; set; }
    }
}
