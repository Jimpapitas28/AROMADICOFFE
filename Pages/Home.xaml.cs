namespace AROMADICOFFE.Pages;

public partial class Home : ContentPage
{
    // Define el ancho fijo de una columna cuando está comprimida
    private const double CollapsedWidth = 70;

    // Estado del corazón: Inicialmente no es favorito (corazón vacío)
    private bool isFavorite = false;

    public Home()
    {
        InitializeComponent();
        InitializeAccordion();
    }

    private void InitializeAccordion()
    {
        // Nota: Se mantiene el código de inicialización del acordeón como lo proporcionaste.
        for (int i = 0; i < 4; i++)
        {
            AccordionGrid.ColumnDefinitions[i].Width = new GridLength(CollapsedWidth);
        }
        _ = ExpandColumn(0, isInitialLoad: true);
    }

    private async void ImageTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is string paramString && int.TryParse(paramString, out int targetIndex))
        {
            await ExpandColumn(targetIndex);
        }
    }

    // Lógica de Expansión del Acordeón (CÓDIGO ORIGINAL DEL USUARIO, NO MODIFICADO)
    private async Task ExpandColumn(int targetIndex, bool isInitialLoad = false)
    {
        uint animationDuration = isInitialLoad ? 0u : 300u;
        var opacityTasks = new List<Task>();

        for (int i = 0; i < AccordionGrid.ColumnDefinitions.Count; i++)
        {
            var image = AccordionGrid.FindByName($"Img{i}") as Image;

            if (image != null)
            {
                double targetOpacity = (i == targetIndex) ? 1.0 : 0.5;
                opacityTasks.Add(image.FadeTo(targetOpacity, animationDuration, Easing.CubicOut));
            }
        }
        await Task.WhenAll(opacityTasks);

        AccordionGrid.Dispatcher.Dispatch(() =>
        {
            for (int i = 0; i < AccordionGrid.ColumnDefinitions.Count; i++)
            {
                var column = AccordionGrid.ColumnDefinitions[i];

                if (i == targetIndex)
                {
                    column.Width = GridLength.Star;
                }
                else
                {
                    column.Width = new GridLength(CollapsedWidth);
                }
            }
        });

        await Task.Delay(10);
    }


    // ===============================================
    // FUNCIONALIDADES SOLICITADAS
    // ===============================================

    private void SearchEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue?.ToLowerInvariant() ?? string.Empty;

        // Si el texto está vacío, mostramos todas las categorías
        if (string.IsNullOrWhiteSpace(searchText))
        {
            foreach (var child in CategoriesLayout.Children)
            {
                // === CORRECCIÓN CS1061: Conversión a View para acceder a IsVisible ===
                if (child is View view)
                {
                    view.IsVisible = true;
                }
            }
            return;
        }

        var categoryLabels = new Dictionary<string, string>
            {
                {"cafe", "Calientes"},
                {"postre", "Postres"},
                {"snack", "Snacks"},
                {"frio", "Frios"},
                {"caliente", "Calientes"}
            };

        // Filtrar las categorías
        foreach (var child in CategoriesLayout.Children)
        {
            // === CORRECCIÓN CS1061: Usar 'if (child is View view)' ===
            if (child is VerticalStackLayout stack && child is View view)
            {
                var label = stack.Children.OfType<Label>()
                                 .FirstOrDefault(l =>
                                     l.Text.Equals("Calientes") ||
                                     l.Text.Equals("Postres") ||
                                     l.Text.Equals("Snacks") ||
                                     l.Text.Equals("Frios"));

                if (label != null)
                {
                    string categoryName = label.Text.ToLowerInvariant();
                    bool matchFound = false;

                    // Coincidencia con palabras clave (ej: escribir 'cafe' para 'Calientes' o 'Frios')
                    foreach (var kvp in categoryLabels)
                    {
                        if (searchText.Contains(kvp.Key) && categoryName.ToLowerInvariant().Contains(kvp.Value.ToLowerInvariant()))
                        {
                            matchFound = true;
                            break;
                        }
                    }

                    // Coincidencia parcial con el nombre de la categoría (ej: escribir 'pos' para 'Postres')
                    if (categoryName.Contains(searchText))
                    {
                        matchFound = true;
                    }

                    // === ASIGNACIÓN CORREGIDA: Usar la variable 'view' ===
                    view.IsVisible = matchFound;
                }
            }
        }
    }

    private void LoQuieroButton_Clicked(object sender, EventArgs e)
    {
        ToggleFavorite(true);
    }

    private void FavoriteButton_Clicked(object sender, EventArgs e)
    {
        ToggleFavorite();
    }

    private void ToggleFavorite(bool forceFavorite = false)
    {
        if (forceFavorite && !isFavorite)
        {
            isFavorite = true;
        }
        else if (!forceFavorite)
        {
            isFavorite = !isFavorite;
        }

        if (isFavorite)
        {
            FavoriteButton.Source = "heart_filled.png";
        }
        else
        {
            FavoriteButton.Source = "heart_outline.png";
        }
    }
}