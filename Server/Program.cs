using System.Text.Json;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<FormOptions>(options =>
{
    // Set the limit to 1,024 MB
    options.MultipartBodyLengthLimit = 1024 * 1024 * 1024;
});

builder.Services.AddRouting();
builder.Services.AddControllers();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
var app = builder.Build();

var config = File.ReadAllText(Server.App.MapPath("config.json"));
Server.App.Config = JsonSerializer.Deserialize<Server.Models.Config>(config) ?? new Server.Models.Config();

app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
});

app.Run();
