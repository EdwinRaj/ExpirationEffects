using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatiN.Core;
using WatiN.Core.Exceptions;

namespace Nse.DownloadEngine
{
    public class NseMetaDataDownloader
    {
        /// <summary>
        /// Returns all expiry year and date combination
        /// </summary>
        /// <param name="derivativeType"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public List<IndexOptionMetaData> GetIndexOptionMetaData(string derivativeType, string symbol)
        {
            const string url = NseUrl.NseHistoricalLink;
            using (var browser = new IE(url))
            {
                browser.WaitForComplete(60);
                SelectList instrumentTypeList = browser.SelectList("instrumentType");
                try
                {
                    foreach (Option currentOption in instrumentTypeList.Options)
                    {
                        if (currentOption.Value.ToLowerInvariant() == derivativeType.ToLowerInvariant())
                        {
                            currentOption.Select();
                            
                            break;
                        }
                    }
                    
                }
                catch (RunScriptException ex)
                {
                    //ignoring it
                    
                }

                browser.DomContainer.RunScript("fillData();");


            }

            return new List<IndexOptionMetaData>();
        }
    }
}
