using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Repositories;
namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        
        // private static readonly List<ItemDto> items = new()
        // {
        //     new ItemDto(Guid.NewGuid(), "Potion", "Restores a small amount of HP", 5, DateTimeOffset.UtcNow),
        //     new ItemDto(Guid.NewGuid(), "Antidote", "Cures poison", 7, DateTimeOffset.UtcNow),
        //     new ItemDto(Guid.NewGuid(), "Bronze Sword", "Deals a small amount of damage", 20, DateTimeOffset.UtcNow)
        // };

        private readonly ItemsRepository itemsRepository= new ItemsRepository();
        // GET /items
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAllAsync()
        {
            var items = (await itemsRepository.GetAllItemsAsync()).Select(item => item.AsDto());
            return items;
        }
        // GET /items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
        {
            var item = (await itemsRepository.GetItemAsync(id))?.AsDto();
            if (item == null)
            {
                return NotFound();  
            }
            return item;
        }
        // POST /items
        [HttpPost]
        public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto createItemDto)
        {
            var item = new Item{
                Id = Guid.NewGuid(),
                Name = createItemDto.Name,
                Description = createItemDto.Description,
                Price = createItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
            await itemsRepository.CreateItemAsync(item);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);    
        }
        // PUT /items/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(Guid id, UpdateItemDto updateItemDto)
        {
            var existingItem = await itemsRepository.GetItemAsync(id);
            if (existingItem == null)
            {
                return NotFound();  
            }
           existingItem.Name = updateItemDto.Name;
           existingItem.Description = updateItemDto.Description;
           existingItem.Price = (decimal)updateItemDto.Price;    

            await itemsRepository.UpdateItemAsync(existingItem);
            return NoContent();
        }
        // DELETE /items/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            var existingItem = await itemsRepository.GetItemAsync(id);
            if (existingItem == null)
            {
                return NotFound();  
            }
            await itemsRepository.DeleteItemAsync(existingItem.Id);
            return NoContent();
        }   
    }
}