using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Nse.DownloadEngine.Tests
{
    [TestFixture]
    public class NseMetaDataDownloaderTests
    {
        [Test]
        [STAThread]
        public void TestMetaDataDownload()
        {
            var dataDownloader = new NseMetaDataDownloader();
            List<IndexOptionMetaData> indexOptionMetaDatas = dataDownloader.GetIndexOptionMetaData(NseInstrumentTypes.IndexOptions, NseIndexOptionSymbols.CnxNifty);
        }
    }
}
