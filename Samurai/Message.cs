using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;

namespace Samurai
{
    /// <summary>
    /// Represents message.
    /// </summary>
    public class Message : SamuraiBase
    {
        /// <summary>
        /// Gets or sets what type of message it is. The value will always be <c>error</c> for 
        /// errors and declines or <c>info</c> for informative messages.
        /// </summary>
        public string Subclass { get; set; }

        /// <summary>
        /// Gets or sets the place where the message applies. Each context is prefixed 
        /// with <c>input</c>, <c>processor</c> or <c>system</c> for user input messages, 
        /// processor messages and UberGateway system messages respectively.
        /// </summary>
        public string Context { get; set; }

        /// <summary>
        /// Gets or sets some specific information about the message.
        /// </summary>
        public string Key { get; set; }
    }
}
