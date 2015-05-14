using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nse.DownloadEngine
{
    public static  class StringUtils
    {
        public static DateTime ToDate(this string data)
        {
            DateTime currentDateTime;
            bool isSuccess = DateTime.TryParse(data,out currentDateTime);
            if (!isSuccess)
            {
                throw new Exception("Invalid DateTime Conversion");
            }
            return currentDateTime;
        }

        public static double ToDouble(this string data)
        {
            double currentData = 0;
            bool isSuccess = double.TryParse(data, out currentData);
            
            return currentData;
        }
    }
}
