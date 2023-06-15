using Microsoft.EntityFrameworkCore;
using ModsenOnlineStore.Store.Infrastructure.Data;
using ModsenOnlineStore.Store.Application.Interfaces.OrderInterfaces;
using ModsenOnlineStore.Store.Infrastructure.Services.OrderService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IOrderService, OrderService>();


var a = builder.Configuration.GetConnectionString("DefaultConnection");
var b = builder.Configuration.GetSection("MigrationsAssembly").Get<string>();

builder.Services.AddDbContext<DataContext>(
    opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly(builder.Configuration.GetSection("MigrationsAssembly").Get<string>())));

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
