using System.Linq.Expressions;

namespace Play.Common
{
    public interface IRepository<T> where T : IEntity
    {
        Task Createsync(T entity);
        Task<IReadOnlyCollection<T>> GetAllItemsAsync();
        Task<IReadOnlyCollection<T>> GetAllItemsAsync(Expression<Func<T, bool>> filter);
        Task<T> GetItemAsync(Guid id);
        Task<T> GetItemAsync(Expression<Func<T, bool>> filter);
        Task RemoveAsync(Guid id);
        Task UpdateAsync(T entity);
    }
}