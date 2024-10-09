using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuctionManagementAPI.Models;
using AuctionManagementAPI.Models.Dto;
using System;

[Route("api/[controller]")]
[ApiController]
public class AuctionController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<AuctionController> _logger;

    public AuctionController(ApplicationDbContext dbContext , ILogger<AuctionController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<IEnumerable<AuctionDTO>> GetAllAuctions()
    {
        _logger.LogInformation("GetAllAuctions method called");
        return Ok(_dbContext.Auctions.ToList());
    }

    
    [HttpGet("{AuctionId:int}", Name = "GetAllAuctions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    //public async Task<IActionResult> GetAuctionById(int id)
    //{
    //    var auction = await _dbContext.Auctions.FindAsync(id);
    //    if (auction == null)
    //    {
    //        return NotFound();
    //    }
    //    return Ok(auction);
    //}

    public ActionResult<AuctionDTO> GetVilla(int AuctionId)
    {
        if (AuctionId == 0)
        {
            _logger.LogError("GetAllAuctions Error with ID " + AuctionId);
            return BadRequest();
        }
        var auction = _dbContext.Auctions.FirstOrDefault(u => u.AuctionId == AuctionId);
        if (auction == null)
        {
            return NotFound();
        }
        return Ok(auction);


    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult <AuctionDTO> CreateAuction([FromBody] AuctionDTO auctionDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        //if (_dbContext.Auctions.FirstOrDefault(u => u.ItemName.ToLower() == AuctionDTO.ItemName.ToLower()) != null)
        //{
        //    ModelState.AddModelError("Custom Error Message", "Item Name already exists");
        //    return BadRequest(ModelState);
        //}

        // Check if the SellerId exists
        //var sellerExists = _dbContext.Users.AnyAsync(u => u.UserId == auctionDto.SellerId);
        //if (!sellerExists)
        //{
        //    return BadRequest("Invalid SellerId. The user does not exist.");
        //}




        var auction = new Auction
        {
            // Assuming you changed AuctionDTO properties to match Auction
            ItemName = auctionDto.ItemName,
            StartingPrice = auctionDto.StartingPrice,
            CurrentPrice = auctionDto.StartingPrice, // Initial current price set to starting price
            StartDate = auctionDto.StartDate,
            EndDate = auctionDto.EndDate,
            IsClosed = false,
            SellerId = auctionDto.SellerId // Make sure this matches your Auction model
        };

        _dbContext.Auctions.Add(auction);
        _dbContext.SaveChanges();

        return CreatedAtRoute("Get Auction", new { id = auction.AuctionId }, auction);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAuction(int id, [FromBody] AuctionDTO auctionDto)
    {
        if (id != auctionDto.AuctionId) // Use AuctionId
        {
            return BadRequest();
        }

        var auction = await _dbContext.Auctions.FindAsync(id);
        if (auction == null)
        {
            return NotFound();
        }

        auction.ItemName = auctionDto.ItemName; // Ensure you're using the correct property
        auction.StartingPrice = auctionDto.StartingPrice;
        auction.Description = auctionDto.Description; // Update other properties as needed
        auction.StartDate = auctionDto.StartDate;
        auction.EndDate = auctionDto.EndDate;
        auction.IsClosed = auctionDto.IsClosed;
        auction.SellerId = auctionDto.SellerId;

        await _dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
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
