using Arsenals.Desktop.Views;

namespace Arsenals.Desktop;

public partial class MainPage : ContentPage
{
	int count = 0;

	private readonly RegistryGunCategoryPage _registryGunCategoryPage;

	public MainPage(RegistryGunCategoryPage registryGunCategoryPage)
	{
		ArgumentNullException.ThrowIfNull(registryGunCategoryPage, nameof(registryGunCategoryPage));
		_registryGunCategoryPage = registryGunCategoryPage;

		InitializeComponent();
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}

	private async void OnClickGunCategoryPage(object sender, EventArgs e)
	{
		await Navigation.PushAsync(_registryGunCategoryPage);
	}
}

