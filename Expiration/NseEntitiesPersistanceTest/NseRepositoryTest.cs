using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nse.Entities;
using Nse.Entities.Repository;
using NUnit.Framework;

namespace Nse.Entities.Test
{
    [TestFixture]
    public class NseRepositoryTest
    {
        private const string DbConnectionString = "NseTestContext";

        [Test]
        public void InsertDerivativeTypeSuccess()
        {
            
            var nseDbContext = new Entities.NseContext(DbConnectionString);
            var derivativeRepository = new RepositoryBase<DerivativeType>(nseDbContext);
            derivativeRepository.Add(new DerivativeType{DerviativeType="EdwinDerivative"});
            nseDbContext.SaveChanges();
        }

        [Test]
        public void InsertDerivativeTypeWithSymbolSuccess()
        {
            var nseDbContext = new Entities.NseContext(DbConnectionString);
            var derivativeRepository = new RepositoryBase<DerivativeType>(nseDbContext);
            derivativeRepository.Add(new DerivativeType
                                     {
                                         DerviativeType = "EdwinDerivative",
                                         Symbols = new List<Symbol>()
                                                   {
                                                       new Symbol()
                                                       {
                                                           SymbolName = "S1"
                                                       },
                                                       new Symbol()
                                                       {
                                                           SymbolName = "S2"
                                                       },
                                                       new Symbol()
                                                       {
                                                           SymbolName = "S3"
                                                       },
                                                   }
                                     });
            nseDbContext.SaveChanges();
        }
    }
}
