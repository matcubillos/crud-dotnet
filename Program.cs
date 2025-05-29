using WebApplication1.Data;
using Microsoft.EntityFrameworkCore;


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
}

// no usar en producción, solo para desarrollo o para inicializacion de la base de datos en primera version
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated(); // Crea la BD y tablas si no existen
}


app.UseCors("AllowAngularOrigin");

//descomentar si se quiere usar https redirection, no recomendado para desarrollo, pero si para producción
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
