using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuctionManagementAPI.Models;
using AuctionManagementAPI.Models.Dto;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class AuctionController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<AuctionController> _logger;

    public AuctionController(ApplicationDbContext dbContext, ILogger<AuctionController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    // GET all auctions
    [HttpGet]
    public ActionResult<IEnumerable<AuctionDTO>> GetAllAuctions()
    {
        _logger.LogInformation("GetAllAuctions method called");
        var auctions = _dbContext.Auctions
                                 .Select(a => new AuctionDTO
                                 {
                                     AuctionId = a.AuctionId,
                                     ItemName = a.ItemName,
                                     StartingPrice = a.StartingPrice,
                                     StartDate = a.StartDate,
                                     EndDate = a.EndDate,
                                     IsClosed = a.IsClosed,
                                     SellerId = a.SellerId
                                 }).ToList();

        return Ok(auctions);
    }

    // GET an auction by ID
    [HttpGet("{AuctionId:int}", Name = "GetAuction")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<AuctionDTO> GetAuction(int AuctionId)
    {
        if (AuctionId == 0)
        {
            _logger.LogError("Invalid AuctionId provided: " + AuctionId);
            return BadRequest();
        }

        var auction = _dbContext.Auctions.FirstOrDefault(u => u.AuctionId == AuctionId);
        if (auction == null)
        {
            return NotFound();
        }

        var auctionDto = new AuctionDTO
        {
            AuctionId = auction.AuctionId,
            ItemName = auction.ItemName,
            StartingPrice = auction.StartingPrice,
            StartDate = auction.StartDate,
            EndDate = auction.EndDate,
            IsClosed = auction.IsClosed,
            SellerId = auction.SellerId
        };

        return Ok(auctionDto);
    }

    // POST create an auction
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<AuctionDTO> CreateAuction([FromBody] AuctionDTO auctionDto)
    {
        if (auctionDto == null || !ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Ensure SellerId exists
        var sellerExists = _dbContext.Users.Any(u => u.UserId == auctionDto.SellerId);
        if (!sellerExists)
        {
            return BadRequest("Invalid SellerId. The user does not exist.");
        }

        var auction = new Auction
        {
            ItemName = auctionDto.ItemName,
            StartingPrice = auctionDto.StartingPrice,
            CurrentPrice = auctionDto.StartingPrice,
            StartDate = auctionDto.StartDate,
            EndDate = auctionDto.EndDate,
            IsClosed = false,
            SellerId = auctionDto.SellerId
        };

        _dbContext.Auctions.Add(auction);
        _dbContext.SaveChanges();

        return CreatedAtRoute("GetAuction", new { AuctionId = auction.AuctionId }, auctionDto);
    }

    // PUT update an auction
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAuction(int id, [FromBody] AuctionDTO auctionDto)
    {
        if (id != auctionDto.AuctionId || auctionDto == null)
        {
            return BadRequest();
        }

        var auction = await _dbContext.Auctions.FindAsync(id);
        if (auction == null)
        {
            return NotFound();
        }

        // Map the fields from auctionDto to the auction entity
        auction.ItemName = auctionDto.ItemName;
        auction.StartingPrice = auctionDto.StartingPrice;
        auction.Description = auctionDto.Description;
        auction.StartDate = auctionDto.StartDate;
        auction.EndDate = auctionDto.EndDate;
        auction.IsClosed = auctionDto.IsClosed;
        auction.SellerId = auctionDto.SellerId;

        await _dbContext.SaveChangesAsync();
        return NoContent();
    }

    // DELETE an auction
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAuction(int id)
    {
        var auction = await _dbContext.Auctions.FindAsync(id);
        if (auction == null)
        {
            return NotFound();
        }

        _dbContext.Auctions.Remove(auction);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }
}
