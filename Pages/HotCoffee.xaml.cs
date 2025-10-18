namespace AROMADICOFFE.Pages;

public partial class CoffeHot : ContentPage
{
	public CoffeHot()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new SegundaPagina());
    }
}