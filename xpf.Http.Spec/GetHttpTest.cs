using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xpf.Http.Spec
{
    [TestClass]
    public class GetHttpTest
    {
        [TestMethod]
        public void DetermineBug()
        {
            var http = new Http();

            var result = http.Navigate("http://siph0n.net/exploits.php?id=965").UserAgent.IE11
                    .GetAsync<string>().Result;

        }
    }
}
