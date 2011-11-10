using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace Samurai
{
    /// <summary>
    /// Represents payment method.
    /// </summary>
    public class PaymentMethod : SamuraiBase
    {
        /// <summary>
        /// Gets or sets payment method token.
        /// </summary>
        public string PaymentMethodToken { get; set; }

        /// <summary>
        /// Gets or sets date and time when the payment method was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets date and time when payment method was updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets custom data.
        /// </summary>
        public string Custom { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether the payment method is retained.
        /// </summary>
        public bool IsRetained { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether the payment method is redacted.
        /// </summary>
        public bool IsRedacted { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether sensitive data (card number and cvv) are valid.
        /// </summary>
        public bool IsSensitiveDataValid { get; set; }

        /// <summary>
        /// Gets or sets a list of messages associated with this payment method.
        /// </summary>
        public List<Message> Messages { get; set; }

        /// <summary>
        /// Gets or sets last four digits of associated card number.
        /// </summary>
        public string LastFourDigits { get; set; }

        /// <summary>
        /// Gets or sets last four digits of associated card number.
        /// </summary>
        public string CardNumber { get; set; }

        /// <summary>
        /// Gets or sets last four digits of associated card number.
        /// </summary>
        public string Cvv { get; set; }

        /// <summary>
        /// Gets or sets card type of associated card.
        /// </summary>
        public string CardType { get; set; }

        /// <summary>
        /// Gets or sets first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets month of expiration.
        /// </summary>
        public int ExpiryMonth { get; set; }

        /// <summary>
        /// Gets or sets year of expiration.
        /// </summary>
        public int ExpiryYear { get; set; }

        /// <summary>
        /// Gets or sets address 1.
        /// </summary>
        public string Address1 { get; set; }

        /// <summary>
        /// Gets or sets address 2.
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// Gets or sets city.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets state.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets zip.
        /// </summary>
        public string Zip { get; set; }

        /// <summary>
        /// Gets or sets country.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Fetches payment method by its token.
        /// </summary>
        /// <param name="paymentMethodToken">Payment method token.</param>
        /// <returns>payment method with given token.</returns>
        public static PaymentMethod Fetch(string paymentMethodToken)
        {
            // create request
            var request = new RestRequest();
            request.Resource = "payment_methods/{PaymentMethodToken}.xml";
            request.RootElement = "payment_method";

            // add token as an url parameter
            request.AddParameter("PaymentMethodToken", paymentMethodToken, ParameterType.UrlSegment);

            return Execute<PaymentMethod>(request);
        }

        /// <summary>
        /// Uploads payment method changes onto server. Only properties that have been
        /// changed will be uploaded.
        /// </summary>
        /// <remarks>
        /// This method will be fetch original payment method by token and compare original
        /// payment method with the current payment method.
        /// </remarks>
        /// <returns>an updated payment method.</returns>
        public PaymentMethod Update()
        {
            // get old
            var oldPM = Fetch(PaymentMethodToken);

            // create root element
            var root = new XElement("payment_method");

            // add custom data if changed
            if (Custom != oldPM.Custom) { root.Add(new XElement("custom", Custom)); }
            // add name if changed
            if (FirstName != oldPM.FirstName) { root.Add(new XElement("first_name", FirstName)); }
            if (LastName != oldPM.LastName) { root.Add(new XElement("last_name", LastName)); }
            // add street address if changed
            if (Address1 != oldPM.Address1) { root.Add(new XElement("address_1", Address1)); }
            if (Address2 != oldPM.Address2) { root.Add(new XElement("address_2", Address2)); }
            // add address if changed
            if (City != oldPM.City) { root.Add(new XElement("city", City)); }
            if (State != oldPM.State) { root.Add(new XElement("state", State)); }
            if (Zip != oldPM.Zip) { root.Add(new XElement("zip", Zip)); }
            if (Country != oldPM.Country) { root.Add(new XElement("country", Country)); }
            // add info about card type if changed
            if (CardType != oldPM.CardType) { root.Add(new XElement("card_type", CardType)); }
            // add expiring info if changed
            if (ExpiryMonth != oldPM.ExpiryMonth) { root.Add(new XElement("expiry_month", ExpiryMonth)); }
            if (ExpiryYear != oldPM.ExpiryYear) { root.Add(new XElement("expiry_year", ExpiryYear)); }

            // create doc based on root element
            var doc = new XDocument(root);

            // create request
            var request = new RestRequest();
            request.Resource = "payment_methods/{PaymentMethodToken}.xml";
            request.Method = Method.PUT;
            request.RootElement = "payment_method";

            // add token as an url parameter
            request.AddParameter("PaymentMethodToken", PaymentMethodToken, ParameterType.UrlSegment);
            // add xml payload as a request body
            request.AddParameter("text/xml", doc.ToString(), ParameterType.RequestBody);

            return Execute<PaymentMethod>(request);
        }

        /// <summary>
        /// Creates a brand new payment method with given parameters and returns its token.
        /// </summary>
        /// <param name="firstName">First name.</param>
        /// <param name="lastName">Last name.</param>
        /// <param name="city">City.</param>
        /// <param name="state">State.</param>
        /// <param name="zip">Zip.</param>
        /// <param name="cardNumber">Card number.</param>
        /// <param name="cardCVV">Security code</param>
        /// <param name="expMonth">Month of the expiration date.</param>
        /// <param name="expYear">Year of the expiration date.</param>
        /// <param name="sandbox">Sandbox payment method flag (sandbox PMs can only be used with Sandbox Processors)</param>
        /// <param name="redirect_url">Url to redirect to from transparent redirect</param>
        /// <returns>token of a brand new payment method.</returns>
        public static string CreateNewPaymentMethodToken(string firstName, string lastName, string city, string state,
            string zip, string cardNumber, string cardCVV, string expMonth, string expYear, bool sandbox = true, string redirect_url = null)
        {
            // client for creating
            var client = new RestClient();
            client.BaseUrl = Samurai.Site; //"https://api.samurai.feefighters.com/v1/";
            
            // create post-request 
            var request = new RestRequest(Method.POST);
            request.Timeout = int.MaxValue;
            request.Resource = "payment_methods/tokenize";
			request.AddParameter("Accept", "application/json", ParameterType.HttpHeader);
			
            // it seems like for redirecting IIS should be installed
            request.AddParameter("redirect_url", string.IsNullOrWhiteSpace(redirect_url) ? "http://127.0.0.1:80" : redirect_url);
            request.AddParameter("merchant_key", Samurai.MerchantKey);

            request.AddParameter("credit_card[first_name]", firstName);
            request.AddParameter("custom", "");
            request.AddParameter("credit_card[last_name]", lastName);
            
            request.AddParameter("credit_card[city]", city);
            request.AddParameter("credit_card[state]", state);
            request.AddParameter("credit_card[zip]", zip);
            
            request.AddParameter("credit_card[card_number]", cardNumber);
            request.AddParameter("credit_card[cvv]", cardCVV);
            request.AddParameter("credit_card[expiry_month]", expMonth);
            request.AddParameter("credit_card[expiry_year]", expYear);
            
            if (sandbox)
            {
                request.AddParameter("sandbox", "true");
            }

            // get response
            var response = client.Execute(request);

            // get token from url
            var match = Regex.Match(response.Content, "\"payment_method_token\":\\s*\"(\\w+)\"", RegexOptions.IgnoreCase);
            return match.Groups[1].ToString();
        }


        /// <summary>
        /// Creates a brand new payment method with given parameters.
        /// </summary>
        /// <param name="firstName">First name.</param>
        /// <param name="lastName">Last name.</param>
        /// <param name="city">City.</param>
        /// <param name="state">State.</param>
        /// <param name="zip">Zip.</param>
        /// <param name="cardNumber">Card number.</param>
        /// <param name="cardCVV">Security code</param>
        /// <param name="expMonth">Month of the expiration date.</param>
        /// <param name="expYear">Year of the expiration date.</param>
        /// <returns>a brand new payment method.</returns>
        public static PaymentMethod Create(string firstName, string lastName, string city, string state,
            string zip, string cardNumber, string cardCVV, string expMonth, string expYear, bool sandbox = true)
        {
            // create a payment method
            string pmToken = CreateNewPaymentMethodToken(firstName, lastName, city, state, zip,
                cardNumber, cardCVV, expMonth, expYear, sandbox);

            // fetch it ny its token
            PaymentMethod pm = PaymentMethod.Fetch(pmToken);

            return pm;
        }

        /// <summary>
        /// Redacts sensitive information from the payment method, rendering it unusable.
        /// </summary>
        /// <returns>reducted payment method.</returns>
        public PaymentMethod Redact()
        {
            // create a request
            var request = new RestRequest(Method.POST);
            request.Resource = "payment_methods/{PaymentMethodToken}/redact.xml";
            request.RootElement = "payment_method";

            // add token as an url parameter
            request.AddParameter("PaymentMethodToken", PaymentMethodToken, ParameterType.UrlSegment);

            return Execute<PaymentMethod>(request);
        }

        /// <summary>
        /// Retains the payment method on api.samurai.feefighters.com. Retain a payment method 
        /// if it will not be used immediately. 
        /// </summary>
        /// <returns>retained payment method.</returns>
        public PaymentMethod Retain()
        {
            // create a request
            var request = new RestRequest(Method.POST);
            request.Resource = "payment_methods/{PaymentMethodToken}/retain.xml";
            request.RootElement = "payment_method";

            // add token as an url parameter
            request.AddParameter("PaymentMethodToken", PaymentMethodToken, ParameterType.UrlSegment);

            return Execute<PaymentMethod>(request);
        }
    }
}
