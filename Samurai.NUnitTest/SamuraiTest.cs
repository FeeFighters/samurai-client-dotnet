using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Samurai.NUnitTest
{
    [TestFixture]
    public class SamuraiTest
    {
        [SetUp]
        public void TestInitialize()
        {
            Samurai.Options = Samurai.DefaultOptions;
        }

        [TearDown]
        public void TestCleanup()
        {
            Samurai.Options = Samurai.DefaultOptions;
        }

        [Test]
        public void OptionsAreDefaultAtStartup()
        {
            Assert.IsNotNull(Samurai.Options);
            Assert.IsNotNull(Samurai.Site);
            Assert.AreEqual(Samurai.Site, "https://api.samurai.feefighters.com/v1/");

            Assert.IsNull(Samurai.MerchantKey);
            Assert.IsNull(Samurai.MerchantPassword);
            Assert.IsNull(Samurai.ProcessorToken);
        }

        [Test]
        public void SetNewOptionsTest()
        {
            var options = new SamuraiOptions()
            {
                MerchantKey = "c55b9694bc164fe518dd7aab",
                MerchantPassword = "cde50d5324038ddc69415050",
                ProcessorToken = "b0d9c4324dabd84975b0a5e1"
            };

            Samurai.Options = options;

            Assert.AreEqual(Samurai.Site, "https://api.samurai.feefighters.com/v1/");
            Assert.AreEqual(Samurai.MerchantKey, options.MerchantKey);
            Assert.AreEqual(Samurai.MerchantPassword, options.MerchantPassword);
            Assert.AreEqual(Samurai.ProcessorToken, options.ProcessorToken);
        }

        [Test]
        public void Samurai_Has_Aliases_For_Options()
        {
            Assert.AreEqual(Samurai.Options.MerchantKey, Samurai.MerchantKey);
            Assert.AreEqual(Samurai.Options.MerchantPassword, Samurai.MerchantPassword);
            Assert.AreEqual(Samurai.Options.ProcessorToken, Samurai.ProcessorToken);
            Assert.AreEqual(Samurai.Options.Site, Samurai.Site);
        }
    }
}
