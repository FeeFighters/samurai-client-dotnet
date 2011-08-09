using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Samurai.Test
{
    [TestClass]
    public class ProcessorTest
    {
        [TestMethod]
        public void The_Processor_Test()
        {
            var theProcessor = Processor.TheProcessor;

            Assert.AreEqual(Samurai.ProcessorToken, theProcessor.ProcessorToken);
        }

        [TestMethod]
        public void New_Processor_Has_Given_Token()
        {
            var processor = new Processor("asdf");

            Assert.AreEqual("asdf", processor.ProcessorToken);
        }
    }
}
