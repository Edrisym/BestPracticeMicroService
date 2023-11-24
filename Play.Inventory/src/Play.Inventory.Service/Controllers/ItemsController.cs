using System.Data.Common;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Play.Common;
using Play.Inventory.Service.Cients;
using Play.Inventory.Service.Dtos;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<InventoryItem> _itemsRepository;
        private readonly CatalogClients _catalogClients;

        public ItemsController(IRepository<InventoryItem> itemsRepository, CatalogClients catalogClients)
        {
            _itemsRepository = itemsRepository;
            _catalogClients = catalogClients;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid UserId)
        {
            if (UserId == Guid.Empty)
            {
                return BadRequest();
            }

            var catalogItems = await _catalogClients.GetCatalogItemAsync();
            var inventotyItemEntities = await _itemsRepository.GetAllItemsAsync(x => x.UserId == UserId);

            var inventoryItemDto = inventotyItemEntities.Select(inventoryItem =>
            {
                var catalogItem = catalogItems.Single(x => x.Id == inventoryItem.CatalogItemId);
                return inventoryItem.AsDto(catalogItem.Name, catalogItem.Description);
            });
            return Ok(inventoryItemDto);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(GrantItemsDto grantItemsDto)
        {
            var inventoryItem = await _itemsRepository.GetItemAsync(
                                        x => x.UserId == grantItemsDto.UserId
                                        && x.CatalogItemId == grantItemsDto.CatalogItemId);
            if (inventoryItem == null)
            {
                inventoryItem = new InventoryItem
                {
                    CatalogItemId = grantItemsDto.CatalogItemId,
                    UserId = grantItemsDto.UserId,
                    Quantity = grantItemsDto.Quantity,
                    AcquiredDate = DateTimeOffset.UtcNow
                };
                await _itemsRepository.Createsync(inventoryItem);
            }
            else
            {
                inventoryItem.Quantity += grantItemsDto.Quantity;
                await _itemsRepository.UpdateAsync(inventoryItem);
            }
            return Ok();
        }
    }
}