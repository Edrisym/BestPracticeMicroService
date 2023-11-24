using Play.Inventory.Service.Dtos;

namespace Play.Inventory.Service.Cients
{
    public class CatalogClients
    {
        private readonly HttpClient _httpClient;
        public CatalogClients(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyCollection<CatalogItemDto>> GetCatalogItemAsync()
        {
            var items = await _httpClient.GetFromJsonAsync<IReadOnlyCollection<CatalogItemDto>>("/items");
            return items;
        }
    }
}