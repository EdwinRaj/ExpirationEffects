using System;
using Nse.Entities.Repository;

namespace Nse.Entities.UnitOfWork
{
    public class NseMetaDataUnitOfWork:INseMetaDataUnitOfWork,IDisposable
    {
        private NseContext _nseDbContext;

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
