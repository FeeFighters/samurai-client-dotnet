using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Samurai.NUnitTest
{
    public static class TestHelper
    {
        public static PaymentMethod CreatePaymentMethod(PaymentMethodPayload payload = null)
        {
            PaymentMethodPayload dummy = new PaymentMethodPayload()
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
            dummy.Merge(payload);
            return PaymentMethod.Create(dummy);
        }
    }
}
