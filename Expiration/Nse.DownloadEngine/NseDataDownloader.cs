using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Nse.Entities.DTO;

namespace Nse.DownloadEngine
{
    public class NseDataDownloader
    {
        private const string BaseUrl = @"http://www.nseIndia.com";

        private const string IndexOptionUrl = @"products/dynaContent/derivatives/equities/histcontract.jsp?" +
                                              @"symbolCode={0}&" + //-10007
                                              @"symbol={1}&" +
                                              @"instrumentType=OPTIDX&symbol={1}&" +
                                              @"expiryDate={2}&" +
                                              @"optionType=CE&" +
                                              @"strikePrice=&dateRange=&" +
                                              @"fromDate={3}&toDate={3}&segmentLink=9&symbolCount";

        public async Task<List<IndexOptionDto>> DownloadData(string symbolCode, string symbol, DateTime expiryDate,
            DateTime historicalDate)
        {
            try
            {

            
            string queryUrl = string.Format(IndexOptionUrl,
                symbolCode,
                symbol,
                expiryDate.ToString("dd-MM-yyyy"),
                historicalDate.ToString("dd-MMM-yyyy"));

            var httpClientHandler = new HttpClientHandler()
                                    {
                                        UseDefaultCredentials = true,
                                        PreAuthenticate = true,
                                        ClientCertificateOptions = ClientCertificateOption.Automatic,
                                        CookieContainer = new CookieContainer()
                                    };

            var httpClient = new HttpClient(httpClientHandler);


            //httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue()
            //                                                {
            //                                                    NoCache = true
            //                                                };
            httpClient.DefaultRequestHeaders.Add("User-Agent",
                @"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36");
            httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, sdch");
            httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-GB,en-US;q=0.8,en;q=0.6");
            httpClient.DefaultRequestHeaders.Add("Accept", @"*/*");
            httpClient.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");

            var baseAddress = new Uri(BaseUrl);
            httpClient.BaseAddress = baseAddress;
            httpClientHandler.CookieContainer.Add(new Cookie("pointer", "1") { Domain = baseAddress.Host });
            httpClientHandler.CookieContainer.Add(new Cookie("sym1", "INFY") { Domain = baseAddress.Host });
            httpClientHandler.CookieContainer.Add(new Cookie("pointerfo", "1") { Domain = baseAddress.Host });
            httpClientHandler.CookieContainer.Add(new Cookie("underlying1", "NIFTY") { Domain = baseAddress.Host });
            httpClientHandler.CookieContainer.Add(new Cookie("instrument1", "OPTIDX") { Domain = baseAddress.Host });
            httpClientHandler.CookieContainer.Add(new Cookie("optiontype1", "CE") { Domain = baseAddress.Host });
            httpClientHandler.CookieContainer.Add(new Cookie("expiry1", "26FEB2015") { Domain = baseAddress.Host });
            httpClientHandler.CookieContainer.Add(new Cookie("strikeprice1", "9000.00") { Domain = baseAddress.Host });
            httpClientHandler.CookieContainer.Add(new Cookie("NSE-TEST-1", "1826627594.20480.0000") { Domain = baseAddress.Host });


                Console.WriteLine("Download Request sent for the Date {0} for the expiration on {1}", historicalDate,
                    expiryDate);
            HttpResponseMessage responseMessage =
                await httpClient.GetAsync(queryUrl, HttpCompletionOption.ResponseContentRead);
            //var responseContent = await responseMessage.Content.ReadAsStringAsync();
                string responseContent;
            using (var responseStream = await responseMessage.Content.ReadAsStreamAsync())
            using (var decompressedStream = new GZipStream(responseStream, CompressionMode.Decompress))
            using (var streamReader = new StreamReader(decompressedStream))
            {
                responseContent = streamReader.ReadToEnd();
            }

            Console.WriteLine("Download Request COMPLETED for the Date {0} for the expiration on {1}", historicalDate,
                expiryDate);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(responseContent);

            var indexOptionDtos = new List<IndexOptionDto>();
            HtmlNodeCollection htmlNodeCollection = htmlDocument.DocumentNode.SelectNodes("//table");

            if (HasValidData(htmlNodeCollection))
            {
                HtmlNode tableNode = htmlNodeCollection.First();
                //ExtractNodeByParsing(tableNode, indexOptionDtos); 
                ExtractNodeByIteration(tableNode, indexOptionDtos);

            }
            return indexOptionDtos;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }


        private static void ExtractNodeByIteration(HtmlNode tableNode, List<IndexOptionDto> indexOptionDtos)
        {

            int rowlndexCounter = 0;
            foreach (HtmlNode rowNode in tableNode.SelectNodes("tr"))
            {
                rowlndexCounter++;
                if (rowlndexCounter <= 2)
                {
                    continue;
                }


                var currentIndexOptionDto = new IndexOptionDto();
                int columnlndexCounter = 0;
                foreach (HtmlNode columnNode in rowNode.SelectNodes("td|th"))
                {
                    columnlndexCounter++;
                    switch (columnlndexCounter)
                    {
                        case IndexOptionColumnIndex.Symbol:
                            currentIndexOptionDto.Symbol = columnNode.InnerText;
                            break;
                        case IndexOptionColumnIndex.HistoricalDate:
                            currentIndexOptionDto.HistoricalDate = columnNode.InnerText.ToDate();
                            break;
                        case IndexOptionColumnIndex.ExpiryDate:
                            currentIndexOptionDto.ExpiryDate = columnNode.InnerText.ToDate();
                            break;
                        case IndexOptionColumnIndex.OptionType:
                            currentIndexOptionDto.OptionType = columnNode.InnerText;
                            break;
                        case IndexOptionColumnIndex.Strike:
                            currentIndexOptionDto.Strike = columnNode.InnerHtml.ToDouble();
                            break;
                        case IndexOptionColumnIndex.Open:
                            currentIndexOptionDto.Open = columnNode.InnerHtml.ToDouble();
                            break;
                        case IndexOptionColumnIndex.High:
                            currentIndexOptionDto.High = columnNode.InnerHtml.ToDouble();
                            break;
                        case IndexOptionColumnIndex.Low:
                            currentIndexOptionDto.Low = columnNode.InnerHtml.ToDouble();
                            break;
                        case IndexOptionColumnIndex.Close:
                            currentIndexOptionDto.Close = columnNode.InnerHtml.ToDouble();
                            break;
                        case IndexOptionColumnIndex.Ltp:
                            currentIndexOptionDto.Ltp = columnNode.InnerHtml.ToDouble();
                            break;
                        case IndexOptionColumnIndex.SettlePrice:
                            currentIndexOptionDto.SettlePrice = columnNode.InnerHtml.ToDouble();
                            break;
                        case IndexOptionColumnIndex.TotalContracts:
                            currentIndexOptionDto.TotalContracts = columnNode.InnerHtml.ToDouble();
                            break;
                        case IndexOptionColumnIndex.TurnoverInLacs:
                            currentIndexOptionDto.TurnoverInLacs = columnNode.InnerHtml.ToDouble();
                            break;
                        case IndexOptionColumnIndex.OpenInterest:
                            currentIndexOptionDto.OpenInterest = columnNode.InnerHtml.ToDouble();
                            break;
                        case IndexOptionColumnIndex.ChangeInOpenInterest:
                            currentIndexOptionDto.ChangeInOpenInterest = columnNode.InnerHtml.ToDouble();
                            break;
                        case IndexOptionColumnIndex.UnderlyingValue:
                            currentIndexOptionDto.UnderlyingValue = columnNode.InnerHtml.ToDouble();
                            break;


                    }

                }
                indexOptionDtos.Add(currentIndexOptionDto);
            }
        }

        private bool HasValidData(IEnumerable<HtmlNode> htmlNodeCollection)
        {
            if (htmlNodeCollection != null)
            {
                HtmlNode tableNode = htmlNodeCollection.First();
                HtmlNode recordNode = tableNode.SelectSingleNode("//tr[3]//td");
                if (recordNode != null && recordNode.InnerText.Trim().ToUpperInvariant() != "NO RECORDS")

                    return true;

            }
            return false;
        }

        private static string ExtractDataWithDataType(HtmlNode tableNode, int rowlndex, int columnIndex)
        {
            return tableNode.SelectSingleNode(string.Format("//tr[{0}]//td[{1}]", rowlndex, columnIndex)).InnerText;
        }

    }
}
