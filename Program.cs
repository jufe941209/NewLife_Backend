using Microsoft.Extensions.FileProviders;
using NewLife.Helpers;

EnvLoader.CargarDotEnv();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddNewtonsoftJson();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

app.UseCors();

// Sirve las imagenes subidas por ImagenesController en /Uploads/productos
var uploadsPath = Path.Combine(builder.Environment.ContentRootPath, "Uploads");
Directory.CreateDirectory(uploadsPath);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = "/Uploads"
});

app.MapControllers();

// Render (y la mayoria de PaaS) inyectan el puerto real via la variable PORT.
// Hay que bindear a 0.0.0.0, no a localhost, para que el contenedor sea alcanzable.
string port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Run($"http://0.0.0.0:{port}");
