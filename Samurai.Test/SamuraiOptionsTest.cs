using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Samurai.Test
{
    [TestClass]
    public class SamuraiOptionsTest
    {
        [TestMethod]
        public void Merge_Test()
        {
            var mainOptions = new SamuraiOptions()
            {
                Site = "http://www.example.com/v1/",
                MerchantKey = "111"
            };

            var anotherOptions = new SamuraiOptions()
            {
                MerchantPassword = "123qwerty",
                MerchantKey = "222"
            };

            // merge main with another
            var mainWithAnother = mainOptions.ReverseMerge(anotherOptions);
            // check it
            Assert.AreEqual(mainOptions.Site,               mainWithAnother.Site);
            Assert.AreEqual(mainOptions.MerchantKey,        mainWithAnother.MerchantKey);
            Assert.AreEqual(anotherOptions.MerchantPassword,mainWithAnother.MerchantPassword);
            Assert.IsTrue(string.IsNullOrEmpty(mainWithAnother.ProcessorToken));

            // merge another with main
            var anotherWithMain = anotherOptions.ReverseMerge(mainOptions);
            // check it
            Assert.AreEqual(mainOptions.Site,               anotherWithMain.Site);
            Assert.AreEqual(anotherOptions.MerchantKey,     anotherWithMain.MerchantKey);
            Assert.AreEqual(anotherOptions.MerchantPassword,anotherWithMain.MerchantPassword);
            Assert.IsTrue(string.IsNullOrEmpty(anotherWithMain.ProcessorToken));
        }
    }
}
