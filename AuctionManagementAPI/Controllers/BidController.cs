using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuctionManagementAPI.Models;
using AuctionManagementAPI.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuctionManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public BidController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<BidDTO>>> GetAllBids()
        {
            var bids = await _dbContext.Bids
                .Select(b => new BidDTO
                {
                    BidId = b.BidId,
                    BidAmount = b.BidAmount,
                    BidTime = b.BidTime,
                    AuctionId = b.AuctionId,
                    BidderId = b.BidderId
                }).ToListAsync();

            if (bids == null || bids.Count == 0)
            {
                return NotFound();
            }

            return Ok(bids);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BidDTO>> GetBid(int id)
        {
            var bid = await _dbContext.Bids
                .Select(b => new BidDTO
                {
                    BidId = b.BidId,
                    BidAmount = b.BidAmount,
                    BidTime = b.BidTime,
                    AuctionId = b.AuctionId,
                    BidderId = b.BidderId
                })
                .FirstOrDefaultAsync(b => b.BidId == id);

            if (bid == null)
            {
                return NotFound();
            }

            return Ok(bid);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BidDTO>> CreateBid([FromBody] BidDTO bidDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bid = new Bid
            {
                BidAmount = bidDto.BidAmount,
                BidTime = bidDto.BidTime,
                AuctionId = bidDto.AuctionId,
                BidderId = bidDto.BidderId
            };

            _dbContext.Bids.Add(bid);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBid), new { id = bid.BidId }, bidDto);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBid(int id, [FromBody] BidDTO bidDto)
        {
            if (id != bidDto.BidId)
            {
                return BadRequest();
            }

            var bid = await _dbContext.Bids.FindAsync(id);
            if (bid == null)
            {
                return NotFound();
            }

            bid.BidAmount = bidDto.BidAmount;
            bid.BidTime = bidDto.BidTime;
            bid.AuctionId = bidDto.AuctionId;
            bid.BidderId = bidDto.BidderId;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBid(int id)
        {
            var bid = await _dbContext.Bids.FindAsync(id);
            if (bid == null)
            {
                return NotFound();
            }

            _dbContext.Bids.Remove(bid);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
