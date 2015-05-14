using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nse.DownloadEngine
{
    public class NseUrl
    {
        public const string NseHistoricalLink =
            "http://www.nseindia.com/products/content/derivatives/equities/historical_fo.htm";

    }

    public class NseInstrumentTypes
    {
        public const string IndexFutures = "FUTIDX";
        public const string StockFutures = "FUTSTK";
        public const string IndexOptions = "OPTIDX";
        public const string StockOptions = "OPTSTK";
        public const string VolatilityFutures = "FUTIVX";
    }

    public class NseIndexOptionSymbols
    {
        public const string CnxNifty = "NIFTY";
        public const string NftyMidCap50 = "NFTYMCAP50";
        public const string BankNifty = "BANKNIFTY";
        public const string CnxInfra = "CNXINFRA";
        public const string CnxIt = "CNXIT";
        public const string CnxPse = "CNXPSE";
        public const string Sp500 = "S&P500";
        public const string Djia = "DJIA";
        public const string Ftse100 = "FTSE100";
    }

    public class NseIndexOptionSymbolsCodes
    {
        public const string CnxNiftyCode = "-10007";
        public const string NftyMidCap50Code = "-10006";
        public const string BankNiftyCode = "-9999";
        public const string CnxInfraCode = "-10000";
        public const string CnxItCode = "10001";
        public const string CnxPseCode = "-10002";
        public const string Sp500Code = "-10008";
        public const string DjiaCode = "-10003";
        public const string Ftse100Code = "-10004";
    }
}
