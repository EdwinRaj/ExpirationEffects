using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Nse.Entities.DTO;

namespace Nse.DownloadEngine
{
    public class NseDataCrawler
    {
        private const int MaxConnectionLimit = 5;
        private readonly NseDataDownloader _nseDataDownloader = new NseDataDownloader();

        public NseDataCrawler()
        {
            ServicePointManager.MaxServicePoints = MaxConnectionLimit;
        }


        public List<IndexOptionDto> GetData(List<DateTime> expiryDates, string symbolCode, string symbol, int historicDayWindow)
        {
            var runningTaskList = new List<Task<List<IndexOptionDto>>>();
            var indexOptionList = new List<IndexOptionDto>();
            foreach (DateTime currentExpiryDate in expiryDates)
            {
                int runningIndex = 0;
                DateTime historicalDate = currentExpiryDate;
                while (runningIndex < historicDayWindow)
                {
                    runningTaskList.Add(_nseDataDownloader.DownloadData(symbolCode, symbol, currentExpiryDate,historicalDate));
                    historicalDate = GetNextWorkingDay(historicalDate);
                    runningIndex++;
                    Thread.Sleep(200);
                }
                
            }
            while (runningTaskList.Any())
            {
                var completedTaskIndex = Task.WaitAny(runningTaskList.ToArray());
                Task<List<IndexOptionDto>> completedTask = runningTaskList[completedTaskIndex];
                runningTaskList.Remove(completedTask);

                if (completedTask.IsFaulted)
                {
                    Console.WriteLine(completedTask.Exception.ToString());
                    continue;
                }

                indexOptionList.AddRange(completedTask.Result);

            }
            return indexOptionList;
            
        }

        private DateTime GetNextWorkingDay(DateTime currentWorkingDay)
        {
            DateTime nextWorkingDay = currentWorkingDay.AddDays(-1);
            while (nextWorkingDay.DayOfWeek == DayOfWeek.Saturday || nextWorkingDay.DayOfWeek == DayOfWeek.Sunday)
            {
                nextWorkingDay = nextWorkingDay.AddDays(-1);
            }

            return nextWorkingDay;
        }
    }
}
