using Arsenals.Desktop.Views.Models;
using CommunityToolkit.Mvvm.Messaging;

namespace Arsenals.Desktop.Views;

public partial class GunCategoryPage : ContentPage
{
	public GunCategoryPage(GunCategoryViewModel viewModel)
	{
		ArgumentNullException.ThrowIfNull(viewModel, nameof(viewModel));

		InitializeComponent();

		BindingContext = viewModel;
		_ = viewModel.FetchCategoryAsync();

		WeakReferenceMessenger.Default.Register<GunCategoryPage, SuccessMessage>(this, static async (page, message) =>
		{
			await page.DisplayAlert(message.Title, message.Value, message.Cancel);
		});

		WeakReferenceMessenger.Default.Register<GunCategoryPage, ErrorMessage>(this, static async (page, message) =>
		{
			await page.DisplayAlert(message.Title, message.Value.Message, message.Cancel);
		});
	}
}