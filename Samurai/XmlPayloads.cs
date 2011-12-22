using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp.Serializers;

namespace Samurai
{
    [SerializeAs(Name = "transaction")]
    public class TransactionPayload
    {
        public TransactionPayload()
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
        [SerializeAs(Name = "description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets dynamic descriptor name for the transaction.
        /// </summary>
        [SerializeAs(Name = "descriptor_name")]
        public string DescriptorName { get; set; }

        /// <summary>
        /// Gets or sets dynamic descriptor phone for the transaction.
        /// </summary>
        [SerializeAs(Name = "descriptor_phone")]
        public string DescriptorPhone { get; set; }

        /// <summary>
        /// Gets or sets custom data, this data does not get passed to the processor, 
        /// it is stored within api.samurai.feefighters.com only.
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

    [SerializeAs(Name = "payment_method")]
    public class PaymentMethodPayload
    {
        public PaymentMethodPayload() {}

        public PaymentMethodPayload(PaymentMethod pm)
        {
            FirstName = pm.FirstName;
            LastName = pm.LastName;
            Address1 = pm.Address1;
            Address2 = pm.Address2;
            City = pm.City;
            State = pm.State;
            Zip = pm.Zip;
            CardNumber = pm.CardNumber;
            Cvv = pm.Cvv;
            ExpiryMonth = pm.ExpiryMonth;
            ExpiryYear = pm.ExpiryYear;
            Custom = pm.Custom;
            Sandbox = pm.Sandbox;
        }

        [SerializeAs(Name = "first_name")]
        public string FirstName {get;set;}

        [SerializeAs(Name = "last_name")]
        public string LastName {get;set;}

        [SerializeAs(Name = "address_1")]
        public string Address1 {get;set;}

        [SerializeAs(Name = "address_2")]
        public string Address2 {get;set;}

        [SerializeAs(Name = "city")]
        public string City {get;set;}

        [SerializeAs(Name = "state")]
        public string State {get;set;}

        [SerializeAs(Name = "zip")]
        public string Zip {get;set;}

        [SerializeAs(Name = "card_number")]
        public string CardNumber {get;set;}

        [SerializeAs(Name = "cvv")]
        public string Cvv {get;set;}

        [SerializeAs(Name = "expiry_month")]
        public int ExpiryMonth {get;set;}

        [SerializeAs(Name = "expiry_year")]
        public int ExpiryYear {get;set;}

        [SerializeAs(Name = "custom")]
        public string Custom {get;set;}

        [SerializeAs(Name = "sandbox")]
        public bool Sandbox {get;set;}

        public PaymentMethodPayload Merge(PaymentMethodPayload payload)
        {
            if (payload == null) return this;

            if (payload.FirstName != null)  { FirstName = payload.FirstName; }
            if (payload.LastName != null)   { LastName = payload.LastName; }
            if (payload.Address1 != null)   { Address1 = payload.Address1; }
            if (payload.Address2 != null)   { Address2 = payload.Address2; }
            if (payload.City != null)       { City = payload.City; }
            if (payload.State != null)      { State = payload.State; }
            if (payload.Zip != null)        { Zip = payload.Zip; }
            if (payload.CardNumber != null) { CardNumber = payload.CardNumber; }
            if (payload.Cvv != null)        { Cvv = payload.Cvv; }
            ExpiryMonth = payload.ExpiryMonth==0 ? ExpiryMonth : payload.ExpiryMonth;
            ExpiryYear = payload.ExpiryYear==0 ? ExpiryYear : payload.ExpiryYear;
            if (payload.Custom != null)     { Custom = payload.Custom; }
            Sandbox = Sandbox || payload.Sandbox;
            return this;
        }
    }
}
