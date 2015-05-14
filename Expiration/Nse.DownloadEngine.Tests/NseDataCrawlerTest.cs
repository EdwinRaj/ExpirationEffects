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
    public class NseDataCrawlerTest
    {
        const string NiftySymbolCode = "-10007";
        const string NiftySymbol = "NIFTY";
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
        public void DownloadNseOptionDataForSingleDate()
        {
            var crawler = new NseDataCrawler();


            List<IndexOptionDto> indexOptionDtos = crawler.GetData(new List<DateTime>() { new DateTime(2015, 4, 30) }, NiftySymbolCode, NiftySymbol, 10);
            Assert.IsTrue(indexOptionDtos.Count > 0);
        }

        [Test]
        public void DownloadNiftyOptionHistoricPrices()
        {
            string sourceFile =
                @"D:\Technical\SourceCode\ExpirationEffects\Expiration\Nse.DownloadEngine\Data\IndexOptionsExpiryDates.xlsx";

            var expirationDatesReader = new NseMetaDataConsumerFromFile();
            List<IndexOptionMetaDataDto> indexOptionMetaDataDtos = expirationDatesReader.ProcessData(sourceFile);
            var crawler = new NseDataCrawler();

            IEnumerable<string> indexOptionSymbols =
                indexOptionMetaDataDtos
                    .Where(x => x.Symbol.ToLowerInvariant() == "nifty")
                    .Select(x => x.Symbol.ToLowerInvariant())
                    .Distinct()
                    .ToList();

            foreach (var currentOptionSymbol in indexOptionSymbols)
            {
                string symbol = currentOptionSymbol;
                IEnumerable<string> expirationYearsList =
                    indexOptionMetaDataDtos.Where(x => x.Symbol.ToLowerInvariant() == symbol.ToLowerInvariant())
                        .Select(x => x.ExpiryYear)
                        .Distinct()
                        .ToList();

                foreach (string currentExpirationYear in expirationYearsList)
                {
                    List<DateTime> expirationDate = indexOptionMetaDataDtos.Where(x => x.Symbol.ToLowerInvariant() == symbol.ToLowerInvariant()
                                                                                && x.ExpiryYear == currentExpirationYear
                                                                                && x.ExpiryDate < DateTime.Today)
                                                                                .Select(x => x.ExpiryDate).ToList();

                    const int historyDataWindowInDays = 20;
                    List<IndexOptionDto> indexOptionDtos = crawler.GetData(expirationDate, NiftySymbolCode, NiftySymbol, historyDataWindowInDays);
                    const string outputFileLocationTemplate = @"D:\Technical\SourceCode\ExpirationEffects\Expiration\Nse.DownloadEngine\Data\Options\HistoricalPrices\{0}\{0}OptionPrices{1}.xlsx";
                    string outPutFileLocation = string.Format(outputFileLocationTemplate, symbol, currentExpirationYear);
                    ExcelHelper.ExcelCsvHelper.WriteToExcel(indexOptionDtos, outPutFileLocation);

                }
            }

        }

        [Test]
        public void DownloadAllOptionHistoricPrices()
        {
            const string sourceFile = @"D:\Technical\SourceCode\ExpirationEffects\Expiration\Nse.DownloadEngine\Data\IndexOptionsExpiryDates.xlsx";

            var expirationDatesReader = new NseMetaDataConsumerFromFile();
            List<IndexOptionMetaDataDto> indexOptionMetaDataDtos = expirationDatesReader.ProcessData(sourceFile);
            var crawler = new NseDataCrawler();

            //Get all symbols
            IEnumerable<string> indexOptionSymbols =
                indexOptionMetaDataDtos
                    .Select(x => x.Symbol.ToLowerInvariant())
                    .Distinct()
                    .ToList();

            foreach (var currentOptionSymbol in indexOptionSymbols)
            {
                string symbol = currentOptionSymbol;
                
                //For each symbol, get all expiration years
                IEnumerable<string> expirationYearsList =
                    indexOptionMetaDataDtos.Where(x => x.Symbol.ToLowerInvariant() == symbol.ToLowerInvariant())
                        .Select(x => x.ExpiryYear)
                        .Distinct()
                        .ToList();
                var indexOptionDtos = new List<IndexOptionDto>();

                foreach (string currentExpirationYear in expirationYearsList)
                {
                    List<DateTime> expirationDatesList = indexOptionMetaDataDtos.Where(x => x.Symbol.ToLowerInvariant() == symbol.ToLowerInvariant()
                                                                                && x.ExpiryYear == currentExpirationYear
                                                                                && x.ExpiryDate < DateTime.Today)
                                                                                .Select(x => x.ExpiryDate).ToList();
                    
                    //expirationDatesList = expirationDatesList.Take(1).ToList();
                    const int historyDataWindowInDays = 20;
                    string symbolCode = _optionsSymbolCodeMap[currentOptionSymbol];
                    string symbolName = currentOptionSymbol;

                    List<IndexOptionDto> currentHistoricalData = crawler.GetData(expirationDatesList, symbolCode, symbolName, historyDataWindowInDays);
                    indexOptionDtos.AddRange(currentHistoricalData);
                    
                }

                const string outputFileLocationTemplate = @"D:\Technical\SourceCode\ExpirationEffects\Expiration\Nse.DownloadEngine\Data\Options\HistoricalPrices\{0}OptionPrices.xlsx";
                string outPutFileLocation = string.Format(outputFileLocationTemplate, currentOptionSymbol);
                ExcelHelper.ExcelCsvHelper.WriteToExcel(indexOptionDtos, outPutFileLocation);
            }

        }
    }
}
