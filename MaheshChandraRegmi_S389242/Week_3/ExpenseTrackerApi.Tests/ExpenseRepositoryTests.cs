using ExpenseTrackerApi.Data;
using ExpenseTrackerApi.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApi.Tests
{
    public class ExpenseRepositoryTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly ExpenseDbContext _context;
        private readonly ExpenseRepository _repository;

        public ExpenseRepositoryTests()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<ExpenseDbContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new ExpenseDbContext(options);
            _context.Database.EnsureCreated();
            _repository = new ExpenseRepository(_context);
        }

        [Fact]
        public void Add_ThenGetAll_ReturnsTheExpense()
        {
            var expense = new Expense
            {
                Description = "Coffee",
                Amount = 5.50m,
                CategoryId = 1
            };

            _repository.Add(expense);

            var all = _repository.GetAll();
            Assert.Single(all);
            Assert.Equal("Coffee", all[0].Description);
            Assert.Equal("Food", all[0].Category!.Name);
        }

        [Fact]
        public void GetTotalAndAverage_MatchTheCliTrackerMath()
        {
            _repository.Add(new Expense { Description = "Bus", Amount = 10m, CategoryId = 2 });
            _repository.Add(new Expense { Description = "Lunch", Amount = 20m, CategoryId = 1 });

            Assert.Equal(30m, _repository.GetTotal());
            Assert.Equal(15m, _repository.GetAverage());
        }

        [Fact]
        public void GetWhere_FiltersByThreshold()
        {
            _repository.Add(new Expense { Description = "Snack", Amount = 4m, CategoryId = 1 });
            _repository.Add(new Expense { Description = "Rent", Amount = 400m, CategoryId = 3 });

            var expensive = _repository.GetWhere(e => e.Amount > 50);
            Assert.Single(expensive);
            Assert.Equal("Rent", expensive[0].Description);
        }

        [Fact]
        public void Delete_RemovesTheExpense()
        {
            var expense = new Expense { Description = "Temp", Amount = 1m, CategoryId = 4 };
            _repository.Add(expense);

            Assert.True(_repository.Delete(expense.Id));
            Assert.Empty(_repository.GetAll());
            Assert.Equal(0m, _repository.GetTotal());
            Assert.Equal(0m, _repository.GetAverage());
        }

        [Fact]
        public void Delete_UnknownId_ReturnsFalse()
        {
            Assert.False(_repository.Delete(Guid.NewGuid()));
        }

        public void Dispose()
        {
            _context.Dispose();
            _connection.Dispose();
        }
    }
}
