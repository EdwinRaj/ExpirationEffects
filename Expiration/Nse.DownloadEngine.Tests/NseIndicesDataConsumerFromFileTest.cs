using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nse.Entities.DTO;
using Nse.ExcelHelper;
using NUnit.Framework;

namespace Nse.DownloadEngine.Tests
{
    [TestFixture]
    public class NseIndicesDataConsumerFromFileTest
    {
        [Test]
        public void ReadNseIndexDailyPositionsFromFileTest()
        {
            string filePath =
                @"D:\Technical\SourceCode\ExpirationEffects\Expiration\Nse.DownloadEngine\Data\NiftyIndexData\CNX NIFTY01-01-2000-30-12-2000.csv";

            var dataConsumerFromFile = new NseIndicesDataConsumerFromFile();
            List<NseIndicesDto> nseIndicesDtos = dataConsumerFromFile.ReadNseIndexDailyPositionsFromFile(filePath);

        }

        [Test]
        public void ReadNseIndexDailyPositionsFromFolderTest()
        {
            string filePath =
                @"D:\Technical\SourceCode\ExpirationEffects\Expiration\Nse.DownloadEngine\Data\NiftyIndexData";

            var dataConsumerFromFile = new NseIndicesDataConsumerFromFile();
            List<NseIndicesDto> nseIndicesDtos = dataConsumerFromFile.ReadNseIndexDailyPositionFromFolder(filePath);

            //Filtering the unique set of data
            var uniqueDataDictionary = new Dictionary<DateTime, NseIndicesDto>();
            foreach (NseIndicesDto currentNseIndexDto in nseIndicesDtos)
            {
                if (!uniqueDataDictionary.ContainsKey(currentNseIndexDto.HistoricalDate))
                    uniqueDataDictionary.Add(currentNseIndexDto.HistoricalDate, currentNseIndexDto);
            }

            //Todo!
            string resultFile = Path.Combine(filePath,"ConsolidatedNift.csv");
            ExcelCsvHelper.WriteToCsv(uniqueDataDictionary.Values.ToList(),resultFile);
            //Analyse the data
        }
    }
}
