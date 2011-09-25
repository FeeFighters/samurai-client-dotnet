using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Samurai.NUnitTest
{
    [TestFixture]
    public class PaymentMethodTest
    {
        [SetUp]
        public void TestInitialize()
        {
            Samurai.Options = new SamuraiOptions()
            {
                MerchantKey = "c55b9694bc164fe518dd7aab",
                MerchantPassword = "cde50d5324038ddc69415050",
                ProcessorToken = "b0d9c4324dabd84975b0a5e1"
            };
        }

        [TearDown]
        public void TestCleanup()
        {
            Samurai.Options = Samurai.DefaultOptions;
        }

        [Test]
        public void Fetch_PaymentMethod_Test()
        {
            var pm = PaymentMethod.Fetch("11162477aad1a7053b72dbd0");

            Assert.IsNotNull(pm);
            Assert.AreEqual(pm.PaymentMethodToken.ToLower(), "11162477aad1a7053b72dbd0".ToLower());
            Assert.AreEqual(pm.CreatedAt, new DateTime(2011, 8, 7, 3, 19, 35, DateTimeKind.Utc));
            Assert.IsFalse(pm.IsRetained);
            Assert.IsFalse(pm.IsRedacted);
            Assert.IsTrue(pm.IsSensitiveDataValid);
            Assert.AreEqual(pm.LastFourDigits, "0027");
            Assert.AreEqual(pm.CardType.ToLower(), "visa".ToLower());
            // write more...
        }

        [Test]
        public void Update_PaymentMethod_Test()
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

        [Test]
        public void Create_Then_Redact_PaymentMethod_Test()
        {
            // create new
            var newPM = TestHelper.CreateScoobyDoPaymentMethod();

            Assert.IsTrue(newPM.IsSensitiveDataValid);
            Assert.IsFalse(newPM.IsRetained);
            Assert.IsFalse(newPM.IsRedacted);

            // redact
            var redactedPM = newPM.Redact();

            Assert.IsTrue(redactedPM.IsRedacted);
            Assert.IsTrue(redactedPM.IsSensitiveDataValid);
            Assert.IsFalse(newPM.IsRetained);
        }

        [Test]
        public void Create_Then_Retain_Then_Redact_PaymentMethod_Test()
        {
            // create new
            var newPM = TestHelper.CreateScoobyDoPaymentMethod();

            Assert.IsTrue(newPM.IsSensitiveDataValid);
            Assert.IsFalse(newPM.IsRetained);
            Assert.IsFalse(newPM.IsRedacted);

            // redact
            var retainedPM = newPM.Retain();

            Assert.IsTrue(retainedPM.IsSensitiveDataValid);
            Assert.IsTrue(retainedPM.IsRetained);
            Assert.IsFalse(retainedPM.IsRedacted);

            // simple purchase
            var redactedPM = retainedPM.Redact();

            Assert.IsTrue(redactedPM.IsSensitiveDataValid);
            Assert.IsTrue(redactedPM.IsRetained);
            Assert.IsTrue(redactedPM.IsRedacted);
        }

        
    }
}
