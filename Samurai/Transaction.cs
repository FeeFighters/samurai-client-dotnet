﻿using System;
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
        /// Gets or sets description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets dynamic descriptor name.
        /// </summary>
        public string DescriptorName { get; set; }

        /// <summary>
        /// Gets or sets dynamic descriptor phone.
        /// </summary>
        public string DescriptorPhone { get; set; }

        /// <summary>
        /// Gets or sets custom data.
        /// </summary>
        public string Custom { get; set; }

        /// <summary>
        /// Gets or sets descriptor.
        /// </summary>
        public string BillingReference { get; set; }

        /// <summary>
        /// Gets or sets custom data.
        /// </summary>
        public string CustomerReference { get; set; }

        /// <summary>
        /// Gets or sets transaction type.
        /// </summary>
        public string TransactionType { get; set; }

        /// <summary>
        /// Gets type of transaction as TransactionType enum.
        /// </summary>
        public TransactionType Type
        {
            get { return Helper.StringToTransactionType(TransactionType); }
        }

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
        /// Gets or sets a list of errors associated with this transaction.
        /// </summary>
        Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string>> Errors { get { return _errors; } }

        /// <summary>
        /// Gets or sets payment method.
        /// </summary>
        public PaymentMethod PaymentMethod { get; set; }

        /// <summary>
        /// Fetches a transaction.
        /// </summary>
        /// <param name="purchaseReferenceId">Transaction reference id.</param>
        /// <returns>a transaction with given reference id.</returns>
        public static Transaction Find(string purchaseReferenceId)
        {
            // create a request
            var request = new RestRequest();
            request.Resource = "transactions/{ReferenceID}.xml";
            request.RootElement = "transaction";

            // add reference is as an url parameter
            request.AddParameter("ReferenceID", purchaseReferenceId, ParameterType.UrlSegment);

            return Execute(request);
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

            return Execute(request);
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

            return Execute(request);
        }

        /// <summary>
        /// Create a credit or refund against the original transaction. Optionally accepts an amount 
        /// to credit, the default is to credit the full value of the original amount.
        /// </summary>
        /// <param name="amount">Amount of partial credit, specify only if needed.</param>
        /// <returns>a credited transaction.</returns>
        public Transaction Credit(string amount = null)
        {
            // create request
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Xml;
            request.Resource = "transactions/{TransactionToken}/credit.xml";
            request.RootElement = "transaction";

            // add transaction token as an url parameter
            request.AddParameter("TransactionToken", TransactionToken, ParameterType.UrlSegment);

            // generate payload
            string amountToPayload = !string.IsNullOrWhiteSpace(amount) ? amount : Helper.DecimalToString(Amount);
            string payload = string.Format("<amount>{0}</amount>", amountToPayload);
            // add payload as a request body
            request.AddParameter("text/xml", payload, ParameterType.RequestBody);

            return Execute(request);
        }
        
        /// <summary>
        /// Create a credit or refund against the original transaction. Optionally accepts an amount 
        /// to credit, the default is to credit the full value of the original amount.
        /// </summary>
        /// <param name="amount">Amount of partial reverse, specify only if needed.</param>
        /// <returns>a reversed transaction.</returns>
        public Transaction Reverse(string amount = null)
        {
            // create request
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Xml;
            request.Resource = "transactions/{TransactionToken}/reverse.xml";
            request.RootElement = "transaction";

            // add transaction token as an url parameter
            request.AddParameter("TransactionToken", TransactionToken, ParameterType.UrlSegment);

            // generate payload
            string amountToPayload = !string.IsNullOrWhiteSpace(amount) ? amount : Helper.DecimalToString(Amount);
            string payload = string.Format("<amount>{0}</amount>", amountToPayload);
            // add payload as a request body
            request.AddParameter("text/xml", payload, ParameterType.RequestBody);

            return Execute(request);
        }

        public bool Success()
        {
            return ProcessorResponse == null ? false : ProcessorResponse.Success;
        }

        public static Transaction Execute(RestRequest request)
        {
            var transaction = Execute<Transaction>(request);
            transaction.ProcessResponseErrors();
            return transaction;
        }

        protected void ProcessResponseErrors()
        {
            var messages = new List<Message>();
            messages.AddRange(PaymentMethod.Messages);
            messages.AddRange(ProcessorResponse.Messages);

            // Sort the messages so that more-critical/relevant ones appear first, since only the first error is added to a field
            messages.Sort(new MessageComparer());

            foreach (Message message in messages)
            {
                if (!Errors.ContainsKey(message.Context) || Errors[message.Context].Count == 0) {
                    Errors.Add(message.Context, new List<string>() {message.Description});
                }
            }
        }
    }
}
