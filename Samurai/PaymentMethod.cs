using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;

namespace Samurai
{
    public class PaymentMethod : SamuraiBase
    {
        public string PaymentMethodToken { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Custom { get; set; }
        public bool IsRetained { get; set; }
        public bool IsRedacted { get; set; }
        public bool IsSensitiveDataValid { get; set; }

        // ... messages

        public string LastFourDigits { get; set; }
        public string CardType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }

        public static PaymentMethod Fetch(string paymentMethodToken)
        {
            var request = new RestRequest();
            request.Resource = "payment_methods/{PaymentMethodToken}.xml";
            request.RootElement = "payment_method";

            request.AddParameter("PaymentMethodToken", paymentMethodToken, ParameterType.UrlSegment);

            return Execute<PaymentMethod>(request);
        }
    }
}
