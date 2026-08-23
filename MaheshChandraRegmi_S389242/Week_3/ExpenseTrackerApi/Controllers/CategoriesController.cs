using ExpenseTrackerApi.Data;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerApi.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly IExpenseRepository _repository;

        public CategoriesController(IExpenseRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_repository.GetCategories().Select(c => new
            {
                c.Id,
                c.Name
            }));
        }
    }
}
