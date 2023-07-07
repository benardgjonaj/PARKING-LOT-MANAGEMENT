using Microsoft.EntityFrameworkCore;
using ParkingLotManagementAPI.Data;
using ParkingLotManagementAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationContext>(
    dbContextOptions=> dbContextOptions.UseSqlServer(builder.Configuration
    .GetConnectionString("DefaultSQLConnection")));
builder.Services.AddScoped<IParkingSpotRepository,ParkingSpotRepository>();
builder.Services.AddScoped<IPricingPlanRepository,PricingPlanRepository>();
builder.Services.AddScoped<ISubscriberRepository,SubscriberRepository>();
builder.Services.AddScoped<ISubscriptionRepository,SubscriptionRepository>();
builder.Services.AddScoped<ILogsRepository,LogsRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
