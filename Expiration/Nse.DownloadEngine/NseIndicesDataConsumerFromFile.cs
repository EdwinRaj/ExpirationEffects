using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using Nse.Entities.DTO;

namespace Nse.DownloadEngine
{
    public class NseIndicesIndex
    {
        public const int HistoricalDate = 0;
        public const int Open = 1;
        public const int High = 2;
        public const int Low = 3;
        public const int Close = 4;
        public const int SharesTraded = 5;
        public const int TurnOverInCrores = 6;
    }
    /// <summary>
    /// Responsibility of the class is to read the daily Index data from CSV
    /// </summary>
    public class NseIndicesDataConsumerFromFile
    {
        /// <summary>
        /// Reads the content of the file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public List<NseIndicesDto> ReadNseIndexDailyPositionsFromFile(string filePath)
        {
            TextReader fileReader = File.OpenText(filePath);
            var reader = new CsvReader(fileReader);
            var nseIndicesDailyPositionDtoList = new List<NseIndicesDto>();

            while (reader.Read())
            {
                var indicesDto = new NseIndicesDto();
                indicesDto.HistoricalDate = reader.GetField<DateTime>(NseIndicesIndex.HistoricalDate);
                indicesDto.Open = reader.GetField<double>(NseIndicesIndex.Open);
                indicesDto.High = reader.GetField<double>(NseIndicesIndex.High);
                indicesDto.Low = reader.GetField<double>(NseIndicesIndex.Low);
                indicesDto.Close = reader.GetField<double>(NseIndicesIndex.Close);
                indicesDto.SharesTraded = reader.GetField<double>(NseIndicesIndex.SharesTraded);
                indicesDto.TurnoverInCrores = reader.GetField<double>(NseIndicesIndex.TurnOverInCrores);
                nseIndicesDailyPositionDtoList.Add(indicesDto);
            }
            return nseIndicesDailyPositionDtoList;
        }

        public List<NseIndicesDto> ReadNseIndexDailyPositionFromFolder(string folderPath)
        {
            var directoryInfo = new DirectoryInfo(folderPath);
            FileInfo[] nseFileInfoList = directoryInfo.GetFiles("*.csv",SearchOption.TopDirectoryOnly);
            var consolidatedDailyPosition = new List<NseIndicesDto>();
            foreach (FileInfo currentFileInfo in nseFileInfoList)
            {
                string fullFilePath = currentFileInfo.FullName;
                List<NseIndicesDto> currentDailyPosition = ReadNseIndexDailyPositionsFromFile(fullFilePath);
                consolidatedDailyPosition.AddRange(currentDailyPosition);
            }
            return consolidatedDailyPosition;
        }
    }
}
