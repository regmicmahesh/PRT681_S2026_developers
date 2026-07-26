using System.ComponentModel.DataAnnotations;

namespace WebApi.Entities {
    public class Todo {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsComplete { get; set; } = false;
    }
}
