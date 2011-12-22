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
        PaymentMethodPayload defaultPayload;
        PaymentMethod defaultPaymentMethod;

        [SetUp]
        public void TestInitialize()
        {
            Samurai.Options = new SamuraiOptions()
            {
                MerchantKey = "a1ebafb6da5238fb8a3ac9f6",
                MerchantPassword = "ae1aa640f6b735c4730fbb56",
                ProcessorToken = "5a0e1ca1e5a11a2997bbf912",
                Debug = true
            };

            defaultPayload = new PaymentMethodPayload()
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Address1 = "123 Main St.",
                Address2 = "Apt #3",
                City = "Chicago",
                State = "IL",
                Zip = "10101",
                CardNumber = "4111-1111-1111-1111",
                Cvv = "123",
                ExpiryMonth = 3,
                ExpiryYear = 2015,
                Custom = "custom",
                Sandbox = true
            };
            defaultPaymentMethod = PaymentMethod.Create(defaultPayload);
        }

        [Test]
        public void CaptureShouldBeSuccessful() {
            var auth = Processor.TheProcessor.Authorize(defaultPaymentMethod.PaymentMethodToken, "100.0");
            var transaction = auth.Capture();
            Assert.IsTrue( transaction.Success() );
        }
        [Test]
        public void CaptureShouldBeSuccessfulForFullAmount() {
            var auth = Processor.TheProcessor.Authorize(defaultPaymentMethod.PaymentMethodToken, "100.0");
            var transaction = auth.Capture("100.0");
            Assert.IsTrue( transaction.Success() );
        }
        [Test]
        public void CaptureShouldBeSuccessfulForPartialAmount() {
            var auth = Processor.TheProcessor.Authorize(defaultPaymentMethod.PaymentMethodToken, "100.0");
            var transaction = auth.Capture("50.0");
            Assert.IsTrue( transaction.Success() );
        }
        [Test]
        public void CaptureFailuresShouldReturnProcessorTransactionInvalidWithDeclinedAuth() {
            var auth = Processor.TheProcessor.Authorize(defaultPaymentMethod.PaymentMethodToken, "100.02");
            var transaction = auth.Capture();
            Assert.IsFalse( transaction.Success() );
            Assert.AreEqual( new List<string>(){"This transaction type is not allowed."}, transaction.Errors["processor.transaction"] );
        }
        [Test]
        public void CaptureFailuresShouldReturnProcessorTransactionDeclined() {
            var auth = Processor.TheProcessor.Authorize(defaultPaymentMethod.PaymentMethodToken, "100.00");
            var transaction = auth.Capture("100.02");
            Assert.IsFalse( transaction.Success() );
            Assert.AreEqual( new List<string>(){"The card was declined."}, transaction.Errors["processor.transaction"] );
        }
        [Test]
        public void CaptureFailuresShouldReturnInputAmountInvalid() {
            var auth = Processor.TheProcessor.Authorize(defaultPaymentMethod.PaymentMethodToken, "100.00");
            var transaction = auth.Capture("100.10");
            Assert.IsFalse( transaction.Success() );
            Assert.AreEqual( new List<string>(){"The transaction amount was invalid."}, transaction.Errors["input.amount"] );
        }

        [Test]
        public void ReverseOnCaptureShouldBeSuccessful() {
            var purchase = Processor.TheProcessor.Purchase(defaultPaymentMethod.PaymentMethodToken, "100.00");
            var transaction = purchase.Reverse();
            Assert.IsTrue( transaction.Success() );
        }
        [Test]
        public void ReverseOnCaptureShouldBeSuccessfulForFullAmount() {
            var purchase = Processor.TheProcessor.Purchase(defaultPaymentMethod.PaymentMethodToken, "100.0");
            var transaction = purchase.Reverse("100.0");
            Assert.IsTrue( transaction.Success() );
        }
        [Test]
        public void ReverseOnCaptureShouldBeSuccessfulForPartialAmount() {
            var purchase = Processor.TheProcessor.Purchase(defaultPaymentMethod.PaymentMethodToken, "100.0");
            var transaction = purchase.Reverse("50.0");
            Assert.IsTrue( transaction.Success() );
        }
        [Test]
        public void ReverseOnAuthorizeShouldBeSuccessful() {
            var purchase = Processor.TheProcessor.Authorize(defaultPaymentMethod.PaymentMethodToken, "100.00");
            var transaction = purchase.Reverse();
            Assert.IsTrue( transaction.Success() );
        }
        [Test]
        public void ReverseFailuresShouldReturnInputAmountInvalid() {
            var auth = Processor.TheProcessor.Purchase(defaultPaymentMethod.PaymentMethodToken, "100.00");
            var transaction = auth.Reverse("100.10");
            Assert.IsFalse( transaction.Success() );
            Assert.AreEqual( new List<string>(){"The transaction amount was invalid."}, transaction.Errors["input.amount"] );
        }

        [Test]
        public void CreditOnCaptureShouldBeSuccessful() {
            var purchase = Processor.TheProcessor.Purchase(defaultPaymentMethod.PaymentMethodToken, "100.00");
            var transaction = purchase.Credit();
            Assert.IsTrue( transaction.Success() );
        }
        [Test]
        public void CreditOnCaptureShouldBeSuccessfulForFullAmount() {
            var purchase = Processor.TheProcessor.Purchase(defaultPaymentMethod.PaymentMethodToken, "100.0");
            var transaction = purchase.Credit("100.0");
            Assert.IsTrue( transaction.Success() );
        }
        [Test]
        public void CreditOnCaptureShouldBeSuccessfulForPartialAmount() {
            var purchase = Processor.TheProcessor.Purchase(defaultPaymentMethod.PaymentMethodToken, "100.0");
            var transaction = purchase.Credit("50.0");
            Assert.IsTrue( transaction.Success() );
        }
        [Test]
        public void CreditOnAuthorizeShouldBeSuccessful() {
            var purchase = Processor.TheProcessor.Authorize(defaultPaymentMethod.PaymentMethodToken, "100.00");
            var transaction = purchase.Credit();
            Assert.IsTrue( transaction.Success() );
        }
        [Test]
        public void CreditFailuresShouldReturnInputAmountInvalid() {
            var auth = Processor.TheProcessor.Purchase(defaultPaymentMethod.PaymentMethodToken, "100.00");
            var transaction = auth.Credit("100.10");
            Assert.IsFalse( transaction.Success() );
            Assert.AreEqual( new List<string>(){"The transaction amount was invalid."}, transaction.Errors["input.amount"] );
        }

        [Test]
        public void VoidOnAuthorizedShouldBeSuccessful() {
            var auth = Processor.TheProcessor.Authorize(defaultPaymentMethod.PaymentMethodToken, "100.00");
            var transaction = auth.Void();
            Assert.IsTrue( transaction.Success() );
        }
        [Test]
        public void VoidOnCapturedShouldBeSuccessful() {
            var purchase = Processor.TheProcessor.Purchase(defaultPaymentMethod.PaymentMethodToken, "100.00");
            var transaction = purchase.Void();
            Assert.IsTrue( transaction.Success() );
        }

        [Test]
        public void FindShouldReturnATransaction() {
            var t_ref = Processor.TheProcessor.Purchase(defaultPaymentMethod.PaymentMethodToken, "20.00");
            var t = Transaction.Find(t_ref.ReferenceId);

            Assert.AreEqual(t.TransactionToken.ToLower(), t_ref.TransactionToken.ToLower());
            Assert.AreEqual(t.ReferenceId.ToLower(), t_ref.ReferenceId.ToLower());
            Assert.AreEqual(t.Description, String.Empty);
            Assert.AreEqual(t.DescriptorName, String.Empty);
            Assert.AreEqual(t.DescriptorPhone, String.Empty);
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
