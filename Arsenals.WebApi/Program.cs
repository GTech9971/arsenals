using System.Text.Encodings.Web;
using System.Text.Unicode;
using Arsenals.ApplicationServices.Guns;
using Arsenals.ApplicationServices.Guns.Dto;
using Arsenals.Domains;
using Arsenals.Domains.Bullets;
using Arsenals.Domains.Bullets.Services;
using Arsenals.Domains.Guns;
using Arsenals.Domains.Guns.Services;
using Arsenals.Infrastructure.Ef;
using Arsenals.Infrastructure.Ef.Bullets;
using Arsenals.Infrastructure.Ef.Guns;
using Arsenals.Infrastructure.FileStorage;
using Arsenals.Infrastructure.FileStorage.Guns;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//DB
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ArsenalDbContext>((_, options) =>
{
    options.UseNpgsql(connectionString, x => x.MigrationsAssembly(typeof(Program).Assembly.FullName));
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

//Json
builder.Services.AddControllers(options =>
{
    //フィルター
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.WriteIndented = true;
    options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();



public partial class Program { }