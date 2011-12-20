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
        PaymentMethodPayload defaultPayload;
        PaymentMethod defaultPaymentMethod;
        string rand;

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

            rand = new Random().Next().ToString();
        }

        [Test]
        public void TheProcessorShouldReturnTheDefaultProcessor() {
            var theProcessor = Processor.TheProcessor;
            
            Assert.IsNotNull(theProcessor);
            Assert.AreEqual(Samurai.ProcessorToken, theProcessor.ProcessorToken);
        }
        
        [Test]
        public void NewProcessorShouldReturnAProcessor() {
            var processor = new Processor("abc123");

            Assert.IsNotNull(processor);
            Assert.AreEqual("abc123", processor.ProcessorToken);
        }

        [Test]
        public void PurchaseShouldBeSuccessful() {
            var transaction = Processor.TheProcessor.Purchase(defaultPaymentMethod.PaymentMethodToken,
                                                              "100.0",
                                                              new TransactionPayload() {
                                                                Descriptor = "descriptor",
                                                                Custom = "custom_data",
                                                                BillingReference = "ABC123"+rand,
                                                                CustomerReference = "Customer (123)"
                                                              });

            Assert.IsTrue( transaction.Success() );
            Assert.AreEqual("descriptor", transaction.Descriptor);
            Assert.AreEqual("custom_data", transaction.Custom);
            Assert.AreEqual("ABC123"+rand, transaction.BillingReference);
            Assert.AreEqual("Customer (123)", transaction.CustomerReference);
        }

        [Test]
        public void PurchaseFailuresShouldReturnProcessorTransactionDeclined() {
            var transaction = Processor.TheProcessor.Purchase(defaultPaymentMethod.PaymentMethodToken,
                                                              "1.02", new TransactionPayload() { BillingReference = rand });
            Assert.IsFalse( transaction.Success() );
            Assert.AreEqual( new List<string>(){"The card was declined."}, transaction.Errors["processor.transaction"] );
        }
        [Test]
        public void PurchaseFailuresShouldReturnInputAmountInvalid() {
            var transaction = Processor.TheProcessor.Purchase(defaultPaymentMethod.PaymentMethodToken,
                                                              "1.10", new TransactionPayload() { BillingReference = rand });
            Assert.IsFalse( transaction.Success() );
            Assert.AreEqual( new List<string>(){"The transaction amount was invalid."}, transaction.Errors["input.amount"] );
        }
        /*
        [Test]
        public void PurchaseFailuresShouldReturnInputCardNumberFailedChecksum() {
            defaultPaymentMethod = PaymentMethod.Create(defaultPayload.Merge(new PaymentMethodPayload() { CardNumber = "1234123412341234" }));
            var transaction = Processor.TheProcessor.Purchase(defaultPaymentMethod.PaymentMethodToken,
                                                              "1.00", new TransactionPayload() { BillingReference = rand });
            Assert.IsFalse( transaction.Success() );
            Assert.AreEqual( new List<string>(){"The card number was invalid."}, transaction.Errors["input.card_number"] );
        }
        [Test]
        public void PurchaseFailuresShouldReturnInputCardNumberInvalid() {
            defaultPaymentMethod = PaymentMethod.Create(defaultPayload.Merge(new PaymentMethodPayload() { CardNumber = "5105105105105100" }));
            var transaction = Processor.TheProcessor.Purchase(defaultPaymentMethod.PaymentMethodToken,
                                                              "1.00", new TransactionPayload() { BillingReference = rand });
            Assert.IsFalse( transaction.Success() );
            Assert.AreEqual( new List<string>(){"The card number was invalid."}, transaction.Errors["input.card_number"] );
        }
        */

        [Test]
        public void PurchaseCvvResponsesShouldReturnProcessorCvvResultCodeM() {
            defaultPaymentMethod = PaymentMethod.Create(defaultPayload.Merge(new PaymentMethodPayload() { Cvv = "111" }));
            var transaction = Processor.TheProcessor.Purchase(defaultPaymentMethod.PaymentMethodToken,
                                                              "1.00", new TransactionPayload() { BillingReference = rand });
            Assert.IsTrue( transaction.Success() );
            Assert.AreEqual( "M", transaction.ProcessorResponse.CvvResultCode );
        }
        [Test]
        public void PurchaseCvvResponsesShouldReturnProcessorCvvResultCodeN() {
            defaultPaymentMethod = PaymentMethod.Create(defaultPayload.Merge(new PaymentMethodPayload() { Cvv = "222" }));
            var transaction = Processor.TheProcessor.Purchase(defaultPaymentMethod.PaymentMethodToken,
                                                              "1.00", new TransactionPayload() { BillingReference = rand });
            Assert.IsTrue( transaction.Success() );
            Assert.AreEqual( "N", transaction.ProcessorResponse.CvvResultCode );
        }
        [Test]
        public void PurchaseAvsResponsesShouldReturnProcessorAvsResultCodeY() {
            defaultPaymentMethod = PaymentMethod.Create(defaultPayload.Merge(new PaymentMethodPayload() {
                Address1 = "1000 1st Av",
                Address2 = "",
                Zip = "10101"
            }));
            var transaction = Processor.TheProcessor.Purchase(defaultPaymentMethod.PaymentMethodToken,
                                                              "1.00", new TransactionPayload() { BillingReference = rand });
            Assert.IsTrue( transaction.Success() );
            Assert.AreEqual( "Y", transaction.ProcessorResponse.AvsResultCode );
        }
        [Test]
        public void PurchaseAvsResponsesShouldReturnProcessorAvsResultCodeZ() {
            defaultPaymentMethod = PaymentMethod.Create(defaultPayload.Merge(new PaymentMethodPayload() {
                Address1 = "",
                Address2 = "",
                Zip = "10101"
            }));
            var transaction = Processor.TheProcessor.Purchase(defaultPaymentMethod.PaymentMethodToken,
                                                              "1.00", new TransactionPayload() { BillingReference = rand });
            Assert.IsTrue( transaction.Success() );
            Assert.AreEqual( "Z", transaction.ProcessorResponse.AvsResultCode );
        }
        [Test]
        public void PurchaseAvsResponsesShouldReturnProcessorAvsResultCodeN() {
            defaultPaymentMethod = PaymentMethod.Create(defaultPayload.Merge(new PaymentMethodPayload() {
                Address1 = "123 Main St.",
                Address2 = "",
                Zip = "60610"
            }));
            var transaction = Processor.TheProcessor.Purchase(defaultPaymentMethod.PaymentMethodToken,
                                                              "1.00", new TransactionPayload() { BillingReference = rand });
            Assert.IsTrue( transaction.Success() );
            Assert.AreEqual( "N", transaction.ProcessorResponse.AvsResultCode );
        }
        
        [Test]
        public void AuthorizeShouldBeSuccessful() {
            var transaction = Processor.TheProcessor.Authorize(defaultPaymentMethod.PaymentMethodToken,
                                                               "100.0",
                                                               new TransactionPayload() {
                                                                 Descriptor = "descriptor",
                                                                 Custom = "custom_data",
                                                                 BillingReference = "ABC123"+rand,
                                                                 CustomerReference = "Customer (123)"
                                                               });

            Assert.IsTrue( transaction.Success() );
            Assert.AreEqual("descriptor", transaction.Descriptor);
            Assert.AreEqual("custom_data", transaction.Custom);
            Assert.AreEqual("ABC123"+rand, transaction.BillingReference);
            Assert.AreEqual("Customer (123)", transaction.CustomerReference);
        }

        [Test]
        public void AuthorizeFailuresShouldReturnProcessorTransactionDeclined() {
            var transaction = Processor.TheProcessor.Authorize(defaultPaymentMethod.PaymentMethodToken,
                                                               "1.02", new TransactionPayload() { BillingReference = rand });
            Assert.IsFalse( transaction.Success() );
            Assert.AreEqual( new List<string>(){"The card was declined."}, transaction.Errors["processor.transaction"] );
        }
        [Test]
        public void AuthorizeFailuresShouldReturnInputAmountInvalid() {
            var transaction = Processor.TheProcessor.Authorize(defaultPaymentMethod.PaymentMethodToken,
                                                               "1.10", new TransactionPayload() { BillingReference = rand });
            Assert.IsFalse( transaction.Success() );
            Assert.AreEqual( new List<string>(){"The transaction amount was invalid."}, transaction.Errors["input.amount"] );
        }
        /*
        [Test]
        public void AuthorizeFailuresShouldReturnInputCardNumberFailedChecksum() {
            defaultPaymentMethod = PaymentMethod.Create(defaultPayload.Merge(new PaymentMethodPayload() { CardNumber = "1234123412341234" }));
            var transaction = Processor.TheProcessor.Authorize(defaultPaymentMethod.PaymentMethodToken,
                                                               "1.00", new TransactionPayload() { BillingReference = rand });
            Assert.IsFalse( transaction.Success() );
            Assert.AreEqual( new List<string>(){"The card number was invalid."}, transaction.Errors["input.card_number"] );
        }
        [Test]
        public void AuthorizeFailuresShouldReturnInputCardNumberInvalid() {
            defaultPaymentMethod = PaymentMethod.Create(defaultPayload.Merge(new PaymentMethodPayload() { CardNumber = "5105105105105100" }));
            var transaction = Processor.TheProcessor.Authorize(defaultPaymentMethod.PaymentMethodToken,
                                                               "1.00", new TransactionPayload() { BillingReference = rand });
            Assert.IsFalse( transaction.Success() );
            Assert.AreEqual( new List<string>(){"The card number was invalid."}, transaction.Errors["input.card_number"] );
        }
        */

        [Test]
        public void AuthorizeCvvResponsesShouldReturnProcessorCvvResultCodeM() {
            defaultPaymentMethod = PaymentMethod.Create(defaultPayload.Merge(new PaymentMethodPayload() { Cvv = "111" }));
            var transaction = Processor.TheProcessor.Authorize(defaultPaymentMethod.PaymentMethodToken,
                                                               "1.00", new TransactionPayload() { BillingReference = rand });
            Assert.IsTrue( transaction.Success() );
            Assert.AreEqual( "M", transaction.ProcessorResponse.CvvResultCode );
        }
        [Test]
        public void AuthorizeCvvResponsesShouldReturnProcessorCvvResultCodeN() {
            defaultPaymentMethod = PaymentMethod.Create(defaultPayload.Merge(new PaymentMethodPayload() { Cvv = "222" }));
            var transaction = Processor.TheProcessor.Authorize(defaultPaymentMethod.PaymentMethodToken,
                                                               "1.00", new TransactionPayload() { BillingReference = rand });
            Assert.IsTrue( transaction.Success() );
            Assert.AreEqual( "N", transaction.ProcessorResponse.CvvResultCode );
        }
        [Test]
        public void AuthorizeAvsResponsesShouldReturnProcessorAvsResultCodeY() {
            defaultPaymentMethod = PaymentMethod.Create(defaultPayload.Merge(new PaymentMethodPayload() {
                Address1 = "1000 1st Av",
                Address2 = "",
                Zip = "10101"
            }));
            var transaction = Processor.TheProcessor.Authorize(defaultPaymentMethod.PaymentMethodToken,
                                                               "1.00", new TransactionPayload() { BillingReference = rand });
            Assert.IsTrue( transaction.Success() );
            Assert.AreEqual( "Y", transaction.ProcessorResponse.AvsResultCode );
        }
        [Test]
        public void AuthorizeAvsResponsesShouldReturnProcessorAvsResultCodeZ() {
            defaultPaymentMethod = PaymentMethod.Create(defaultPayload.Merge(new PaymentMethodPayload() {
                Address1 = "",
                Address2 = "",
                Zip = "10101"
            }));
            var transaction = Processor.TheProcessor.Authorize(defaultPaymentMethod.PaymentMethodToken,
                                                               "1.00", new TransactionPayload() { BillingReference = rand });
            Assert.IsTrue( transaction.Success() );
            Assert.AreEqual( "Z", transaction.ProcessorResponse.AvsResultCode );
        }
        [Test]
        public void AuthorizeAvsResponsesShouldReturnProcessorAvsResultCodeN() {
            defaultPaymentMethod = PaymentMethod.Create(defaultPayload.Merge(new PaymentMethodPayload() {
                Address1 = "123 Main St",
                Address2 = "",
                Zip = "60610"
            }));
            var transaction = Processor.TheProcessor.Authorize(defaultPaymentMethod.PaymentMethodToken,
                                                               "1.00", new TransactionPayload() { BillingReference = rand });
            Assert.IsTrue( transaction.Success() );
            Assert.AreEqual( "N", transaction.ProcessorResponse.AvsResultCode );
        }

    }
}
