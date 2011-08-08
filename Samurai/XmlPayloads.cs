using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp.Serializers;

namespace Samurai
{
    [SerializeAs(Name = "transaction")]
    class PurchaseXmlPayload
    {
        public PurchaseXmlPayload()
        {
            CurrencyCode = "USD";
        }

        /// <summary>
        /// Gets or sets type for transaction.
        /// </summary>
        [SerializeAs(Name = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets amount to authorize.
        /// </summary>
        [SerializeAs(Name = "amount")]
        public string Amount { get; set; }

        /// <summary>
        /// Gets or sets currency code, default is USD.
        /// </summary>
        [SerializeAs(Name = "currency_code")]
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets token identifying the payment method to authorize.
        /// </summary>
        [SerializeAs(Name = "payment_method_token")]
        public string PaymentMethodToken { get; set; }

        /// <summary>
        /// Gets or sets descriptor for the transaction.
        /// </summary>
        [SerializeAs(Name = "descriptor")]
        public string Descriptor { get; set; }

        /// <summary>
        /// Gets or sets custom data, this data does not get passed to the processor, 
        /// it is stored within samurai.feefighters.com only.
        /// </summary>
        [SerializeAs(Name = "custom")]
        public string Custom { get; set; }

        /// <summary>
        /// Gets or sets an identifier for the customer, 
        /// this will appear in the processor if supported.
        /// </summary>
        [SerializeAs(Name = "customer_reference")]
        public string CustomerReference { get; set; }

        /// <summary>
        /// Gets or sets an identifier for the purchase, 
        /// this will appear in the processor if supported.
        /// </summary>
        [SerializeAs(Name = "billing_reference")]
        public string BillingReference { get; set; }
    }
}
