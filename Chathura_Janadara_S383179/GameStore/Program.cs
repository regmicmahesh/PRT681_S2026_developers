using GameStore.Data;
using GameStore.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddValidation();
//builder.Services.AddControllers();

var connString = "Data Source=Gamestore.db";
builder.Services.AddSqlite<GameStoreContext>(connString);
var app = builder.Build();
//app.MapControllers();
app.MapGameEndpoints();


app.Run();