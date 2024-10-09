using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuctionManagementAPI.Models;
using AuctionManagementAPI.Models.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public ItemController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ItemDTO>>> GetAllItems()
        {
            var items = await _dbContext.Items
                .Select(i => new ItemDTO
                {
                    ItemId = i.ItemId,
                    Name = i.Name,
                    Description = i.Description,
                    ImageUrl = i.ImageUrl,
                    EstimatedValue = i.EstimatedValue,
                    AuctionId = i.AuctionId
                }).ToListAsync();

            if (items == null || items.Count == 0)
            {
                return NotFound();
            }

            return Ok(items);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ItemDTO>> GetItem(int id)
        {
            var item = await _dbContext.Items
                .Select(i => new ItemDTO
                {
                    ItemId = i.ItemId,
                    Name = i.Name,
                    Description = i.Description,
                    ImageUrl = i.ImageUrl,
                    EstimatedValue = i.EstimatedValue,
                    AuctionId = i.AuctionId
                })
                .FirstOrDefaultAsync(i => i.ItemId == id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ItemDTO>> CreateItem([FromBody] ItemDTO itemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = new Item
            {
                Name = itemDto.Name,
                Description = itemDto.Description,
                ImageUrl = itemDto.ImageUrl,
                EstimatedValue = itemDto.EstimatedValue,
                AuctionId = itemDto.AuctionId
            };

            _dbContext.Items.Add(item);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetItem), new { id = item.ItemId }, itemDto);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] ItemDTO itemDto)
        {
            if (id != itemDto.ItemId)
            {
                return BadRequest();
            }

            var item = await _dbContext.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            item.Name = itemDto.Name;
            item.Description = itemDto.Description;
            item.ImageUrl = itemDto.ImageUrl;
            item.EstimatedValue = itemDto.EstimatedValue;
            item.AuctionId = itemDto.AuctionId;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _dbContext.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _dbContext.Items.Remove(item);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
