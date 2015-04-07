using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Nse.Spike
{
    [TestFixture]
    public class DownloadUsingHttpWebRequest
    {
        [Test]
        public void DownloadData()
        {
            string url =
                @"http://www.nseindia.com/products/dynaContent/derivatives/equities/histcontract.jsp?symbolCode=-10007&symbol=NIFTY&instrumentType=OPTIDX&symbol=NIFTY&expiryDate=26-03-2015&optionType=CE&strikePrice=&dateRange=&fromDate=26-Mar-2015&toDate=26-Mar-2015&segmentLink=9&symbolCount";

            var httpClient = new HttpClient();
            Task<HttpResponseMessage> getRequestTask = httpClient.GetAsync(new Uri(url));
            getRequestTask.Start();

            while (!getRequestTask.IsCompleted)
            {
                
            }

            HttpResponseMessage httpResponseMessage = getRequestTask.Result;
            
        }
    }
}
