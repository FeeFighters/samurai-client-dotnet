using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Samurai.Test
{
    [TestClass]
    public class PaymentMethodTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Samurai.Options = new SamuraiOptions()
            {
                MerchantKey = "c55b9694bc164fe518dd7aab",
                MerchantPassword = "cde50d5324038ddc69415050",
                ProcessorToken = "b0d9c4324dabd84975b0a5e1"
            };
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Samurai.Options = Samurai.DefaultOptions;
        }

        [TestMethod]
        public void FetchTest()
        {
            var pm = PaymentMethod.Fetch("11162477aad1a7053b72dbd0");

            Assert.IsNotNull(pm);
            Assert.AreEqual(pm.PaymentMethodToken, "11162477aad1a7053b72dbd0", true);
            Assert.AreEqual(pm.CreatedAt, new DateTime(2011, 8, 7, 3, 19, 35, DateTimeKind.Utc));
            Assert.IsFalse(pm.IsRetained);
            Assert.IsFalse(pm.IsRedacted);
            Assert.IsTrue(pm.IsSensitiveDataValid);
            Assert.AreEqual(pm.LastFourDigits, "0027");
            Assert.AreEqual(pm.CardType, "visa", true);
            // write more...
        }

        [TestMethod]
        public void UpdateTest()
        {
            // fetch
            var pm = PaymentMethod.Fetch("11162477aad1a7053b72dbd0");
            string oldCustom = pm.Custom;

            // updating
            string newCustom = string.Format("Updated at {0}.", DateTime.UtcNow);
            pm.Custom = newCustom;
            pm.Update();

            pm = PaymentMethod.Fetch("11162477aad1a7053b72dbd0");

            // assert
            Assert.IsNotNull(pm);
            Assert.AreNotEqual(oldCustom, pm.Custom);
            Assert.AreEqual(newCustom, pm.Custom);
            Assert.IsTrue(pm.IsSensitiveDataValid);
        }
    }
}
