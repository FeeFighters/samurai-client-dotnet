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
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="processorToken">Processor token.</param>
        public Processor(string processorToken)
        {
            ProcessorToken = processorToken;
        }

        /// <summary>
        /// Returns the default processor specified by Samurai.ProcessorToken if you passed it into Samurai.Options.
        /// </summary>
        public static Processor TheProcessor
        {
            get { return new Processor(Samurai.ProcessorToken); }
        }

        /// <summary>
        /// Convenience method that calls the purchase method on the default processor.
        /// </summary>
        /// <param name="paymentMethodToken">Token identifying the payment method to authorize.</param>
        /// <param name="amount">Amount to authorize.</param>
        /// <param name="descriptor">Descriptor for the transaction.</param>
        /// <param name="custom">Custom data.</param>
        /// <returns>a transaction containing the processor's response.</returns>
        public static Transaction Purchase(string paymentMethodToken, string amount, string descriptor = null,
            string custom = null)
        {
            return TheProcessor.Purchase(paymentMethodToken, amount, descriptor, custom, null, null);
        }

        /// <summary>
        /// Convenience method that calls the authorize method on the default processor.
        /// </summary>
        /// <param name="paymentMethodToken">Token identifying the payment method to authorize.</param>
        /// <param name="amount">Amount to authorize.</param>
        /// <param name="descriptor">Descriptor for the transaction.</param>
        /// <param name="custom">Custom data.</param>
        /// <returns>a transaction containing the processor's response.</returns>
        public static Transaction Authorize(string paymentMethodToken, string amount, string descriptor = null,
            string custom = null)
        {
            return TheProcessor.Authorize(paymentMethodToken, amount, descriptor, custom, null, null);
        }

        public string ProcessorToken { get; private set; }

        /// <summary>
        /// Convenience method to authorize and capture a payment_method for a particular amount in one transaction.
        /// It's a most generic form of this method.
        /// </summary>
        /// <param name="paymentMethodToken">Token identifying the payment method to authorize.</param>
        /// <param name="amount">Amount to authorize.</param>
        /// <param name="descriptor">Descriptor for the transaction.</param>
        /// <param name="custom">Custom data.</param>
        /// <param name="customer_reference">An identifier for the customer, this will appear in the processor if supported.</param>
        /// <param name="billing_reference">An identifier for the purchase, this will appear in the processor if supported.</param>
        /// <returns>a transaction containing the processor's response.</returns>
        public Transaction Purchase(string paymentMethodToken, string amount, string descriptor = null,
            string custom = null, string customer_reference = null, string billing_reference = null)
        {
            // create request
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Xml;
            request.Resource = "processors/{ProcessorToken}/purchase.xml";
            request.RootElement = "transaction";

            // set processor token
            request.AddParameter("ProcessorToken", ProcessorToken, ParameterType.UrlSegment);

            // generate payload
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

            // send a request and deserialize response into transaction
            return Execute<Transaction>(request);
        }

        /// <summary>
        /// Authorizes a payment_method for a particular amount. 
        /// </summary>
        /// <param name="paymentMethodToken">Token identifying the payment method to authorize.</param>
        /// <param name="amount">Amount to authorize.</param>
        /// <param name="descriptor">Descriptor for the transaction.</param>
        /// <param name="custom">Custom data.</param>
        /// <param name="customer_reference">An identifier for the customer, this will appear in the processor if supported.</param>
        /// <param name="billing_reference">An identifier for the purchase, this will appear in the processor if supported.</param>
        /// <returns>a transaction containing the processor's response.</returns>
        public Transaction Authorize(string paymentMethodToken, string amount, string descriptor = null,
            string custom = null, string customer_reference = null, string billing_reference = null)
        {
            // create request
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Xml;
            request.Resource = "processors/{ProcessorToken}/authorize.xml";
            request.RootElement = "transaction";

            // set processor token
            request.AddParameter("ProcessorToken", ProcessorToken, ParameterType.UrlSegment);

            // generate payload
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

            // send a request and deserialize response into transaction
            return Execute<Transaction>(request);
        }
    }
}
