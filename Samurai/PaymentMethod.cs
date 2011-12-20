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
        /// Gets or sets a value that indicates whether the expiration date is valid.
        /// </summary>
        public bool IsExpirationValid { get; set; }

        /// <summary>
        /// Gets or sets a list of messages associated with this payment method.
        /// </summary>
        public List<Message> Messages { get; set; }

        /// <summary>
        /// Gets or sets a list of errors associated with this payment method.
        /// </summary>
        Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string>> Errors { get { return _errors; } }

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
        /// Gets or sets sandbox.
        /// </summary>
        public bool Sandbox { get; set; }

        /// <summary>
        /// Fetches payment method by its token.
        /// </summary>
        /// <param name="paymentMethodToken">Payment method token.</param>
        /// <returns>payment method with given token.</returns>
        public static PaymentMethod Find(string paymentMethodToken)
        {
            // create request
            var request = new RestRequest();
            request.Resource = "payment_methods/{PaymentMethodToken}.xml";
            request.RootElement = "payment_method";

            // add token as an url parameter
            request.AddParameter("PaymentMethodToken", paymentMethodToken, ParameterType.UrlSegment);

            var foundPM = Execute(request);
            return (foundPM.Errors.ContainsKey("system.general")) ? null : foundPM;
        }

        /// <summary>
        /// Update the payment method with the provided payload attributes
        /// </summary>
        /// <remarks>
        /// This method will set the attributes of this payment method. It does not
        /// persist those changes to the server.
        /// </remarks>
        public void SetAttributes(PaymentMethodPayload payload)
        {
            FirstName = payload.FirstName;
            LastName = payload.LastName;
            Address1 = payload.Address1;
            Address2 = payload.Address2;
            City = payload.City;
            State = payload.State;
            Zip = payload.Zip;
            CardNumber = payload.CardNumber;
            Cvv = payload.Cvv;
            ExpiryMonth = payload.ExpiryMonth;
            ExpiryYear = payload.ExpiryYear;
            Custom = payload.Custom;
            Sandbox = payload.Sandbox;
        }

        /// <summary>
        /// Update the payment method with the provided payload attributes
        /// </summary>
        /// <remarks>
        /// This method will set the attributes of this payment method.
        /// It will also persist the new changes to the server.
        /// </remarks>
        public void UpdateAttributes(PaymentMethodPayload payload)
        {
            SetAttributes(payload);
            Update();
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
        public void Update()
        {
            // create root element
            var root = new XElement("payment_method");

            root.Add(new XElement("first_name", FirstName));
            root.Add(new XElement("last_name", LastName));
            root.Add(new XElement("address_1", Address1));
            root.Add(new XElement("address_2", Address2));
            root.Add(new XElement("city", City));
            root.Add(new XElement("state", State));
            root.Add(new XElement("zip", Zip));
            root.Add(new XElement("country", Country));
            root.Add(new XElement("card_number", CardNumber));
            root.Add(new XElement("cvv", Cvv));
            root.Add(new XElement("expiry_month", ExpiryMonth));
            root.Add(new XElement("expiry_year", ExpiryYear));
            root.Add(new XElement("custom", Custom));
            root.Add(new XElement("sandbox", Sandbox));

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

            PaymentMethod newPM = Execute(request);
            newPM.CopyPropertiesTo(this);
            ProcessResponseErrors();
        }

        /// <summary>
        /// Creates a brand new payment method with given parameters and returns its token.
        /// </summary>
        /// <param name="payload">Payment Method payload, containing parameters for the new PM</param>
        /// <returns>token of a brand new payment method.</returns>
        public static string TokenizePaymentMethod(PaymentMethodPayload payload)
        {
            // client for creating
            var client = new RestClient();
            client.BaseUrl = Samurai.Site;
            client.UserAgent = "FeeFighters Samurai .NET Client v"+Samurai.Version;

            // create post-request 
            var request = new RestRequest(Method.POST);
            request.Timeout = int.MaxValue;
            request.Resource = "payment_methods/tokenize";
			request.AddParameter("Accept", "application/json", ParameterType.HttpHeader);
			
            request.AddParameter("redirect_url", "http://127.0.0.1:80");
            request.AddParameter("merchant_key", Samurai.MerchantKey);

            request.AddParameter("credit_card[first_name]", payload.FirstName);
            request.AddParameter("credit_card[last_name]", payload.LastName);
            request.AddParameter("credit_card[address_1]", payload.Address1);
            request.AddParameter("credit_card[address_2]", payload.Address2);
            request.AddParameter("credit_card[city]", payload.City);
            request.AddParameter("credit_card[state]", payload.State);
            request.AddParameter("credit_card[zip]", payload.Zip);
            request.AddParameter("credit_card[card_number]", payload.CardNumber);
            request.AddParameter("credit_card[cvv]", payload.Cvv);
            request.AddParameter("credit_card[expiry_month]", payload.ExpiryMonth);
            request.AddParameter("credit_card[expiry_year]", payload.ExpiryYear);
            request.AddParameter("custom", payload.Custom);
            request.AddParameter("sandbox", payload.Sandbox);

            // get response
            if (Samurai.Debug) {
                Console.WriteLine(request.Resource.ToString());
                request.Parameters.ForEach(delegate(RestSharp.Parameter p) {
                    Console.WriteLine(p.ToString());
                });
            }
            var response = client.Execute(request);
            if (Samurai.Debug) {
                Console.WriteLine(response.StatusCode.ToString() + " - " + response.StatusDescription.ToString());
                Console.WriteLine(response.Content.ToString());
            }

            // get token from url
            var match = Regex.Match(response.Content, "\"payment_method_token\":\\s*\"(\\w+)\"", RegexOptions.IgnoreCase);
            return match.Groups[1].ToString();
        }


        /// <summary>
        /// Creates a brand new payment method with given parameters.
        /// </summary>
        /// <param name="payload">Payment Method payload, containing parameters for the new PM</param>
        /// <returns>a brand new payment method.</returns>
        public static PaymentMethod Create(PaymentMethodPayload payload)
        {
            // create a payment method
            string pmToken = TokenizePaymentMethod(payload);

            // fetch it ny its token
            PaymentMethod pm = PaymentMethod.Find(pmToken);

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

            return Execute(request);
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

            return Execute(request);
        }

        protected static PaymentMethod Execute(RestRequest request)
        {
            var pm = Execute<PaymentMethod>(request);
            pm.ProcessResponseErrors();
            return pm;
        }

        protected void ProcessResponseErrors()
        {
            // Sort the messages so that more-critical/relevant ones appear first, since only the first error is added to a field
            Messages.Sort(new MessageComparer());
            foreach (Message message in Messages)
            {
                if (!Errors.ContainsKey(message.Context) || Errors[message.Context].Count == 0) {
                    Errors.Add(message.Context, new List<string>() {message.Description});
                }
            }
        }
    }
}
