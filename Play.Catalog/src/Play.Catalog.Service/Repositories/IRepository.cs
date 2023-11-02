using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories
{
    public interface IRepository<T> where T : IEntity
    {
        Task Createsync(T entity);
        Task<IReadOnlyCollection<T>> GetAllItemsAsync();
        Task<T> GetItemAsync(Guid id);
        Task RemoveAsync(Guid id);
        Task UpdateAsync(T entity);
    }
}