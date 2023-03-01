using MagicVilla_VillaAPI;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Repository;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File("log/villaLog.txt", rollingInterval: RollingInterval.Day).CreateLogger();
//builder.Host.UseSerilog();

//add database configuration
builder.Services.AddDbContext<ApplicationDbContext>(Options=>
{
  Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnectionString"));
});

builder.Services.AddTransient<IVillaRepository, VillaRepository>();//dependency injection
builder.Services.AddTransient<IVillaNumberRepository, VillaNumberRepository>();//dependency injection

builder.Services.AddSingleton<ILoggerCustom, Logg>();//this is how dependency injection will be added 

builder.Services.AddControllers(options =>
{
   // options.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson();//.AddXmlDataContractSerializerFormatters(); // it will help in patch api request
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(MappingConfig)); //add service to automapper
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
