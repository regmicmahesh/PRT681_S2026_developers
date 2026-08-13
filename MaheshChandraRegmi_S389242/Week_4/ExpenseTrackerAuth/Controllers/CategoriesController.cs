using ExpenseTrackerAuth.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerAuth.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly ExpenseRepository _repository;

        public CategoriesController(ExpenseRepository repository)
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
