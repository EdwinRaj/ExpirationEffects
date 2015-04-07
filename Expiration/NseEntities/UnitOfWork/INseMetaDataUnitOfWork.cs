using NseEntities.Repository;

namespace NseEntities.UnitOfWork
{
    public interface INseMetaDataUnitOfWork
    {
            // Save pending changes to the data store.
            void Commit();

            // Repositories
            IRepositoryBase<DerivativeType> DerivativeType { get; }
            IRepositoryBase<ExpirationDetail> Expiration { get; }
            IRepositoryBase<Symbol> Symbols { get; }
    }
}
