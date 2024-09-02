using Arsenals.ApplicationServices.Guns;

namespace Arsenals.Desktop.Views;

public partial class RegistryGunCategoryPage : ContentPage
{
	private readonly RegistryGunCategoryApplicationService _registryGunCategoryApplicationService;
	public RegistryGunCategoryPage(RegistryGunCategoryApplicationService registryGunCategoryApplicationService)
	{
		ArgumentNullException.ThrowIfNull(registryGunCategoryApplicationService, nameof(registryGunCategoryApplicationService));
		_registryGunCategoryApplicationService = registryGunCategoryApplicationService;

		InitializeComponent();

		this.BindingContext = new RegistryGunCategoryViewModel(_registryGunCategoryApplicationService);
	}
}