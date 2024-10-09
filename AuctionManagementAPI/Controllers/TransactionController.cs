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
    public class TransactionController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public TransactionController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<TransactionDTO>>> GetAllTransactions()
        {
            var transactions = await _dbContext.Transactions
                .Select(t => new TransactionDTO
                {
                    TransactionId = t.TransactionId,
                    Amount = t.Amount,
                    TransactionDate = t.TransactionDate,
                    UserId = t.UserId,
                    AuctionId = t.AuctionId,
                    PaymentMethod = t.PaymentMethod
                }).ToListAsync();

            if (transactions == null || transactions.Count == 0)
            {
                return NotFound();
            }

            return Ok(transactions);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TransactionDTO>> GetTransaction(int id)
        {
            var transaction = await _dbContext.Transactions
                .Select(t => new TransactionDTO
                {
                    TransactionId = t.TransactionId,
                    Amount = t.Amount,
                    TransactionDate = t.TransactionDate,
                    UserId = t.UserId,
                    AuctionId = t.AuctionId,
                    PaymentMethod = t.PaymentMethod
                })
                .FirstOrDefaultAsync(t => t.TransactionId == id);

            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TransactionDTO>> CreateTransaction([FromBody] TransactionDTO transactionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var transaction = new Transaction
            {
                Amount = transactionDto.Amount,
                TransactionDate = transactionDto.TransactionDate,
                UserId = transactionDto.UserId,
                AuctionId = transactionDto.AuctionId,
                PaymentMethod = transactionDto.PaymentMethod
            };

            _dbContext.Transactions.Add(transaction);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.TransactionId }, transactionDto);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTransaction(int id, [FromBody] TransactionDTO transactionDto)
        {
            if (id != transactionDto.TransactionId)
            {
                return BadRequest();
            }

            var transaction = await _dbContext.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            transaction.Amount = transactionDto.Amount;
            transaction.TransactionDate = transactionDto.TransactionDate;
            transaction.UserId = transactionDto.UserId;
            transaction.AuctionId = transactionDto.AuctionId;
            transaction.PaymentMethod = transactionDto.PaymentMethod;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction = await _dbContext.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            _dbContext.Transactions.Remove(transaction);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
