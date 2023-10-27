using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories
{
    public interface IItemsRepository
    {
       Task Createsync (Item entity);
       Task<IReadOnlyCollection<Item>> GetAllItemsAsync();
       Task<Item> GetItemAsync(Guid id);
       Task RemoveAsync (Guid id);
       Task UpdateAsync (Item entity);
    }
}