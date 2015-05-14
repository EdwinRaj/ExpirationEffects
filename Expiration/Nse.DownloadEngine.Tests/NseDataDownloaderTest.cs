using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nse.Entities.DTO;
using NUnit.Framework;

namespace Nse.DownloadEngine.Tests
{
    [TestFixture]
    public class NseDataDownloaderTest
    {
        private readonly Dictionary<string, string> _optionsSymbolCodeMap = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);


        [SetUp]
        public void Setup()
        {
            _optionsSymbolCodeMap.Add(NseIndexOptionSymbols.CnxNifty, NseIndexOptionSymbolsCodes.CnxNiftyCode);
            _optionsSymbolCodeMap.Add(NseIndexOptionSymbols.NftyMidCap50, NseIndexOptionSymbolsCodes.NftyMidCap50Code);
            _optionsSymbolCodeMap.Add(NseIndexOptionSymbols.BankNifty, NseIndexOptionSymbolsCodes.BankNiftyCode);
            _optionsSymbolCodeMap.Add(NseIndexOptionSymbols.CnxInfra, NseIndexOptionSymbolsCodes.CnxInfraCode);
            _optionsSymbolCodeMap.Add(NseIndexOptionSymbols.CnxIt, NseIndexOptionSymbolsCodes.CnxItCode);
            _optionsSymbolCodeMap.Add(NseIndexOptionSymbols.CnxPse, NseIndexOptionSymbolsCodes.CnxPseCode);
            _optionsSymbolCodeMap.Add(NseIndexOptionSymbols.Sp500, NseIndexOptionSymbolsCodes.Sp500Code);
            _optionsSymbolCodeMap.Add(NseIndexOptionSymbols.Djia, NseIndexOptionSymbolsCodes.DjiaCode);
            _optionsSymbolCodeMap.Add(NseIndexOptionSymbols.Ftse100, NseIndexOptionSymbolsCodes.Ftse100Code);

        }

        [Test]
        public void DownloadData()
        {
            var dataDownloader = new NseDataDownloader();
             const string symbol = "CNXINFRA";
            DateTime expiryDate = new DateTime(2014,1,30);
            Task<List<IndexOptionDto>> downloadDataTask = dataDownloader.DownloadData(_optionsSymbolCodeMap[symbol],symbol,expiryDate,expiryDate);
            downloadDataTask.Wait();
            List<IndexOptionDto> indexOptionDtos = downloadDataTask.Result;
            ExcelHelper.ExcelCsvHelper.WriteToExcel(indexOptionDtos, @"D:\Technical\SourceCode\ExpirationEffects\Expiration\Nse.DownloadEngine\Data\Options\HistoricalPrices\test.csv");
            Assert.IsTrue(downloadDataTask.IsCompleted);
        }
    }
}
