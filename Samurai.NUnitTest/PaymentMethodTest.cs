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
        PaymentMethodPayload defaultPayload;
        PaymentMethodPayload defaultUpdatePayload;

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

            defaultUpdatePayload = new PaymentMethodPayload()
            {
                FirstName = "FirstNameX",
                LastName = "LastNameX",
                Address1 = "123 Main St.X",
                Address2 = "Apt #3X",
                City = "ChicagoX",
                State = "IL",
                Zip = "10101",
                CardNumber = "5454-5454-5454-5454",
                Cvv = "456",
                ExpiryMonth = 5,
                ExpiryYear = 2016,
                Custom = "customX",
                Sandbox = true
            };
        }

        [TearDown]
        public void TestCleanup()
        {
            Samurai.Options = Samurai.DefaultOptions;
        }

        [Test]
        public void S2SCreateShouldBeSuccessful()
        {
            var pm = TestHelper.CreatePaymentMethod(defaultPayload);
            
            Assert.IsTrue(pm.IsSensitiveDataValid);
            Assert.IsTrue(pm.IsExpirationValid);
            Assert.AreEqual(defaultPayload.FirstName, pm.FirstName);
            Assert.AreEqual(defaultPayload.LastName, pm.LastName);
            Assert.AreEqual(defaultPayload.Address1, pm.Address1);
            Assert.AreEqual(defaultPayload.Address2, pm.Address2);
            Assert.AreEqual(defaultPayload.City, pm.City);
            Assert.AreEqual(defaultPayload.State, pm.State);
            Assert.AreEqual(defaultPayload.Zip, pm.Zip);
            Assert.AreEqual(defaultPayload.CardNumber.Substring(defaultPayload.CardNumber.Length - 4), pm.LastFourDigits);
            Assert.AreEqual(defaultPayload.ExpiryMonth, pm.ExpiryMonth);
            Assert.AreEqual(defaultPayload.ExpiryYear, pm.ExpiryYear);
        }
        
        [Test]
        public void S2SCreateFailOnInputCardNumberShouldReturnIsBlank(){
            PaymentMethodPayload attrs = defaultPayload.Merge(new PaymentMethodPayload(){ CardNumber = "" });
            var pm = TestHelper.CreatePaymentMethod(attrs);
        
            Assert.IsFalse( pm.IsSensitiveDataValid );
            Assert.AreEqual( new List<string>(){"The card number was blank."}, pm.Errors["input.card_number"] );
        }
        [Test]
        public void S2SCreateFailOnInputCardNumberShouldReturnTooShort(){
            PaymentMethodPayload attrs = defaultPayload.Merge(new PaymentMethodPayload(){ CardNumber = "4111-1" });
            var pm = TestHelper.CreatePaymentMethod(attrs);
        
            Assert.IsFalse( pm.IsSensitiveDataValid );
            Assert.AreEqual( new List<string>(){"The card number was too short."}, pm.Errors["input.card_number"] );
        }
        [Test]
        public void S2SCreateFailOnInputCardNumberShouldReturnTooLong(){
            PaymentMethodPayload attrs = defaultPayload.Merge(new PaymentMethodPayload(){ CardNumber = "4111-1111-1111-1111-11" });
            var pm = TestHelper.CreatePaymentMethod(attrs);
        
            Assert.IsFalse( pm.IsSensitiveDataValid );
            Assert.AreEqual( new List<string>(){"The card number was too long."}, pm.Errors["input.card_number"] );
        }
        [Test]
        public void S2SCreateFailOnInputCardNumberShouldReturnFailedChecksum(){
            PaymentMethodPayload attrs = defaultPayload.Merge(new PaymentMethodPayload(){ CardNumber = "4111-1111-1111-1234" });
            var pm = TestHelper.CreatePaymentMethod(attrs);
        
            Assert.IsFalse( pm.IsSensitiveDataValid );
            Assert.AreEqual( new List<string>(){"The card number was invalid."}, pm.Errors["input.card_number"] );
        }
        
        [Test]
        public void S2SCreateFailOnInputCvvShouldReturnTooShort(){
            PaymentMethodPayload attrs = defaultPayload.Merge(new PaymentMethodPayload(){ Cvv = "1" });
            var pm = TestHelper.CreatePaymentMethod(attrs);
        
            Assert.IsFalse( pm.IsSensitiveDataValid );
            Assert.AreEqual( new List<string>(){"The CVV was too short."}, pm.Errors["input.cvv"] );
        }
        [Test]
        public void S2SCreateFailOnInputCvvShouldReturnTooLong(){
            PaymentMethodPayload attrs = defaultPayload.Merge(new PaymentMethodPayload(){ Cvv = "111111" });
            var pm = TestHelper.CreatePaymentMethod(attrs);
        
            Assert.IsFalse( pm.IsSensitiveDataValid );
            Assert.AreEqual( new List<string>(){"The CVV was too long."}, pm.Errors["input.cvv"] );
        }
        [Test]
        public void S2SCreateFailOnInputCvvShouldReturnNotNumeric(){
            PaymentMethodPayload attrs = defaultPayload.Merge(new PaymentMethodPayload(){ Cvv = "abcd1" });
            var pm = TestHelper.CreatePaymentMethod(attrs);
        
            Assert.IsFalse( pm.IsSensitiveDataValid );
            Assert.AreEqual( new List<string>(){"The CVV was invalid."}, pm.Errors["input.cvv"] );
        }
        
        [Test]
        public void S2SCreateFailOnInputExpiryMonthShouldReturnIsInvalid(){
            PaymentMethodPayload attrs = defaultPayload.Merge(new PaymentMethodPayload(){ ExpiryMonth = -1 });
            var pm = TestHelper.CreatePaymentMethod(attrs);
        
            Assert.IsTrue( pm.IsSensitiveDataValid );
            Assert.IsFalse( pm.IsExpirationValid );
            Assert.AreEqual( new List<string>(){"The expiration month was invalid."}, pm.Errors["input.expiry_month"] );
        }
        [Test]
        public void S2SCreateFailOnInputExpiryYearShouldReturnIsInvalid(){
            PaymentMethodPayload attrs = defaultPayload.Merge(new PaymentMethodPayload(){ ExpiryYear = -1 });
            var pm = TestHelper.CreatePaymentMethod(attrs);
        
            Assert.IsTrue( pm.IsSensitiveDataValid );
            Assert.IsFalse( pm.IsExpirationValid );
            Assert.AreEqual( new List<string>(){"The expiration year was invalid."}, pm.Errors["input.expiry_year"] );
        }
        
        [Test]
        public void S2SUpdateShouldBeSuccessful()
        {
            var pm = TestHelper.CreatePaymentMethod(defaultPayload);
            pm.UpdateAttributes(defaultUpdatePayload);
        
            Assert.IsTrue(pm.IsSensitiveDataValid);
            Assert.IsTrue(pm.IsExpirationValid);
            Assert.AreEqual(defaultUpdatePayload.FirstName, pm.FirstName);
            Assert.AreEqual(defaultUpdatePayload.LastName, pm.LastName);
            Assert.AreEqual(defaultUpdatePayload.Address1, pm.Address1);
            Assert.AreEqual(defaultUpdatePayload.Address2, pm.Address2);
            Assert.AreEqual(defaultUpdatePayload.City, pm.City);
            Assert.AreEqual(defaultUpdatePayload.State, pm.State);
            Assert.AreEqual(defaultUpdatePayload.Zip, pm.Zip);
            Assert.AreEqual(defaultUpdatePayload.CardNumber.Substring(defaultUpdatePayload.CardNumber.Length - 4), pm.LastFourDigits);
            Assert.AreEqual(defaultUpdatePayload.ExpiryMonth, pm.ExpiryMonth);
            Assert.AreEqual(defaultUpdatePayload.ExpiryYear, pm.ExpiryYear);
        }
        [Test]
        public void S2SUpdateShouldBeSuccessfulPreservingSensitiveData()
        {
            var pm = TestHelper.CreatePaymentMethod(defaultPayload);
            defaultUpdatePayload = defaultUpdatePayload.Merge(new PaymentMethodPayload(){
                CardNumber = "****************",
                Cvv = "***"
            });
            pm.UpdateAttributes(defaultUpdatePayload);
        
            Assert.IsTrue(pm.IsSensitiveDataValid);
            Assert.IsTrue(pm.IsExpirationValid);
            Assert.AreEqual(defaultUpdatePayload.FirstName, pm.FirstName);
            Assert.AreEqual(defaultUpdatePayload.LastName, pm.LastName);
            Assert.AreEqual(defaultUpdatePayload.Address1, pm.Address1);
            Assert.AreEqual(defaultUpdatePayload.Address2, pm.Address2);
            Assert.AreEqual(defaultUpdatePayload.City, pm.City);
            Assert.AreEqual(defaultUpdatePayload.State, pm.State);
            Assert.AreEqual(defaultUpdatePayload.Zip, pm.Zip);
            Assert.AreEqual(defaultPayload.CardNumber.Substring(defaultPayload.CardNumber.Length - 4), pm.LastFourDigits);
            Assert.AreEqual(defaultUpdatePayload.ExpiryMonth, pm.ExpiryMonth);
            Assert.AreEqual(defaultUpdatePayload.ExpiryYear, pm.ExpiryYear);
        }
        
        [Test]
        public void S2SUpdateFailOnInputCardNumberShouldReturnTooShort()
        {
            var pm = TestHelper.CreatePaymentMethod(defaultPayload);
            pm.UpdateAttributes(defaultPayload.Merge(new PaymentMethodPayload(){ CardNumber = "4111-1" }));
        
            Assert.IsFalse( pm.IsSensitiveDataValid );
            Assert.AreEqual( new List<string>(){"The card number was too short."}, pm.Errors["input.card_number"] );
        }
        [Test]
        public void S2SUpdateFailOnInputCardNumberShouldReturnTooLong()
        {
            var pm = TestHelper.CreatePaymentMethod(defaultPayload);
            pm.UpdateAttributes(defaultPayload.Merge(new PaymentMethodPayload(){ CardNumber = "4111-1111-1111-1111-11" }));
        
            Assert.IsFalse( pm.IsSensitiveDataValid );
            Assert.AreEqual( new List<string>(){"The card number was too long."}, pm.Errors["input.card_number"] );
        }
        [Test]
        public void S2SUpdateFailOnInputCardNumberShouldReturnFailedChecksum()
        {
            var pm = TestHelper.CreatePaymentMethod(defaultPayload);
            pm.UpdateAttributes(defaultPayload.Merge(new PaymentMethodPayload(){ CardNumber = "4111-1111-1111-1234" }));
        
            Assert.IsFalse( pm.IsSensitiveDataValid );
            Assert.AreEqual( new List<string>(){"The card number was invalid."}, pm.Errors["input.card_number"] );
        }
        
        [Test]
        public void S2SUpdateFailOnInputCvvShouldReturnTooShort()
        {
            var pm = TestHelper.CreatePaymentMethod(defaultPayload);
            pm.UpdateAttributes(defaultPayload.Merge(new PaymentMethodPayload(){ Cvv = "1" }));
        
            Assert.IsFalse( pm.IsSensitiveDataValid );
            Assert.AreEqual( new List<string>(){"The CVV was too short."}, pm.Errors["input.cvv"] );
        }
        [Test]
        public void S2SUpdateFailOnInputCvvShouldReturnTooLong()
        {
            var pm = TestHelper.CreatePaymentMethod(defaultPayload);
            pm.UpdateAttributes(defaultPayload.Merge(new PaymentMethodPayload(){ Cvv = "111111" }));
        
            Assert.IsFalse( pm.IsSensitiveDataValid );
            Assert.AreEqual( new List<string>(){"The CVV was too long."}, pm.Errors["input.cvv"] );
        }
        [Test]
        public void S2SUpdateFailOnInputCvvShouldReturnNotNumeric()
        {
            var pm = TestHelper.CreatePaymentMethod(defaultPayload);
            pm.UpdateAttributes(defaultPayload.Merge(new PaymentMethodPayload(){ Cvv = "abcd1" }));
        
            Assert.IsFalse( pm.IsSensitiveDataValid );
            Assert.AreEqual( new List<string>(){"The CVV was invalid."}, pm.Errors["input.cvv"] );
        }
        
        [Test]
        public void S2SUpdateFailOnInputExpiryMonthShouldReturnIsInvalid()
        {
            var pm = TestHelper.CreatePaymentMethod(defaultPayload);
            pm.UpdateAttributes(defaultPayload.Merge(new PaymentMethodPayload(){ ExpiryMonth = -1 }));
        
            Assert.IsTrue( pm.IsSensitiveDataValid );
            Assert.IsFalse( pm.IsExpirationValid );
            Assert.AreEqual( new List<string>(){"The expiration month was invalid."}, pm.Errors["input.expiry_month"] );
        }
        [Test]
        public void S2SUpdateFailOnInputExpiryYearShouldReturnIsInvalid()
        {
            var pm = TestHelper.CreatePaymentMethod(defaultPayload);
            pm.UpdateAttributes(defaultPayload.Merge(new PaymentMethodPayload(){ ExpiryYear = -1 }));
        
            Assert.IsTrue( pm.IsSensitiveDataValid );
            Assert.IsFalse( pm.IsExpirationValid );
            Assert.AreEqual( new List<string>(){"The expiration year was invalid."}, pm.Errors["input.expiry_year"] );
        }
        
        [Test]
        public void FindShouldBeSuccessful()
        {
            var pm = TestHelper.CreatePaymentMethod(defaultPayload);
            pm = PaymentMethod.Find(pm.PaymentMethodToken);
        
            Assert.IsTrue(pm.IsSensitiveDataValid);
            Assert.IsTrue(pm.IsExpirationValid);
            Assert.AreEqual(defaultPayload.FirstName, pm.FirstName);
            Assert.AreEqual(defaultPayload.LastName, pm.LastName);
            Assert.AreEqual(defaultPayload.Address1, pm.Address1);
            Assert.AreEqual(defaultPayload.Address2, pm.Address2);
            Assert.AreEqual(defaultPayload.City, pm.City);
            Assert.AreEqual(defaultPayload.State, pm.State);
            Assert.AreEqual(defaultPayload.Zip, pm.Zip);
            Assert.AreEqual(defaultPayload.CardNumber.Substring(defaultPayload.CardNumber.Length - 4), pm.LastFourDigits);
            Assert.AreEqual(defaultPayload.ExpiryMonth, pm.ExpiryMonth);
            Assert.AreEqual(defaultPayload.ExpiryYear, pm.ExpiryYear);
        }
        [Test]
        public void FindShouldFailOnAnInvalidToken()
        {
            PaymentMethod pm = PaymentMethod.Find("abc123");
            Assert.IsNull(pm);
        }
        
    }
}
