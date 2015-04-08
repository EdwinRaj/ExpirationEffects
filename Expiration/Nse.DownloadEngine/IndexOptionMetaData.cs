using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nse.DownloadEngine
{
    public class IndexOptionMetaData
    {
        public string DerivativeType { get; set; }
        public string Symbol { get; set; }
        public string ExpiryYear { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
