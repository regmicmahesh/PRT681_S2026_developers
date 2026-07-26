using WebApi.Entities;

namespace WebApi.Data {

    public class SqliteRepository {
        private WebApiDbContext _context;

        public SqliteRepository(WebApiDbContext context)
        {
            _context = context;
        }

        public List<Todo> GetAll() => [.. _context.Todos];

        public Todo? GetById(Guid guid) => _context.Todos.Find(guid);

        public Todo CreateTodo(Todo todo) {
             _context.Todos.Add(todo);
            _context.SaveChanges();
            return todo;
        }


    }
}
