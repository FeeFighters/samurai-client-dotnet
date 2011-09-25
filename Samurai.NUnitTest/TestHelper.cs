using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Samurai.NUnitTest
{
    public static class TestHelper
    {
        public static PaymentMethod CreateScoobyDoPaymentMethod()
        {
            return PaymentMethod.Create("Scooby", "Do", "Mystery Van", "IL", "60607",
                "4111111111111111", "123", "04", "2014");
        }
    }
}
