using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nse.Entities.DTO;
using Nse.Entities.UnitOfWork;
using NUnit.Framework;

namespace Nse.DownloadEngine.Tests
{
    [TestFixture]
    public class DeploymentFunctions
    {
        [Test]
        public void ProcessIndexOptions()
        {
            const string indexOptionsPath = @"D:\Technical\SourceCode\ExpirationEffects\Expiration\Nse.DownloadEngine\Data\IndexOptionsExpiryDates.xlsx";

            var consumer = new NseMetaDataConsumerFromFile();
            List<IndexOptionMetaDataDto> indexOptionMetaDataDtos = consumer.ProcessData(indexOptionsPath);

            const string dbConnectionString = "NseContext";
            NseMetaDataUnitOfWork unitOfWork = new NseMetaDataUnitOfWork(dbConnectionString);
            unitOfWork.SaveNseMetaData(indexOptionMetaDataDtos);
        }

        [Test]
        public void ProcessIndexFutures()
        {
            const string indexOptionsPath = @"D:\Technical\SourceCode\ExpirationEffects\Expiration\Nse.DownloadEngine\Data\IndexFutureExpiryDates.xlsx";

            var consumer = new NseMetaDataConsumerFromFile();
            List<IndexOptionMetaDataDto> indexOptionMetaDataDtos = consumer.ProcessData(indexOptionsPath);

            const string dbConnectionString = "NseContext";
            NseMetaDataUnitOfWork unitOfWork = new NseMetaDataUnitOfWork(dbConnectionString);
            unitOfWork.SaveNseMetaData(indexOptionMetaDataDtos);
        }
    }
}
