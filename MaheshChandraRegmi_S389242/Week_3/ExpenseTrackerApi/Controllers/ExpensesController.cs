using ExpenseTrackerApi.Data;
using ExpenseTrackerApi.Dtos;
using ExpenseTrackerApi.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerApi.Controllers
{
    [ApiController]
    [Route("api/expenses")]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseRepository _repository;

        public ExpensesController(IExpenseRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ExpenseDto>> GetAll([FromQuery] decimal? minAmount)
        {
            var expenses = minAmount is null
                ? _repository.GetAll()
                : _repository.GetWhere(e => e.Amount > minAmount.Value);

            return Ok(expenses.Select(ToDto));
        }

        [HttpGet("{id}")]
        public ActionResult<ExpenseDto> GetById(Guid id)
        {
            var expense = _repository.GetById(id);
            if (expense is null)
            {
                return NotFound();
            }

            return Ok(ToDto(expense));
        }

        [HttpGet("summary")]
        public ActionResult<SummaryDto> GetSummary()
        {
            var all = _repository.GetAll();
            return Ok(new SummaryDto(
                Count: all.Count,
                Total: _repository.GetTotal(),
                Average: _repository.GetAverage()
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
                CategoryId = dto.CategoryId
            };

            _repository.Add(expense);

            var created = _repository.GetById(expense.Id)!;
            return CreatedAtAction(nameof(GetById), new { id = expense.Id }, ToDto(created));
        }

        [HttpPut("{id}")]
        public ActionResult<ExpenseDto> Update(Guid id, [FromBody] UpdateExpenseDto dto)
        {
            var existing = _repository.GetById(id);
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

            var updated = _repository.GetById(id)!;
            return Ok(ToDto(updated));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            if (!_repository.Delete(id))
            {
                return NotFound();
            }

            return NoContent();
        }

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
