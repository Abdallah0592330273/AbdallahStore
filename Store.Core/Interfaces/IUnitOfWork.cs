using Store.Core.Entities;

namespace Store.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : class, IEntity;
        Task<int> Complete();
    }
}
