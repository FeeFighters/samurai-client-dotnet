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

        [SetUp]
        public void TestInitialize()
        {
            Samurai.Options = new SamuraiOptions()
            {
                MerchantKey = "a1ebafb6da5238fb8a3ac9f6",
                MerchantPassword = "ae1aa640f6b735c4730fbb56",
                ProcessorToken = "69ac9c704329bb067d427bf0",
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
  
}
[Test]
public void S2SCreateFailOnInputCvvShouldReturnTooLong(){
  
}
[Test]
public void S2SCreateFailOnInputCvvShouldReturnNotNumeric(){
  
}

[Test]
public void S2SCreateFailOnInputExpiryMonthShouldReturnIsBlank(){
  
}
[Test]
public void S2SCreateFailOnInputExpiryMonthShouldReturnIsInvalid(){
  
}
[Test]
public void S2SCreateFailOnInputExpiryYearShouldReturnIsBlank(){
  
}
[Test]
public void S2SCreateFailOnInputExpiryYearShouldReturnIsInvalid(){
  
}

[Test]
public void S2SUpdateShouldBeSuccessful()
{
  
}
[Test]
public void S2SUpdateShouldBeSuccessfulPreservingSensitiveData()
{
  
}

[Test]
public void S2SUpdateFailOnInputCardNumberShouldReturnTooShort()
{
  
}
[Test]
public void S2SUpdateFailOnInputCardNumberShouldReturnTooLong()
{
  
}
[Test]
public void S2SUpdateFailOnInputCardNumberShouldReturnFailedChecksum()
{
  
}

[Test]
public void S2SUpdateFailOnInputCvvShouldReturnTooShort()
{
  
}
[Test]
public void S2SUpdateFailOnInputCvvShouldReturnTooLong()
{
  
}
[Test]
public void S2SUpdateFailOnInputCvvShouldReturnNotNumeric()
{
  
}

[Test]
public void S2SUpdateFailOnInputExpiryMonthShouldReturnIsBlank()
{
  
}
[Test]
public void S2SUpdateFailOnInputExpiryMonthShouldReturnIsInvalid()
{
  
}
[Test]
public void S2SUpdateFailOnInputExpiryYearShouldReturnIsBlank()
{
  
}
[Test]
public void S2SUpdateFailOnInputExpiryYearShouldReturnIsInvalid()
{
  
}

[Test]
public void FindShouldBeSuccessful()
{
  
}
[Test]
public void FindShouldFailOnAnInvalidToken()
{
  
}




        [Test]
        public void Fetch_PaymentMethod_Test()
        {
			var stored_pm = TestHelper.CreatePaymentMethod();
            var pm = PaymentMethod.Fetch(stored_pm.PaymentMethodToken);

            Assert.IsNotNull(pm);
            Assert.AreEqual(pm.PaymentMethodToken.ToLower(), stored_pm.PaymentMethodToken.ToLower());
            Assert.IsFalse(pm.IsRetained);
            Assert.IsFalse(pm.IsRedacted);
            Assert.IsTrue(pm.IsSensitiveDataValid);
            Assert.AreEqual(pm.LastFourDigits, "1111");
            // write more...
        }

        [Test]
        public void Update_PaymentMethod_Test()
        {
            // fetch
			var stored_pm = TestHelper.CreatePaymentMethod();
            string oldCustom = stored_pm.Custom;

            // updating
            string newCustom = string.Format("Updated at {0}.", DateTime.UtcNow);
            stored_pm.Custom = newCustom;
            stored_pm.Update();

            var pm = PaymentMethod.Fetch(stored_pm.PaymentMethodToken);

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
            var pm = TestHelper.CreatePaymentMethod();

            Assert.IsTrue(pm.IsSensitiveDataValid);
            Assert.IsFalse(pm.IsRetained);
            Assert.IsFalse(pm.IsRedacted);

            // redact
            var redactedPM = pm.Redact();

            Assert.IsTrue(redactedPM.IsRedacted);
            Assert.IsTrue(redactedPM.IsSensitiveDataValid);
            Assert.IsFalse(pm.IsRetained);
        }

        [Test]
        public void Create_Then_Retain_Then_Redact_PaymentMethod_Test()
        {
            // create new
            var pm = TestHelper.CreatePaymentMethod();

            Assert.IsTrue(pm.IsSensitiveDataValid);
            Assert.IsFalse(pm.IsRetained);
            Assert.IsFalse(pm.IsRedacted);

            // redact
            var retainedPM = pm.Retain();

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
