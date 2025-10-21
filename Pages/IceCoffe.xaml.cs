namespace AROMADICOFFE.Pages;

public partial class IceCoffe : ContentPage
{
    // Colores de los recursos XAML para usar en C#
    private readonly Color primaryCoffee = Color.FromArgb("#4A3F35");
    private readonly Color lightBackground = Color.FromArgb("#E0D1C0");
    private readonly Color selectedCategoryColor = Color.FromArgb("#886140");

    public IceCoffe()
	{
		InitializeComponent();
        // Llama al filtro inicial para mostrar las "Especialidades" al cargar
        FilterProducts("Todos");

    }

    // 1. Maneja el clic en los botones de categoría
    private void OnCategoryClicked(object sender, EventArgs e)
    {
        var selectedButton = sender as Button;

        if (selectedButton == null) return;

        // 1.1 Reiniciar el estilo de TODOS los botones
        foreach (var child in CategoryButtonsLayout.Children)
        {
            if (child is Button button)
            {
                button.BackgroundColor = lightBackground;
                button.TextColor = primaryCoffee;
            }
        }

        // 1.2 Aplicar el estilo al botón seleccionado
        selectedButton.BackgroundColor = selectedCategoryColor;
        selectedButton.TextColor = Colors.White;

        // 1.3 Aplicar el filtro de productos
        FilterProducts(selectedButton.Text);
    }

    // 2. Lógica de filtrado de productos
    private void FilterProducts(string category)
    {
        // Oculta todos los productos por defecto
        AmericanoGrid.IsVisible = false;
        MochaGrid.IsVisible = false;
        CarameloGrid.IsVisible = false;
        FresaGrid.IsVisible = false;
        ChocolateGrid.IsVisible = false;

        // Muestra solo los productos que coincidan con el filtro
        switch (category)
        {
            case "Todos":
                // Frapuchinos (3)
                CarameloGrid.IsVisible = true;
                FresaGrid.IsVisible = true;
                ChocolateGrid.IsVisible = true;
                AmericanoGrid.IsVisible = true;
                MochaGrid.IsVisible = true;
                break;
            case "Especialidades":
                // Frapuchinos (3)
                CarameloGrid.IsVisible = true;
                FresaGrid.IsVisible = true;
                ChocolateGrid.IsVisible = true;
                break;
            case "Clásicos":
                // Americano (1)
                AmericanoGrid.IsVisible = true;
                break;
            case "De la Casa":
                // Moca (1)
                MochaGrid.IsVisible = true;
                break;
                // Puedes añadir más casos si tienes más categorías
        }
    }

    // 3. Mantiene la funcionalidad de la estrella
    private void OnFavoriteStarClicked(object sender, EventArgs e)
    {
        var button = sender as ImageButton;

        if (button != null)
        {
            // Obtenemos el archivo de la imagen actual
            string currentSource = (button.Source as FileImageSource)?.File;

            if (currentSource == "star_outline_icon.png")
            {
                button.Source = "star_filled_icon.png";
            }
            else
            {
                button.Source = "star_outline_icon.png";
            }
        }

    }

}