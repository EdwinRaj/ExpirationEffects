using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nse.Entities.DTO
{
    public class IndexOptionDto
    {
        public string Symbol { get; set; }
        public DateTime HistoricalDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string OptionType { get; set; }
        public double Strike { get; set; }
        public double Price { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double Ltp { get; set; }
        public double SettlePrice { get; set; }
        public double TotalContracts { get; set; }
        public double TurnoverInLacs { get; set; }
        public double OpenInterest { get; set; }
        public double ChangeInOpenInterest { get; set; }
        public double UnderlyingValue { get; set; }

    }

    public class IndexOptionColumnIndex
    {
        public const int Symbol = 1;
        public const int HistoricalDate = 2;
        public const int ExpiryDate = 3;
        public const int OptionType = 4;
        public const int Strike = 5;
        public const int Open = 6;
        public const int High = 7;
        public const int Low = 8;
        public const int Close = 9;
        public const int Ltp = 10;
        public const int SettlePrice = 11;
        public const int TotalContracts = 12;
        public const int TurnoverInLacs = 13;
        public const int OpenInterest = 14;
        public const int ChangeInOpenInterest = 15;
        public const int UnderlyingValue = 16;
    }


}
