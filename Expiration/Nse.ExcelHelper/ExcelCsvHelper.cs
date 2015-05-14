using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace Nse.ExcelHelper
{
    public static class ExcelCsvHelper
    {
        public static void WriteToCsv<T>(List<T> dataList, string filePath)
        {
            TextWriter textWriter = File.CreateText(filePath);
            var writer = new CsvWriter(textWriter);
            writer.WriteRecords(dataList);
        }


        public static void WriteToExcel<T>(List<T> dataList, string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists)
            {
                Console.WriteLine("Deleting the file {0}", filePath);
                fileInfo.Delete();
            }

            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet excelWorksheet = package.Workbook.Worksheets.Add("Sheet1");
                excelWorksheet.Cells["A1"].LoadFromCollection(dataList, PrintHeaders: true,TableStyle: TableStyles.Light21);

                package.Save();
            }
        }
    }
}
