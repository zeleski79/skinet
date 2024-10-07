//**************
// SERVICES
//**************

using API.Middleware;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<StoreContext>( opt => {
    // Get the our defult connection string from the appsettings.Development.json file
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); 
});
builder.Services.AddScoped<IProductRepository, ProductRepository>(); 
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddCors();

//**************
// MIDDLEWARE
//**************

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials()
    .WithOrigins("http://localhost:4200","https://localhost:4200"));

app.MapControllers();

try {
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<StoreContext>();
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context);
} catch (Exception ex) {
    Console.WriteLine(ex);
    throw;
}

app.Run();
