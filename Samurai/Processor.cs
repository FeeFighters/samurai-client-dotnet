using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;
using System.Xml.Linq;

namespace Samurai
{
    public class Processor : SamuraiBase
    {
        public Processor(string processorToken)
        {
            ProcessorToken = processorToken;
        }

        public static Processor TheProcessor
        {
            get { return new Processor(Samurai.ProcessorToken); }
        }

        public string ProcessorToken { get; private set; }

        /// <summary>
        /// Most generic form of simple purchase.
        /// </summary>
        /// <param name="paymentMethodToken"></param>
        /// <param name="amount"></param>
        /// <param name="descriptor"></param>
        /// <param name="custom"></param>
        /// <param name="customer_reference"></param>
        /// <param name="billing_reference"></param>
        /// <returns></returns>
        public Transaction Purchase(string paymentMethodToken, string amount, string descriptor = null,
            string custom = null, string customer_reference = null, string billing_reference = null)
        {
            // Create request
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Xml;
            request.Resource = "processors/{ProcessorToken}/purchase.xml";
            request.RootElement = "transaction";

            // Set processor token
            request.AddParameter("ProcessorToken", ProcessorToken, ParameterType.UrlSegment);

            // Generate payload
            request.AddBody(new PurchaseXmlPayload()
            {
                Type = "purchase",
                Amount = amount,
                CurrencyCode = "USD",
                PaymentMethodToken = paymentMethodToken,
                Descriptor = descriptor ?? string.Empty,
                Custom = custom ?? string.Empty,
                CustomerReference = customer_reference ?? string.Empty,
                BillingReference = billing_reference ?? string.Empty
            });

            // Send a request and deserialize response into transaction
            return Execute<Transaction>(request);
        }

        /// <summary>
        /// Authorizes a payment_method for a particular amount. 
        /// </summary>
        /// <param name="paymentMethodToken"></param>
        /// <param name="amount"></param>
        /// <param name="descriptor"></param>
        /// <param name="custom"></param>
        /// <param name="customer_reference"></param>
        /// <param name="billing_reference"></param>
        /// <returns></returns>
        public Transaction Authorize(string paymentMethodToken, string amount, string descriptor = null,
            string custom = null, string customer_reference = null, string billing_reference = null)
        {
            // Create request
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Xml;
            request.Resource = "processors/{ProcessorToken}/authorize.xml";
            request.RootElement = "transaction";

            // Set processor token
            request.AddParameter("ProcessorToken", ProcessorToken, ParameterType.UrlSegment);

            // Generate payload
            request.AddBody(new PurchaseXmlPayload()
            {
                Type = "authorize",
                Amount = amount,
                CurrencyCode = "USD",
                PaymentMethodToken = paymentMethodToken,
                Descriptor = descriptor ?? string.Empty,
                Custom = custom ?? string.Empty,
                CustomerReference = customer_reference ?? string.Empty,
                BillingReference = billing_reference ?? string.Empty
            });

            // Send a request and deserialize response into transaction
            return Execute<Transaction>(request);
        }
    }
}
