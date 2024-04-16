using AptitudeTest.Application.Services;
using AptitudeTest.Background_Services;
using AptitudeTest.Common.Data;
using AptitudeTest.Common.Helpers;
using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Data;
using AptitudeTest.Filters;
using APTITUDETEST.Common.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using Npgsql;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var configuration = builder.Configuration.
  SetBasePath(Directory.GetCurrentDirectory())
 .AddJsonFile($"appsettings.json", optional: false)
.AddEnvironmentVariables().Build();
var connectionString = configuration.GetConnectionString("AptitudeTest");

var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
dataSourceBuilder.MapComposite<DapperUserFamilyVM>();
dataSourceBuilder.MapComposite<DapperUserFamilyVM>();
var dataSource = dataSourceBuilder.Build();

builder.Services.AddHostedService<DeleteImagesJob>();
builder.Services.AddHostedService<TestStatusUpdateJob>();
builder.Services.AddDbContext<AppDbContext>(item => item.UseNpgsql(connectionString));
builder.Services.AddDbContext<DapperAppDbContext>(item =>
{
    item.UseNpgsql(dataSource);

});
builder.Services.AddMemoryCache();

LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/wwwroot/nlog.config"));

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IUserAuthenticationService, UserAuthenticationService>();
builder.Services.AddScoped<IUsersRepository, UserRepository>();
builder.Services.AddScoped<IUserAuthenticationRepository, UserAuthenticationRepository>();
builder.Services.AddScoped<IAdminAuthenticationService, AdminAuthenticationService>();
builder.Services.AddScoped<IAdminAuthenticationRepository, AdminAuthenticationRepository>();
builder.Services.AddTransient<ICollegeService, CollegeService>();
builder.Services.AddScoped<ICollegeRepository, CollegeRepository>();
builder.Services.AddTransient<IDegreeService, DegreeService>();
builder.Services.AddScoped<IDegreeRepository, DegreeRepository>();
builder.Services.AddTransient<IStreamService, StreamService>();
builder.Services.AddScoped<IStreamRepository, StreamRepository>();
builder.Services.AddTransient<IProfileService, ProfileService>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();

builder.Services.AddTransient<IGroupService, GroupService>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();

builder.Services.AddTransient<IQuestionService, QuestionService>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddTransient<IAdminService, AdminService>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddTransient<ITestService, TestService>();
builder.Services.AddScoped<ITestRepository, TestRepository>();
builder.Services.AddTransient<ICandidateService, CandidateService>();
builder.Services.AddScoped<ICandidateRepository, CandidateRepository>();
builder.Services.AddTransient<ISettingsService, SettingsService>();
builder.Services.AddScoped<ISettingsRepository, SettingsRepository>();
builder.Services.AddTransient<IResultService, ResultService>();
builder.Services.AddScoped<IResultRepository, ResultRepository>();
builder.Services.AddTransient<IScreenCaptureService, ScreenCaptureService>();
builder.Services.AddScoped<IScreenCaptureRepository, ScreenCaptureRepository>();
builder.Services.AddTransient<IReportsService, ReportsService>();
builder.Services.AddScoped<IReportsRepository, ReportsRepository>();
builder.Services.AddSingleton<ILoggerManager, LoggerManager>();

builder.Services.AddTransient<ISessionIdHelperInMemoryService, SessionIdHelperInMemoryService>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<SessionIdCheckFilterAttribute>();
builder.Services.AddScoped<UserActiveTestHelper>();

builder.Services.AddScoped<ISessionIdHelperInDbRepository, SessionIdHelperInDbRepository>();
builder.Services.AddScoped<ISessionIdHelperInDbService, SessionIdHelperInDbService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
    opt.OperationFilter<AddXIdSwagggerFilter>();
});
builder.Services.AddCors(option =>
{
    option.AddPolicy("MyPolicy", builder =>
    {
        //builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        //builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:4200");
        builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("https://aptitudetest-frontend-uat.web2.anasource.com");
    });
});

//add authentication 

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:AuthKey"])),
        ValidateAudience = false,
        ValidateIssuer = false,
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("MyPolicy");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

app.MapControllers();

app.Run();
