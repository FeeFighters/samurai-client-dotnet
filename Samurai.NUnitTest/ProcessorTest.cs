using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Samurai.NUnitTest
{
    [TestFixture]
    public class ProcessorTest
    {
        [Test]
        public void The_Processor_Test()
        {
            var theProcessor = Processor.TheProcessor;

            Assert.IsNotNull(theProcessor);
            Assert.AreEqual(Samurai.ProcessorToken, theProcessor.ProcessorToken);
        }

        [Test]
        public void New_Processor_Has_Given_Token()
        {
            var processor = new Processor("asdf");

            Assert.IsNotNull(processor);
            Assert.AreEqual("asdf", processor.ProcessorToken);
        }
    }
}
