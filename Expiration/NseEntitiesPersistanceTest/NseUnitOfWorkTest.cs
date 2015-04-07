using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nse.Entities;
using Nse.Entities.UnitOfWork;
using NUnit.Framework;

namespace Nse.Entities.Test
{
    [TestFixture]
    public class NseUnitOfWorkTest
    {
        private const string DbConnectionString = "NseTestContext";

        [Test]
        public void AddDerivativeSymbolsExpirationTest()
        {
            var unitOfWork = new NseMetaDataUnitOfWork(DbConnectionString);

            var oneDayExpiry = new ExpirationDetail()
                                                {
                                                    ExpirationDate = DateTime.Now.AddDays(-1),
                                                    ExpirationYear = "2015"
                                                };

            var twoDayExpiry = new ExpirationDetail()
                                                {
                                                    ExpirationDate = DateTime.Now.AddDays(-2),
                                                    ExpirationYear = "2015"
                                                };

            Symbol niftySymbol = new Symbol()
                                 {
                                     SymbolName = "Nifty",
                                     ExpirationDetails = new List<ExpirationDetail>()
                                                         {
                                                             oneDayExpiry,
                                                             twoDayExpiry
                                                         }
                                 };

            DerivativeType derivativeType = new DerivativeType(){DerviativeType = "UnitOfWork"};
            derivativeType.Symbols.Add(niftySymbol);
            
            unitOfWork.DerivativeType.Add(derivativeType);
            unitOfWork.Commit();

            Symbol symbol = unitOfWork.Symbols.GetAll().First(x => x.SymbolName=="Nifty");
            symbol.SymbolName = "NiftyChange";
            unitOfWork.Symbols.Update(symbol);
            unitOfWork.Commit();
        }
    }
}
