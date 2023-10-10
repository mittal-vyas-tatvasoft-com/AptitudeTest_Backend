//using APTITUDETEST.Common.Data;
using APTITUDETEST.Common.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;



var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration.
  SetBasePath(System.IO.Directory.GetCurrentDirectory())
 .AddJsonFile($"appsettings.json", optional: false)
.AddEnvironmentVariables().Build();


var connectionString = configuration.GetConnectionString("AptitudeTest");


builder.Services.AddDbContext<AppDbContext>(item => item.UseNpgsql(connectionString));


var app = builder.Build();

app.Run();
