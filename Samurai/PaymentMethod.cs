using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;
using System.Xml.Linq;

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

        public List<Message> Messages { get; set; }

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

        public PaymentMethod Update()
        {
            // get old
            var oldPM = Fetch(PaymentMethodToken);

            // create root element
            var root = new XElement("payment_method");

            // custom data
            if (Custom != oldPM.Custom) { root.Add(new XElement("custom", Custom)); }
            // name
            if (FirstName != oldPM.FirstName) { root.Add(new XElement("first_name", FirstName)); }
            if (LastName != oldPM.LastName) { root.Add(new XElement("last_name", LastName)); }
            // address
            if (Address1 != oldPM.Address1) { root.Add(new XElement("address_1", Address1)); }
            if (Address2 != oldPM.Address2) { root.Add(new XElement("address_2", Address2)); }
            // address
            if (City != oldPM.City) { root.Add(new XElement("city", City)); }
            if (State != oldPM.State) { root.Add(new XElement("state", State)); }
            if (Zip != oldPM.Zip) { root.Add(new XElement("zip", Zip)); }
            if (Country != oldPM.Country) { root.Add(new XElement("country", Country)); }
            // about card
            if (CardType != oldPM.CardType) { root.Add(new XElement("card_type", CardType)); }
            // expiring
            if (ExpiryMonth != oldPM.ExpiryMonth) { root.Add(new XElement("expiry_month", ExpiryMonth)); }
            if (ExpiryYear != oldPM.ExpiryYear) { root.Add(new XElement("expiry_year", ExpiryYear)); }

            // create doc based on root element
            var doc = new XDocument(root);

            // create request
            var request = new RestRequest();
            request.Resource = "payment_methods/{PaymentMethodToken}.xml";
            request.Method = Method.PUT;
            request.RootElement = "payment_method";

            request.AddParameter("PaymentMethodToken", PaymentMethodToken, ParameterType.UrlSegment);
            request.AddParameter("text/xml", doc.ToString(), ParameterType.RequestBody);

            return Execute<PaymentMethod>(request);
        }
    }
}
