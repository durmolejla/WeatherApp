namespace WeatherApp;

public partial class DrugiPage: ContentPage
{
	public DrugiPage()
	{
		InitializeComponent();
	}

    private async void OnVrijemeDanasClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Dnevna());
    }

    private async void OnVrijemeZa7DanaClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Sedmodnevna());
    }
    private async void NazadButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());

    }
}