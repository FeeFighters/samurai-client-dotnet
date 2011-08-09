using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Samurai.Test
{
    [TestClass]
    public class TransactionTest
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

        [TestMethod]
        public void Fetch_Transaction_Test()
        {
            var t = Transaction.Fetch("7C07CBBEEA7676981F711994");

            Assert.AreEqual(t.TransactionToken, "3bfde5b1e79aaef7fb1b6012", true);
            Assert.AreEqual(t.ReferenceId, "7c07cbbeea7676981f711994", true);
            Assert.AreEqual(t.CreatedAt, new DateTime(2011, 8, 7, 3, 19, 46, DateTimeKind.Utc));
            Assert.AreEqual(t.Descriptor, string.Empty);
            Assert.AreEqual(t.Custom, string.Empty);
            //Assert.AreEqual(trans.BillingReference, string.Empty);
            //Assert.AreEqual(trans.CustomerReference, string.Empty);
            Assert.AreEqual(t.TransactionType, "purchase", true);
            Assert.AreEqual(t.Amount, 50);
            Assert.AreEqual(t.CurrencyCode, "US", true);
            Assert.AreEqual(t.ProcessorToken, "b0d9c4324dabd84975b0a5e1", true);
            Assert.IsNotNull(t.ProcessorResponse);
            Assert.IsNotNull(t.PaymentMethod);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Samurai.Options = Samurai.DefaultOptions;
        }
    }
}
