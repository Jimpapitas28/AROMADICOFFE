namespace AROMADICOFFE;

public partial class SegundaPagina : ContentPage
{
    double precioBase = 2.50; // precio por café
    int cantidad = 1;
    int azucar = 0;

    public SegundaPagina()
	{
		InitializeComponent();
	}

    private void AzucarStepper_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        azucar = (int)e.NewValue;
        AzucarLabel.Text = $"Azúcar: {azucar} cucharaditas";
    }

    private void CantidadStepper_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        cantidad = (int)e.NewValue;
        CantidadLabel.Text = $"Cantidad: {cantidad} {(cantidad == 1 ? "café" : "cafés")}";
        CalcularTotal();
    }

    private void CalcularTotal()
    {
        double total = cantidad * precioBase;
        TotalLabel.Text = $"Total: ${total:F2}";
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (CafePicker.SelectedItem == null)
        {
            await DisplayAlert("Error", "Por favor selecciona un tipo de café.", "OK");
            return;
        }

        string cafe = CafePicker.SelectedItem.ToString();
        string mensaje = $"Has pedido {cantidad} {cafe}(s) con {azucar} cucharadita(s) de azúcar.\n\nTotal: ${cantidad * precioBase:F2}";
        await DisplayAlert("Pedido Confirmado ☕", mensaje, "Aceptar");
    }
}