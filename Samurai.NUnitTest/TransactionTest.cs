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
        private Processor _processor;
        private string _paymentMethodToken;		
		
        [SetUp]
        public void TestInitialize()
        {
            Samurai.Options = new SamuraiOptions()
            {
                MerchantKey = "a1ebafb6da5238fb8a3ac9f6",
                MerchantPassword = "ae1aa640f6b735c4730fbb56",
                ProcessorToken = "5a0e1ca1e5a11a2997bbf912"
            };
			_processor = Processor.TheProcessor;
			_paymentMethodToken = TestHelper.CreatePaymentMethod().PaymentMethodToken;
        }

        [Test]
        public void Fetch_Transaction_Test()
        {
			var t_ref = _processor.Purchase(_paymentMethodToken, "20.00");
            var t = Transaction.Fetch(t_ref.ReferenceId);

            Assert.AreEqual(t.TransactionToken.ToLower(), t_ref.TransactionToken.ToLower());
            Assert.AreEqual(t.ReferenceId.ToLower(), t_ref.ReferenceId.ToLower());
            Assert.AreEqual(t.Descriptor, String.Empty);
            Assert.AreEqual(t.Custom, String.Empty);
            Assert.AreEqual(t.BillingReference, String.Empty);
            Assert.AreEqual(t.CustomerReference, String.Empty);
            Assert.AreEqual(TransactionType.Purchase, t.Type);
            Assert.AreEqual(t.Amount, 20);
            Assert.AreEqual(t.CurrencyCode.ToLower(), "USD".ToLower());
            Assert.AreEqual(t.ProcessorToken.ToLower(), "5a0e1ca1e5a11a2997bbf912".ToLower());
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
