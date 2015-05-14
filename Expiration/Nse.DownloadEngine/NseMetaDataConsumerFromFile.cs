using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nse.Entities.DTO;
using Nse.Entities.UnitOfWork;
using OfficeOpenXml;

namespace Nse.DownloadEngine
{
    public class NseMetaDataConsumerFromFile
    {
        private int InstrumentTypeIndex = 1;
        private int SymbolIndex = 2;
        private int ExpiryYearIndex = 3;
        private int ExpiryDateIndex = 4;



        public List<IndexOptionMetaDataDto> ProcessData(string filePath)
        {
            var info = new FileInfo(filePath);
            if (!info.Exists)
            {
                Console.WriteLine("File {0} doesn't exists",filePath);
            }

            var metaDataDtoList = new List<IndexOptionMetaDataDto>();

            using (var excelPackage = new ExcelPackage(info))
            {
                ExcelWorksheet dataWorksheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
                if (dataWorksheet != null)
                {
                    int numberOfRows = dataWorksheet.Dimension.End.Row;
                    //First row is a header, so we start from second row
                    for (int rowIndex = 2; rowIndex < numberOfRows; rowIndex++)
                    {
                        var dataDto = new IndexOptionMetaDataDto();
                        dataDto.DerivativeType = dataWorksheet.Cells[rowIndex, InstrumentTypeIndex].Text;
                        dataDto.Symbol = dataWorksheet.Cells[rowIndex, SymbolIndex].Text;
                        dataDto.ExpiryYear = dataWorksheet.Cells[rowIndex, ExpiryYearIndex].Text;
                        dataDto.ExpiryDate = dataWorksheet.Cells[rowIndex, ExpiryDateIndex].Text.ToDate();
                        metaDataDtoList.Add(dataDto);
                    }
                }
                else
                {
                    Console.WriteLine("No data sheet available to process");
                }
            }
            return metaDataDtoList;
        }
    }
}
