using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;

namespace Samurai
{
    public class Transaction : SamuraiBase
    {
        public string ReferenceId { get; set; }
        public string TransactionToken { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Descriptor { get; set; }
        public string Custom { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string ProcessorToken { get; set; }
        
        // ...

        public PaymentMethod PaymentMethod { get; set; }

        public static Transaction Fetch(string purchaseReferenceId)
        {
            var request = new RestRequest();
            request.Resource = "transactions/{ReferenceID}.xml";
            request.RootElement = "transaction";

            request.AddParameter("ReferenceID", purchaseReferenceId, ParameterType.UrlSegment);

            return Execute<Transaction>(request);
        }
    }
}
