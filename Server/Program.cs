using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var app = builder.Build();

var config = File.ReadAllText(Server.App.MapPath("config.json"));
Server.App.Settings = JsonSerializer.Deserialize<Server.Models.Config>(config) ?? new Server.Models.Config();

app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();
app.Run();
