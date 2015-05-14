using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nse.Entities.DTO
{
    public class NseIndicesDto
    {
        public DateTime HistoricalDate { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double SharesTraded { get; set; }
        public double TurnoverInCrores { get; set; }

    }
}
