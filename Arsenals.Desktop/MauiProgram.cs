using Microsoft.Extensions.Logging;
using Arsenals.ApplicationServices.Guns.Dto;
using Arsenals.Domains.Guns;
using Arsenals.Infrastructure.Ef;
using Arsenals.Infrastructure.Ef.Guns;
using Arsenals.Domains.Bullets;
using Arsenals.Domains;
using Arsenals.Domains.Users;
using Arsenals.Infrastructure.Ef.Bullets;
using Arsenals.Infrastructure.FileStorage;
using Arsenals.Infrastructure.FileStorage.Guns;
using Arsenals.Domains.Bullets.Services;
using Arsenals.Domains.Guns.Services;
using Arsenals.ApplicationServices.Guns;
using Arsenals.ApplicationServices.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Arsenals.Desktop.Views;
using System.Reflection;

namespace Arsenals.Desktop;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		string environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development";

		Assembly assembly = Assembly.GetExecutingAssembly();
		Type type = typeof(MauiProgram);

		using Stream? appSettingJsonStream = assembly.GetManifestResourceStream($"{type.Namespace}.appsettings.json");
		using Stream? appSettingVariableJsonStream = assembly.GetManifestResourceStream($"{type.Namespace}.appsettings.{environment}.json");

		ArgumentNullException.ThrowIfNull(appSettingJsonStream, nameof(appSettingJsonStream));
		ArgumentNullException.ThrowIfNull(appSettingVariableJsonStream, nameof(appSettingVariableJsonStream));

		builder.Configuration
				.AddJsonStream(appSettingJsonStream)
				.AddJsonStream(appSettingVariableJsonStream);

		//DI
		//DB
		string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
		builder.Services.AddDbContext<ArsenalDbContext>((_, options) =>
		{
			options.UseNpgsql(connectionString, x => x.MigrationsAssembly(typeof(MauiProgram).Assembly.FullName));
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

		builder.Services.AddScoped<LoginUserApplicationService>();

		//ViewModel
		builder.Services.AddTransient<GunCategoryViewModel>();

		//ページ
		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<GunCategoryPage>();
		//コンポーネント
		builder.Services.AddTransient<GunCard>();


#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}


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