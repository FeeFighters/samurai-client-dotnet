using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;

namespace Samurai
{
    public class SamuraiBase
    {
        public SamuraiBase()
        {
        }

        /// <summary>
        /// Executes request and returns parsed object.
        /// </summary>
        /// <typeparam name="T">Type of object to return.</typeparam>
        /// <param name="request">Request.</param>
        /// <returns>a parsed object.</returns>
        public static T Execute<T>(RestRequest request) where T : new()
        {
            // set up client
            var client = new RestClient();
            client.BaseUrl = Samurai.Site;
            client.Authenticator = new HttpBasicAuthenticator(Samurai.MerchantKey, Samurai.MerchantPassword);

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

            // prepare deserializer
            var ds = new RestSharp.Deserializers.XmlDeserializer();
            ds.Culture = System.Globalization.CultureInfo.InvariantCulture;
            ds.DateFormat = "yyyy-MM-dd HH:mm:ss UTC";

            return ds.Deserialize<T>(response);
        }
    }
}
