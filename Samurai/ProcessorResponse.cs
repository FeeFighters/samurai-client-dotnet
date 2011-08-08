using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Samurai
{
    public class ProcessorResponse : SamuraiBase
    {
        public bool Success { get; set; }
        public List<Message> Messages { get; set; }
        public string GatewayData { get; set; }
        public string AvsResultCode { get; set; }
    }
}
