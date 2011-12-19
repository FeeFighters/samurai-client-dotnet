using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;

namespace Samurai
{
    /// <summary>
    /// Represents message.
    /// </summary>
    public class Message : SamuraiBase
    {
        /// <summary>
        /// Gets or sets what type of message it is. The value will always be <c>error</c> for 
        /// errors and declines or <c>info</c> for informative messages.
        /// </summary>
        public string Subclass { get; set; }

        /// <summary>
        /// Gets or sets the place where the message applies. Each context is prefixed 
        /// with <c>input</c>, <c>processor</c> or <c>system</c> for user input messages, 
        /// processor messages and UberGateway system messages respectively.
        /// </summary>
        public string Context { get; set; }

        /// <summary>
        /// Gets or sets some specific information about the message.
        /// </summary>
        public string Key { get; set; }


        public static readonly Dictionary<string, string> DefaultResponseMappings = new Dictionary<string, string>()
        {
            // Transaction Responses
            { "info processor.transaction success"      , "The transaction was successful." },
            { "error processor.transaction declined"    , "The card was declined." },
            { "error processor.issuer call"             , "Call the card issuer for further instructions." },
            { "error processor.issuer unavailable"      , "The authorization did not respond within the alloted time." },
            // { "error input.card_number invalid"         , "The card number was invalid." },
            // { "error input.expiry_month invalid"        , "The expiration date month was invalid, or prior to today." },
            // { "error input.expiry_year invalid"         , "The expiration date year was invalid, or prior to today." },
            { "error processor.pin invalid"             , "The PIN number is incorrect." },
            { "error input.amount invalid"              , "The transaction amount was invalid." },
            { "error processor.transaction declined_insufficient_funds" , "The transaction was declined due to insufficient funds." },
            { "error processor.network_gateway merchant_invalid"        , "The Merchant Number is incorrect." },
            { "error input.merchant_login invalid"      , "The merchant ID is not valid or active." },
            { "error input.store_number invalid"        , "Invalid Store Number." },
            { "error processor.bank_info invalid"       , "Invalid banking information." },
            { "error processor.transaction not_allowed" , "This transaction type is not allowed." },
            { "error processor.transaction type_invalid"    , "Requested transaction type is not allowed for this card/merchant." },
            { "error processor.transaction method_invalid"  , "The requested transaction could not be performed for this merchant." },
            { "error input.amount exceeds_limit"            , "The maximum transaction amount was exceeded." },
            { "error input.cvv invalid"                     , "The CVV code was not correct." },
            { "error processor.network_gateway communication_error"     , "There was a fatal communication error." },
            { "error processor.network_gateway unresponsive"            , "The processing network is temporarily unavailable." },
            // { "error processor.network_gateway merchant_invalid"        , "The merchant number is not on file." },
        
            // AVS Responses
            { "info processor.avs_result_code 0"  , "No response." },
            { "info processor.avs_result_code Y"  , "The address and 5-digit ZIP match." },
            { "info processor.avs_result_code Z"  , "The 5-digit ZIP matches, the address does not." },
            { "info processor.avs_result_code X"  , "The address and 9-digit ZIP match." },
            { "info processor.avs_result_code A"  , "The address matches, the ZIP does not." },
            { "info processor.avs_result_code E"  , "There was an AVS error, or the data was illegible." },
            { "info processor.avs_result_code R"  , "The AVS request timed out." },
            { "info processor.avs_result_code S"  , "The issuer does not support AVS." },
            { "info processor.avs_result_code F"  , "The street addresses and postal codes match." },
            { "info processor.avs_result_code N"  , "The address and ZIP do not match." },
        
            // CVV Responses
            { "error input.cvv declined" , "The CVV code was not correct." },
        
            // Input validations
            { "error input.card_number is_blank"        , "The card number was blank." },
            { "error input.card_number not_numeric"     , "The card number was invalid." },
            { "error input.card_number too_short"       , "The card number was too short." },
            { "error input.card_number too_long"        , "The card number was too long." },
            { "error input.card_number failed_checksum" , "The card number was invalid." },
            { "error input.card_number is_invalid"      , "The card number was invalid." },
            { "error input.cvv is_blank"                , "The CVV was blank." },
            { "error input.cvv not_numeric"             , "The CVV was invalid." },
            { "error input.cvv too_short"               , "The CVV was too short." },
            { "error input.cvv too_long"                , "The CVV was too long." },
            { "error input.expiry_month is_blank"       , "The expiration month was blank." },
            { "error input.expiry_month not_numeric"    , "The expiration month was invalid." },
            { "error input.expiry_month is_invalid"     , "The expiration month was invalid." },
            { "error input.expiry_year is_blank"        , "The expiration year was blank." },
            { "error input.expiry_year not_numeric"     , "The expiration year was invalid." },
            { "error input.expiry_year is_invalid"      , "The expiration year was invalid." },
        };

        /// <summary>
        /// Gets or sets message description
        /// </summary>
        private string _description = String.Empty;
        public string Description {
            get {
                if (string.IsNullOrWhiteSpace(_description)) {
                    var _key = Subclass + " " + Context + " " + Key;
                    return DefaultResponseMappings.ContainsKey(_key) ? DefaultResponseMappings[_key] : "";
                }
                return _description;
            }
            set { _description = value; }
        }
    }

    public class MessageComparer:IComparer<Message>
    {
        public int Compare(Message x, Message y)
        {
            List<string> keyOrderList = new List<string> { "is_blank", "not_numeric", "too_short", "too_long", "failed_checksum" };
            return keyOrderList.IndexOf(x.Key) - keyOrderList.IndexOf(y.Key);
        }
    }

}
