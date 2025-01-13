using System.Diagnostics;
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
using Arsenals.WebApi.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Okta.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

//Log
builder.Logging.AddLog4Net();

IHostEnvironment environment = builder.Environment;
Debug.WriteLine(environment.EnvironmentName);
Debug.WriteLine(Directory.GetCurrentDirectory());

// DB設定読み込み
builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("dbsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"dbsettings.{environment.EnvironmentName}.json", optional: true);

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
// *コンストラクタ引数つきProfileを必要とする場合の設定
// https://christosmonogios.com/2023/01/21/How-To-Pass-Parameters-To-An-AutoMapper-Profile-Constructor-aka-Dependency-Injection-For-AutoMapper/
builder.Services.AddAutoMapper(
    (serviceProvider, mapperConfiguration) =>
        mapperConfiguration.AddProfile(new DtoMappingProfile(serviceProvider.GetRequiredService<IConfiguration>())),
        typeof(DtoMappingProfile).Assembly
);
//コンストラクタ引数が入らない場合は以下で良い
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
var logger = loggerFactory.CreateLogger<Program>();
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
    opt.DefaultChallengeScheme = OktaDefaults.ApiAuthenticationScheme;
    opt.DefaultSignInScheme = OktaDefaults.ApiAuthenticationScheme;
}).AddOktaWebApi(new OktaWebApiOptions()
{
    OktaDomain = builder.Configuration["Okta:OktaDomain"],
    AuthorizationServerId = builder.Configuration["Okta:AuthorizationServerId"],
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
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
        return Task<User>.Run(() =>
        {
            return (User?)new User(new UserId("test"), [new UserRole("Admin")]);
        });
    }
}