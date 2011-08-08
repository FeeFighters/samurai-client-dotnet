using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;

namespace Samurai
{
    public class Message : SamuraiBase
    {
        public string Subclass { get; set; }
        public string Context { get; set; }
        public string Key { get; set; }
    }
}
