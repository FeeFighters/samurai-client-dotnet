using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Samurai.Test
{
    [TestClass]
    public class PurchaseTest
    {
        private Processor _processor;
        private string _paymentMethodToken;

        private string _testDescriptor
        {
            get { return string.Format("Descriptor for testing purposes at {0}", DateTime.UtcNow); }
        }
        private string _testCustom
        {
            get { return string.Format("Custom data for testing purposes at {0}", DateTime.UtcNow); }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            Samurai.Options = new SamuraiOptions()
            {
                MerchantKey = "c55b9694bc164fe518dd7aab",
                MerchantPassword = "cde50d5324038ddc69415050",
                ProcessorToken = "b0d9c4324dabd84975b0a5e1"
            };

            _processor = Processor.TheProcessor;
            _paymentMethodToken = "11162477aad1a7053b72dbd0";
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Samurai.Options = Samurai.DefaultOptions;
        }

        [TestMethod]
        public void Simple_Purchase_17_USD_As_String()
        {
            var t = _processor.Purchase(_paymentMethodToken, "17.00");

            Assert.IsTrue(t.ProcessorResponse.Success);
            Assert.AreEqual(TransactionType.Purchase, t.Type);
            Assert.AreEqual(17, t.Amount);
        }

        [TestMethod]
        public void Simple_Purchase_17_25_USD_As_Decimal()
        {
            var t = _processor.Purchase(_paymentMethodToken, 17.25m);

            Assert.IsTrue(t.ProcessorResponse.Success);
            Assert.AreEqual(TransactionType.Purchase, t.Type);
            Assert.AreEqual(17.25m, t.Amount);
        }

        [TestMethod]
        public void Simple_Purchase_2_USD_As_Int()
        {
            var t = _processor.Purchase(_paymentMethodToken, 2);

            Assert.IsTrue(t.ProcessorResponse.Success);
            Assert.AreEqual(TransactionType.Purchase, t.Type);
            Assert.AreEqual(2, t.Amount);
        }

        [TestMethod]
        public void Simple_Purchase_18_75_USD_As_Decimal_With_Descriptor_And_Custom()
        {
            string descriptor = _testDescriptor;
            string custom = _testCustom;

            var t = _processor.Purchase(_paymentMethodToken, 18.75m, descriptor, custom);

            Assert.IsTrue(t.ProcessorResponse.Success);
            Assert.AreEqual(TransactionType.Purchase, t.Type);
            Assert.AreEqual(18.75m, t.Amount);
            Assert.AreEqual(descriptor, t.Descriptor);
            Assert.AreEqual(custom, t.Custom);
        }

        [TestMethod]
        public void Authorize_13_45_USD_As_Decimal()
        {
            decimal amount = 13.45m;
            string descriptor = _testDescriptor;
            string custom = _testCustom;

            var t = _processor.Authorize(_paymentMethodToken, amount, descriptor, custom);

            Assert.IsTrue(t.ProcessorResponse.Success);
            Assert.AreEqual(TransactionType.Authorize, t.Type);
            Assert.AreEqual(amount, t.Amount);
            Assert.AreEqual(descriptor, t.Descriptor);
            Assert.AreEqual(custom, t.Custom);
        }

        [TestMethod]
        public void Find_Authorization_Test()
        {
            var authorization = _processor.Authorize(_paymentMethodToken, 2);
            var transaction = Transaction.Fetch(authorization.ReferenceId);

            Assert.AreEqual(authorization.ReferenceId, transaction.ReferenceId);
            Assert.AreEqual(TransactionType.Authorize, authorization.Type);
        }

        [TestMethod]
        public void Capture_Authorization_Test()
        {
            var amount = 3m;

            var authorization = _processor.Authorize(_paymentMethodToken, amount);
            var capturedTr = authorization.Capture(amount);

            Assert.AreEqual(amount, capturedTr.Amount);
            Assert.AreEqual(TransactionType.Capture, capturedTr.Type);
            Assert.IsTrue(capturedTr.ProcessorResponse.Success);
        }

        [TestMethod]
        public void Capture_Authorization_Without_Specifying_Amount_Test()
        {
            var amount = 5m;

            var authorization = _processor.Authorize(_paymentMethodToken, amount);
            var capturedTr = authorization.Capture();

            Assert.AreEqual(amount, capturedTr.Amount);
            Assert.AreEqual(TransactionType.Capture, capturedTr.Type);
            Assert.IsTrue(capturedTr.ProcessorResponse.Success);
        }

        [TestMethod]
        public void Partially_Capture_Authorization_Test()
        {
            var authAmount = 4m;
            var captAmount = 2.5m;

            var authorization = _processor.Authorize(_paymentMethodToken, authAmount);
            var capturedTr = authorization.Capture(captAmount);

            Assert.AreEqual(captAmount, capturedTr.Amount);
            Assert.AreEqual(TransactionType.Capture, capturedTr.Type);
            Assert.IsTrue(capturedTr.ProcessorResponse.Success);
        }

        [TestMethod]
        public void Void_Authorization_Test()
        {
            var amount = 6.75m;

            var authorization = _processor.Authorize(_paymentMethodToken, amount);
            var voidedAuth = authorization.Void();

            Assert.IsTrue(voidedAuth.ProcessorResponse.Success);
            Assert.AreEqual(TransactionType.Void, voidedAuth.Type);
            Assert.AreEqual(amount, voidedAuth.Amount);
        }

        [TestMethod]
        public void Void_Recent_Purchase_Test()
        {
            var amount = 7m;

            var purchase = _processor.Purchase(_paymentMethodToken, amount);
            var voidedPurchase = purchase.Void();

            Assert.IsTrue(voidedPurchase.ProcessorResponse.Success);
            Assert.AreEqual(TransactionType.Void, voidedPurchase.Type);
            Assert.AreEqual(amount, voidedPurchase.Amount);
        }

        [TestMethod]
        public void Purchase_With_Tracking_Data_Test()
        {
            var amount = 8.15m;
            string billingRef = "ABC123";
            string customerRef = "Customer (123)";

            var purchase = _processor.Purchase(_paymentMethodToken, amount,
                customer_reference: customerRef,
                billing_reference: billingRef
            );

            Assert.IsNotNull(purchase);
            Assert.IsTrue(purchase.ProcessorResponse.Success);
            Assert.AreEqual(TransactionType.Purchase, purchase.Type);
            Assert.AreEqual(amount, purchase.Amount);
            Assert.AreEqual(billingRef, purchase.BillingReference);
            Assert.AreEqual(customerRef, purchase.CustomerReference);
        }

        [TestMethod]
        public void Authorize_With_Tracking_Data_Test()
        {
            var amount = 8.25m;
            string billingRef = "ABC123";
            string customerRef = "Customer (123)";

            var authorization = _processor.Authorize(_paymentMethodToken, amount,
                customer_reference: customerRef,
                billing_reference: billingRef
            );

            Assert.IsNotNull(authorization);
            Assert.IsTrue(authorization.ProcessorResponse.Success);
            Assert.AreEqual(TransactionType.Authorize, authorization.Type);
            Assert.AreEqual(amount, authorization.Amount);
            Assert.AreEqual(billingRef, authorization.BillingReference);
            Assert.AreEqual(customerRef, authorization.CustomerReference);
        }

        // [TestMethod]
        // public void ShouldNot_Be_Able_To_Credit_Recent_Purchase_Test()
        // {
        //     var amount = 7.35m;
        // 
        //     var purchase = _processor.Purchase(_paymentMethodToken, amount);
        //     var creditedPurchase = purchase.Credit();
        // 
        //     Assert.IsFalse(creditedPurchase.ProcessorResponse.Success);
        //     Assert.AreEqual(TransactionType.Credit, creditedPurchase.Type);
        // }
    }
}
