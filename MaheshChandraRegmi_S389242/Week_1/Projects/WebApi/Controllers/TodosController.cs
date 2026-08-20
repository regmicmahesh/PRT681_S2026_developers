using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.Dtos;
using WebApi.Entities;

namespace WebApi.Controllers
{


    [ApiController]
    [Route("api/todos")]
    public class TodosController : ControllerBase
    {

        private SqliteRepository _repository;

        public TodosController(SqliteRepository repository)
        {
            _repository = repository;
        }



        [HttpGet]
        public ActionResult<IEnumerable<Todo>> GetAll() => _repository.GetAll();

        [HttpGet("{id}")]
        public ActionResult<Todo?> GetTodoById(Guid id) => _repository.GetById(id);

        [HttpPost]
        public ActionResult<Todo> CreateTodo([FromBody] CreateTodoDto todoDto)
        {
            {
                var todo = new Todo
                {
                    Title = todoDto.Title,
                    Description = todoDto.Description
                };

                _repository.CreateTodo(todo);
                return CreatedAtAction(
                    nameof(GetTodoById),
                    new { id = todo.Id },
                    todo
                );



            }


        }
    }
}
