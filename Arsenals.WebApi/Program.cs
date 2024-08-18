using System.Text.Encodings.Web;
using System.Text.Unicode;
using Arsenals.ApplicationServices.Guns;
using Arsenals.Domains;
using Arsenals.Domains.Bullets.Services;
using Arsenals.Domains.Guns.Services;
using Arsenals.Infrastructure.FileStorage;

var builder = WebApplication.CreateBuilder(args);

//DB

//DI
//Repository
builder.Services.AddTransient<IFileManager, FSFileManager>();

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