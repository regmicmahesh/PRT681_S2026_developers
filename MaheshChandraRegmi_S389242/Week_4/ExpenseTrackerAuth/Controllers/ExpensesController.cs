using System.Security.Claims;
using ExpenseTrackerAuth.Data;
using ExpenseTrackerAuth.Dtos;
using ExpenseTrackerAuth.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerAuth.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/expenses")]
    public class ExpensesController : ControllerBase
    {
        private readonly ExpenseRepository _repository;

        public ExpensesController(ExpenseRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ExpenseDto>> GetAll([FromQuery] decimal? minAmount)
        {
            var userId = CurrentUserId();
            var expenses = minAmount is null
                ? _repository.GetAllForUser(userId)
                : _repository.GetWhereForUser(userId, e => e.Amount > minAmount.Value);

            return Ok(expenses.Select(ToDto));
        }

        [HttpGet("{id}")]
        public ActionResult<ExpenseDto> GetById(Guid id)
        {
            var expense = _repository.GetByIdForUser(id, CurrentUserId());
            if (expense is null)
            {
                return NotFound();
            }

            return Ok(ToDto(expense));
        }

        [HttpGet("summary")]
        public ActionResult<SummaryDto> GetSummary()
        {
            var userId = CurrentUserId();
            var all = _repository.GetAllForUser(userId);
            return Ok(new SummaryDto(
                Count: all.Count,
                Total: _repository.GetTotalForUser(userId),
                Average: _repository.GetAverageForUser(userId)
            ));
        }

        [HttpPost]
        public ActionResult<ExpenseDto> Create([FromBody] CreateExpenseDto dto)
        {
            if (_repository.GetCategory(dto.CategoryId) is null)
            {
                return BadRequest("Unknown category.");
            }

            var expense = new Expense
            {
                Description = dto.Description,
                Amount = dto.Amount,
                CategoryId = dto.CategoryId,
                UserId = CurrentUserId()
            };

            _repository.Add(expense);

            var created = _repository.GetByIdForUser(expense.Id, expense.UserId)!;
            return CreatedAtAction(nameof(GetById), new { id = expense.Id }, ToDto(created));
        }

        [HttpPut("{id}")]
        public ActionResult<ExpenseDto> Update(Guid id, [FromBody] UpdateExpenseDto dto)
        {
            var existing = _repository.GetByIdForUser(id, CurrentUserId());
            if (existing is null)
            {
                return NotFound();
            }

            if (_repository.GetCategory(dto.CategoryId) is null)
            {
                return BadRequest("Unknown category.");
            }

            existing.Description = dto.Description;
            existing.Amount = dto.Amount;
            existing.CategoryId = dto.CategoryId;
            existing.Category = null;

            _repository.Update(existing);

            var updated = _repository.GetByIdForUser(id, CurrentUserId())!;
            return Ok(ToDto(updated));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            if (!_repository.DeleteForUser(id, CurrentUserId()))
            {
                return NotFound();
            }

            return NoContent();
        }

        private string CurrentUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("Token is missing the user id claim.");

        private static ExpenseDto ToDto(Expense expense) => new(
            expense.Id,
            expense.Description,
            expense.Amount,
            expense.CreatedAt,
            expense.CategoryId,
            expense.Category?.Name ?? ""
        );
    }
}
