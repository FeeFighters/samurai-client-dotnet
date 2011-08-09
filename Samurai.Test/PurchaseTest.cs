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
            Assert.AreEqual(17, t.Amount);
        }

        [TestMethod]
        public void Simple_Purchase_17_25_USD_As_Decimal()
        {
            var t = _processor.Purchase(_paymentMethodToken, 17.25m);

            Assert.IsTrue(t.ProcessorResponse.Success);
            Assert.AreEqual(17.25m, t.Amount);
        }

        [TestMethod]
        public void Simple_Purchase_2_USD_As_Int()
        {
            var t = _processor.Purchase(_paymentMethodToken, 2);

            Assert.IsTrue(t.ProcessorResponse.Success);
            Assert.AreEqual(2, t.Amount);
        }

        [TestMethod]
        public void Simple_Purchase_18_75_USD_As_Decimal_With_Descriptor_And_Custom()
        {
            string descriptor = _testDescriptor;
            string custom = _testCustom;

            var t = _processor.Purchase(_paymentMethodToken, 18.75m, descriptor, custom);

            Assert.IsTrue(t.ProcessorResponse.Success);
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
    }
}
