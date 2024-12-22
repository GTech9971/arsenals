using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Arsenals.ApplicationServices.Bullets;
using Arsenals.ApplicationServices.Guns;
using Arsenals.ApplicationServices.Guns.Dto;
using Arsenals.ApplicationServices.Users;
using Arsenals.Domains;
using Arsenals.Domains.Bullets;
using Arsenals.Domains.Bullets.Services;
using Arsenals.Domains.Guns;
using Arsenals.Domains.Guns.Services;
using Arsenals.Domains.Users;
using Arsenals.Infrastructure.Ef;
using Arsenals.Infrastructure.Ef.Bullets;
using Arsenals.Infrastructure.Ef.Guns;
using Arsenals.Infrastructure.FileStorage;
using Arsenals.Infrastructure.FileStorage.Guns;
using Arsenals.WebApi;
using Arsenals.WebApi.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

//Log
builder.Logging.AddLog4Net();

//DB
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ロガーファクトリの設定
var loggerFactory = LoggerFactory.Create(loggingBuilder =>
{
    loggingBuilder.AddLog4Net();
});

builder.Services.AddDbContext<ArsenalDbContext>((_, options) =>
{
    options.UseNpgsql(connectionString, x => x.MigrationsAssembly(typeof(Program).Assembly.FullName));
    options.UseLoggerFactory(loggerFactory); // ここでカスタムロガーファクトリを使用
    options.EnableSensitiveDataLogging(); // 重要: SQL の詳細を含めるためのオプション
});

// Json変換失敗時
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState.Values
                                            .SelectMany(x => x.Errors)
                                            .Select(x => x.ErrorMessage);
        var result = new
        {
            Errors = new
            {
                Message = string.Join(",", errors)
            }
        };
        return new BadRequestObjectResult(result);
    };
});

//AutoMapper
builder.Services.AddAutoMapper(typeof(DtoMappingProfile).Assembly);
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

//DI
//Repository
builder.Services.AddTransient<IGunRepository, EfGunRepository>();
builder.Services.AddTransient<IGunIdFactory, EfGunIdFactory>();
builder.Services.AddTransient<IGunCategoryRepository, EfGunCategoryRepository>();
builder.Services.AddTransient<IGunCategoryIdFactory, EfGunCategoryIdFactory>();
builder.Services.AddTransient<IBulletRepository, EfBulletRepository>();
builder.Services.AddTransient<IBulletIdFactory, EfBulletIdFactory>();
builder.Services.AddTransient<IFileManager, FSFileManager>();
builder.Services.AddTransient<IUserRepository, DummyUserRepository>();

builder.Services.AddTransient<IGunImageRepository, FSGunImageRepository>();

//Service
builder.Services.AddScoped<BulletService>();
builder.Services.AddScoped<GunCategoryService>();
builder.Services.AddScoped<GunService>();

//ApplicationService
builder.Services.AddScoped<FetchGunCategoryApplicationService>();
builder.Services.AddScoped<DeleteGunCategoryApplicationService>();
builder.Services.AddScoped<RegistryGunCategoryApplicationService>();
builder.Services.AddScoped<UpdateGunCategoryApplicationService>();

builder.Services.AddScoped<FetchAllGunApplicationService>();
builder.Services.AddScoped<FetchGunApplicationService>();
builder.Services.AddScoped<RegistryGunApplicationService>();
builder.Services.AddScoped<DeleteGunApplicationService>();
builder.Services.AddScoped<UpdateGunApplicationService>();

builder.Services.AddScoped<GunImageUploadApplicationService>();

builder.Services.AddScoped<RegistryBulletApplicationService>();

builder.Services.AddScoped<LoginUserApplicationService>();

//Filter
builder.Services.AddScoped<ExceptionFilter>();
builder.Services.AddScoped<LoggingFilter>();

//Jwt
builder.Services.AddScoped<JwtHandler>();

//Json
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
    options.Filters.Add<LoggingFilter>();
}).AddJsonOptions(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.JsonSerializerOptions.WriteIndented = true;
    }
    options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
});

//Auth
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        RequireExpirationTime = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration["JwtSettings:SecurityKey"]!
        ))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();



public partial class Program { }

public class DummyUserRepository : IUserRepository
{
    public async Task<bool> CheckPasswordAsync(UserId id, Password password)
    {
        return true;
    }

    public Task<User?> FetchAsync(UserId id)
    {
        return Task<User?>.Run(() =>
        {
            return new User(new UserId("test"), [new UserRole("Admin")]);
        });
    }
}