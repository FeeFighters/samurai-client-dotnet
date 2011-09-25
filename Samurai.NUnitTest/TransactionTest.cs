using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Samurai.NUnitTest
{
    [TestFixture]
    public class TransactionTest
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

        [Test]
        public void Fetch_Transaction_Test()
        {
            var t = Transaction.Fetch("7C07CBBEEA7676981F711994");

            Assert.AreEqual(t.TransactionToken.ToLower(), "3bfde5b1e79aaef7fb1b6012".ToLower());
            Assert.AreEqual(t.ReferenceId.ToLower(), "7c07cbbeea7676981f711994".ToLower());
            Assert.AreEqual(t.CreatedAt, new DateTime(2011, 8, 7, 3, 19, 46, DateTimeKind.Utc));
            Assert.AreEqual(t.Descriptor, String.Empty);
            Assert.AreEqual(t.Custom, String.Empty);
            Assert.AreEqual(t.BillingReference, String.Empty);
            Assert.AreEqual(t.CustomerReference, String.Empty);
            Assert.AreEqual(TransactionType.Purchase, t.Type);
            Assert.AreEqual(t.Amount, 50);
            Assert.AreEqual(t.CurrencyCode.ToLower(), "US".ToLower());
            Assert.AreEqual(t.ProcessorToken.ToLower(), "b0d9c4324dabd84975b0a5e1".ToLower());
            Assert.IsNotNull(t.ProcessorResponse);
            Assert.IsNotNull(t.PaymentMethod);
        }

        [TearDown]
        public void TestCleanup()
        {
            Samurai.Options = Samurai.DefaultOptions;
        }
    }
}
