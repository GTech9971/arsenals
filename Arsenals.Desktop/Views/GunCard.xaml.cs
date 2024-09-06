namespace Arsenals.Desktop.Views;

public partial class GunCard : ContentView
{
	public static readonly BindableProperty TitleProperty =
		   BindableProperty.Create(nameof(Title), typeof(string), typeof(GunCard), string.Empty, propertyChanged: OnTitleChanged);

	public static readonly BindableProperty DescriptionProperty =
		BindableProperty.Create(nameof(Description), typeof(string), typeof(GunCard), string.Empty, propertyChanged: OnDescriptionChanged);

	public static readonly BindableProperty ImageSourceProperty =
		BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(GunCard), null, propertyChanged: OnImageSourceChanged);

	public string Title
	{
		get => (string)GetValue(TitleProperty);
		set => SetValue(TitleProperty, value);
	}

	public string Description
	{
		get => (string)GetValue(DescriptionProperty);
		set => SetValue(DescriptionProperty, value);
	}

	public ImageSource ImageSource
	{
		get => (ImageSource)GetValue(ImageSourceProperty);
		set => SetValue(ImageSourceProperty, value);
	}

	public GunCard()
	{
		InitializeComponent();
	}

	private static void OnTitleChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var control = (GunCard)bindable;
		control.CardTitle.Text = (string)newValue;
	}

	private static void OnDescriptionChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var control = (GunCard)bindable;
		control.CardDescription.Text = (string)newValue;
	}

	private static void OnImageSourceChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var control = (GunCard)bindable;
		control.CardImage.Source = (ImageSource)newValue;
	}
}