using WebApi.Data;

namespace WebApi {
    public class WebApi {
        public static void Main(String[] args) {

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers();

            builder.Services.AddDbContext<WebApiDbContext>();
            builder.Services.AddScoped<SqliteRepository, SqliteRepository>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.MapControllers();

            app.Run();

        }
    }
}
