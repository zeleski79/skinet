//**************
// SERVICES
//**************

using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<StoreContext>( opt => {
    // Get the our defult connection string from the appsettings.Development.json file
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); 
});

//**************
// MIDDLEWARE
//**************

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapControllers();

app.Run();
