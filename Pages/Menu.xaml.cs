namespace AROMADICOFFE.Pages;

public partial class Menu : ContentPage
{
	public Menu()
	{
		InitializeComponent();
	}

    private void Snacks_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Snacks());
    }
}