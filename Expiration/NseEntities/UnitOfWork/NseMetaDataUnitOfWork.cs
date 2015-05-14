using System;
using System.Collections.Generic;
using System.Linq;
using Nse.Entities.DTO;
using Nse.Entities.Repository;

namespace Nse.Entities.UnitOfWork
{
    public class NseMetaDataUnitOfWork:INseMetaDataUnitOfWork,IDisposable
    {
        private readonly NseContext _nseDbContext;

        public NseMetaDataUnitOfWork(string dbConnectionString):this(new NseContext(dbConnectionString))
        {
            
        }

        public NseMetaDataUnitOfWork(NseContext nseDbContext)
        {
            _nseDbContext = nseDbContext;
        }


        private IRepositoryBase<DerivativeType> _derivativeTypes;
        private IRepositoryBase<ExpirationDetail> _expirationDetail;
        private IRepositoryBase<Symbol> _symbols;

        public void Commit()
        {
            _nseDbContext.SaveChanges();
        }

        public IRepositoryBase<DerivativeType> DerivativeType
        {
            get
            {
                if (_derivativeTypes == null)
                {
                    _derivativeTypes = new RepositoryBase<DerivativeType>(_nseDbContext);
                }
                return _derivativeTypes;
            }
        }

        public IRepositoryBase<ExpirationDetail> Expiration
        {
            get
            {
                if (_expirationDetail == null)
                {
                    _expirationDetail = new RepositoryBase<ExpirationDetail>(_nseDbContext);
                }
                return _expirationDetail;
            }
        }

        public IRepositoryBase<Symbol> Symbols
        {
            get
            {
                if (_symbols == null)
                {
                    _symbols = new RepositoryBase<Symbol>(_nseDbContext);
                }
                return _symbols;
            }
        }


        public void SaveNseMetaData(List<IndexOptionMetaDataDto> indexOptionMetaDataDtos)
        {
            //Process all symbols and derviatives against database
            //Cache their ID
            //Check if expiry year and date exists
            //if doesnt exist, create one
            CacheDerivativeType(indexOptionMetaDataDtos);
            CacheSymbols(indexOptionMetaDataDtos);
            ProcessExpiryDates(indexOptionMetaDataDtos);
        }

        private void ProcessExpiryDates(List<IndexOptionMetaDataDto> indexOptionMetaDataDtos)
        {
            foreach (IndexOptionMetaDataDto currentMetaData in indexOptionMetaDataDtos)
            {
                int symbolId = _symbolDictionary[currentMetaData.Symbol];
                ExpirationDetail currentExpirationDetail = Expiration.GetAll().FirstOrDefault(x=>x.SymbolId == symbolId 
                                                                                            && x.ExpirationYear.ToLower() == currentMetaData.ExpiryYear.ToLower()
                                                                                            && x.ExpirationDate == currentMetaData.ExpiryDate);

                if (currentExpirationDetail == null)
                {
                    Expiration.Add(new ExpirationDetail
                                   {
                                       ExpirationYear = currentMetaData.ExpiryYear,
                                       ExpirationDate = currentMetaData.ExpiryDate,
                                       SymbolId = symbolId
                                   });
                    Commit();
                }
            }
        }

        private void CacheSymbols(List<IndexOptionMetaDataDto> indexOptionMetaDataDtos)
        {
            var symbolByInstrumentList = indexOptionMetaDataDtos.Select(x=> new { InstrumentType = x.DerivativeType, Symbol = x.Symbol}).Distinct().ToList();
            foreach (var currentSymbol in symbolByInstrumentList)
            {
                int instrumentTypeId = _instrumentTypeDictionary[currentSymbol.InstrumentType];
                Symbol symbolData = GetSymbol(currentSymbol.Symbol,currentSymbol.InstrumentType);
                if (symbolData == null)
                {
                    Symbols.Add(new Symbol {DerivativeTypeId = instrumentTypeId, SymbolName = currentSymbol.Symbol});
                    Commit();
                    symbolData = GetSymbol(currentSymbol.Symbol,currentSymbol.InstrumentType);
                }

                if (symbolData == null)
                {
                    throw new Exception(string.Format("Cannot find derivative for InstrumentName {0}", currentSymbol));
                }

                _symbolDictionary.Add(currentSymbol.Symbol,symbolData.SymbolId);
            }
        }

        private Symbol GetSymbol(string symbolName,string instrumentName)
        {
            return Symbols.GetAll().FirstOrDefault(x => x.SymbolName.ToLower() == symbolName.ToLower() 
                                                    && x.DerivativeType.DerviativeType.ToLower() == instrumentName.ToLower());
        }

        private void CacheDerivativeType(List<IndexOptionMetaDataDto> indexOptionMetaDataDtos)
        {
            List<string> instrumentTypes = indexOptionMetaDataDtos.Select(x => x.DerivativeType)
                .Distinct(StringComparer.InvariantCultureIgnoreCase)
                .ToList();
            foreach (string currentInstrumentType in instrumentTypes)
            {
                DerivativeType derivativeType = GetDerivativeByName(currentInstrumentType);
                if (derivativeType == null)
                {
                    DerivativeType.Add(new DerivativeType {DerviativeType = currentInstrumentType});
                    Commit();
                    derivativeType = GetDerivativeByName(currentInstrumentType);
                }
                if (derivativeType == null)
                {
                    throw new Exception(string.Format("Cannot find derivative for InstrumentName {0}", currentInstrumentType));
                }
                _instrumentTypeDictionary.Add(currentInstrumentType, derivativeType.DerivativeTypeId);
            }
        }

        private DerivativeType GetDerivativeByName(string currentInstrumentType)
        {
            return DerivativeType.GetAll().FirstOrDefault(inst => inst.DerviativeType.ToLower() == currentInstrumentType.ToLower());
        }

        readonly Dictionary<string, int> _instrumentTypeDictionary = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
        Dictionary<string, int> _symbolDictionary = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);

        private int GetDerivativeId(string derivativeName)
        {
            if (!_instrumentTypeDictionary.ContainsKey(derivativeName))
            {
                DerivativeType currentType = DerivativeType.GetAll()
                                                .First(x => x.DerviativeType.ToLowerInvariant() == derivativeName.ToLowerInvariant());
                if (currentType != null)
                {
                    int derivativeTypeId = currentType.DerivativeTypeId;
                    _instrumentTypeDictionary.Add(derivativeName, derivativeTypeId);
                }
                else
                {
                    
                }
            }
            return _instrumentTypeDictionary[derivativeName];
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_nseDbContext != null)
                {
                    _nseDbContext.Dispose();
                }
            }
        }

        #endregion
    }
}
