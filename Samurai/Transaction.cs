using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;

namespace Samurai
{
    /// <summary>
    /// Represents a transaction.
    /// </summary>
    public class Transaction : SamuraiBase
    {
        /// <summary>
        /// Gets or sets transaction reference id.
        /// </summary>
        public string ReferenceId { get; set; }

        /// <summary>
        /// Gets or sets transaction token.
        /// </summary>
        public string TransactionToken { get; set; }

        /// <summary>
        /// Gets or sets time when transaction was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets descriptor.
        /// </summary>
        public string Descriptor { get; set; }

        /// <summary>
        /// Gets or sets custom data.
        /// </summary>
        public string Custom { get; set; }

        /// <summary>
        /// Gets or sets transaction type.
        /// </summary>
        public string TransactionType { get; set; }

        /// <summary>
        /// Gets or sets amount.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets currency code. "USD" for example.
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets processor token.
        /// </summary>
        public string ProcessorToken { get; set; }

        /// <summary>
        /// Gets or sets processor response.
        /// </summary>
        public ProcessorResponse ProcessorResponse { get; set; }

        /// <summary>
        /// Gets or sets payment method.
        /// </summary>
        public PaymentMethod PaymentMethod { get; set; }

        /// <summary>
        /// Fetches a transaction.
        /// </summary>
        /// <param name="purchaseReferenceId">Transaction reference id.</param>
        /// <returns>a transaction with given reference id.</returns>
        public static Transaction Fetch(string purchaseReferenceId)
        {
            // create a request
            var request = new RestRequest();
            request.Resource = "transactions/{ReferenceID}.xml";
            request.RootElement = "transaction";

            // add reference is as an url parameter
            request.AddParameter("ReferenceID", purchaseReferenceId, ParameterType.UrlSegment);

            return Execute<Transaction>(request);
        }

        /// <summary>
        /// Captures an authorization. Optionally specify an amount to do a partial 
        /// capture of the initial authorization. The default is to capture the full 
        /// amount of the authorization.
        /// </summary>
        /// <remarks>
        /// If amount is not specefied Capture(string=null) method will be used.
        /// </remarks>
        /// <param name="amount">Amount of partial capture, specify only if needed.</param>
        /// <returns>Captured transaction.</returns>
        public Transaction Capture(decimal amount)
        {
            string amountString = Helper.DecimalToString(amount);
            return Capture(amountString);
        }

        /// <summary>
        /// Captures an authorization. Optionally specify an amount to do a partial 
        /// capture of the initial authorization. The default is to capture the full 
        /// amount of the authorization.
        /// </summary>
        /// <param name="amount">Amount of partial capture, specify only if needed.</param>
        /// <returns>Captured transaction.</returns>
        public Transaction Capture(string amount=null)
        {
            // create request
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Xml;
            request.Resource = "transactions/{TransactionToken}/capture.xml";
            request.RootElement = "transaction";

            // add transaction token as an url parameter
            request.AddParameter("TransactionToken", TransactionToken, ParameterType.UrlSegment);

            // generate payload
            string amountToPayload = !string.IsNullOrWhiteSpace(amount) ? 
                                amount : Amount.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            string payload = string.Format("<amount>{0}</amount>", amountToPayload);
            // add payload as a request body
            request.AddParameter("text/xml", payload, ParameterType.RequestBody);

            return Execute<Transaction>(request);
        }

        /// <summary>
        /// Voids this transaction. If the transaction has not yet been captured and settled 
        /// it can be voided to prevent any funds from transferring.
        /// </summary>
        /// <returns>Voided transaction.</returns>
        public Transaction Void()
        {
            // create request
            var request = new RestRequest(Method.POST);
            request.Resource = "transactions/{TransactionToken}/void.xml";
            request.RootElement = "transaction";

            // add processor token as an url parameter
            request.AddParameter("TransactionToken", TransactionToken, ParameterType.UrlSegment);

            return Execute<Transaction>(request);
        }
    }
}
