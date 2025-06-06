using Microsoft.EntityFrameworkCore;
using WebApplication1.Extensions;
using WebApplication1.Persistense;

DotNetEnv.Env.Load(); // Cargar variables de entorno desde .env
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));
builder.Services.AddScoped<DbInitializer>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularOrigin",
        builder => builder.WithOrigins("http://localhost:4200")
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var app = builder.Build();

//configuraciones extras necesarias para la seguridad de la app
app.Use(async (context, next) =>
{
    var headers = context.Response.Headers;

    // prevención básica
    headers.XContentTypeOptions = "nosniff";
    headers.XFrameOptions = "DENY";
    headers.XXSSProtection = "1; mode=block";

    // seguridad en transporte (HTTPS obligatorio), solo para producción
  //headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains; preload";

    // política de Referencia (no envía referrer fuera del dominio)
    headers["Referrer-Policy"] = "strict-origin-when-cross-origin";

    // política de permisos (API del navegador)
    headers["Permissions-Policy"] = "geolocation=(), microphone=(), camera=()";

    await next();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    await app.InitializeDatabaseAsync();
}

await app.InitializeDatabaseAsync();


app.UseCors("AllowAngularOrigin");

////descomentar si se quiere usar https redirection, no recomendado para desarrollo, pero si para producción
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
