namespace Arsenals.Desktop.Views;

public partial class RegistryGunCategoryPage : ContentPage
{
	public RegistryGunCategoryPage(RegistryGunCategoryViewModel viewModel)
	{
		ArgumentNullException.ThrowIfNull(viewModel, nameof(viewModel));

		InitializeComponent();

		BindingContext = viewModel;
		_ = viewModel.FetchCategoryAsync();
	}
}