﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;

namespace Samurai
{
    public class SamuraiBase
    {
        public static T Execute<T>(RestRequest request) where T : new()
        {
            // set up client
            var client = new RestClient();
            client.BaseUrl = Samurai.Site;
            client.Authenticator = new HttpBasicAuthenticator(Samurai.MerchantKey, Samurai.MerchantPassword);

            // get response
            var response = client.Execute(request);

            // prepare deserializer
            var ds = new RestSharp.Deserializers.XmlDeserializer();
            ds.Culture = System.Globalization.CultureInfo.InvariantCulture;
            ds.DateFormat = "yyyy-MM-dd HH:mm:ss UTC";

            return ds.Deserialize<T>(response);
        }
    }
}