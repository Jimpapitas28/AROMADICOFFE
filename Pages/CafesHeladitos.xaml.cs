// CafesHeladitos.xaml.cs

using AROMADICOFFE.Models; // Necesario para las clases Coffee y CartItem
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;
using System; // Necesario para el try/catch y las conversiones

namespace AROMADICOFFE.Pages
{
    public partial class CafesHeladitos : ContentPage
    {
        // La lista estática COMPARTIDA de artículos en el carrito es de tipo CartItem
        public static List<CartItem> ShoppingCart { get; set; } = new List<CartItem>();

        // Colores de los recursos XAML (se mantienen igual)
        private readonly Color primaryCoffee = Color.FromArgb("#4A3F35");
        private readonly Color lightBackground = Color.FromArgb("#E0D1C0");
        private readonly Color selectedCategoryColor = Color.FromArgb("#886140");

        // LISTA MAESTRA DE CAFÉS (Productos base)
        private List<Coffee> AllCoffees = new List<Coffee>
        {
            new Coffee("Américano Frio", "Elixir Matinal", "$12.50", "americano2.png", "Clásicos"),
            new Coffee("Moca Frio de la Casa", "Esencia Pura", "$10.75", "moca2.jpg", "De la Casa"),
            new Coffee("Frapuchino de Caramelo", "Despertar Intenso", "$15.00", "caramelo.png", "Especialidades"),
            new Coffee("Frappuchino de Fresa", "Esencia Pura", "$10.75", "strawberry.jpg", "Especialidades"),
            new Coffee("Frappé de Chocolate", "Esencia Pura", "$10.75", "chocolate.png", "Especialidades"),
        };

        public CafesHeladitos()
        {
            InitializeComponent();
            FilterProducts("Todos");
        }

        // =======================================================
        // LÓGICA DE NAVEGACIÓN Y CARRITO
        // =======================================================

        // 1. Método para el botón flotante de carrito (Navega a Compras)
        private async void OnCartButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Compras());
        }

        // 2. Método para el botón "COMPRAR" en cada tarjeta (ACTUALIZADO)
        private async void OnBuyClicked(object sender, EventArgs e)
        {
            var button = sender as Button;

            // Subimos en la jerarquía visual para encontrar el Grid de producto
            if (button.Parent is Grid innerGrid && innerGrid.Parent is Frame frame && frame.Parent is Grid productGrid)
            {
                var nameLabel = innerGrid.Children.OfType<Label>()
                                                 .FirstOrDefault(c => Grid.GetColumn(c) == 1 && Grid.GetRow(c) == 0);

                if (nameLabel != null)
                {
                    string coffeeName = nameLabel.Text;
                    var selectedCoffee = AllCoffees.FirstOrDefault(c => c.Name == coffeeName);

                    if (selectedCoffee != null)
                    {
                        // 1. Preguntar por Tamaño (con emoji de café ☕)
                        string selectedSize = await DisplayActionSheet("Selecciona Tamaño ☕", "Cancelar", null, "Pequeño", "Mediano", "Grande");

                        if (selectedSize == "Cancelar" || selectedSize == null) return; // Usuario canceló

                        // 2. Preguntar por Azúcar (con emoji de cuchara 🥄)
                        string selectedSugar = await DisplayActionSheet("Nivel de Azúcar 🥄", "Cancelar", null, "Sin Azúcar", "Normal", "Extra");

                        if (selectedSugar == "Cancelar" || selectedSugar == null) return; // Usuario canceló

                        // 3. 🌟 NUEVA FUNCIONALIDAD: Preguntar por Cantidad 🌟
                        string quantityText = await DisplayPromptAsync("Cantidad", "¿Cuántos quieres?", "Aceptar", "Cancelar", placeholder: "Ingresa un número", keyboard: Keyboard.Numeric);

                        // Si cancela o no ingresa nada
                        if (quantityText == null || string.IsNullOrWhiteSpace(quantityText)) return;

                        int quantity = 0;
                        if (!int.TryParse(quantityText, out quantity) || quantity <= 0)
                        {
                            await DisplayAlert("Error ❌", "Por favor, ingresa una cantidad válida (número entero mayor a cero).", "OK");
                            return;
                        }

                        // 4. Crear el nuevo CartItem con las selecciones
                        var newItem = new CartItem
                        {
                            Product = selectedCoffee,
                            SelectedSize = selectedSize,
                            SugarLevel = selectedSugar,
                            Quantity = quantity // Ya obtenida del DisplayPromptAsync
                        };

                        // 5. Agregar al carrito estático
                        ShoppingCart.Add(newItem);

                        await DisplayAlert("Agregado ✅",
                                           $"{newItem.Product.Name} ({newItem.SelectedSize}, {newItem.SugarLevel}) x{newItem.Quantity} añadido al carrito.",
                                           "OK");
                    }
                }
            }
        }

        // =======================================================
        // LÓGICA DE FILTRADO Y ESTILOS (Se mantienen igual)
        // =======================================================

        // 3. Maneja el clic en los botones de categoría
        private void OnCategoryClicked(object sender, EventArgs e)
        {
            var selectedButton = sender as Button;

            if (selectedButton == null) return;

            // 3.1 Reiniciar el estilo de TODOS los botones
            foreach (var child in CategoryButtonsLayout.Children)
            {
                if (child is Button button)
                {
                    button.BackgroundColor = lightBackground;
                    button.TextColor = primaryCoffee;
                }
            }

            // 3.2 Aplicar el estilo al botón seleccionado
            selectedButton.BackgroundColor = selectedCategoryColor;
            selectedButton.TextColor = Colors.White;

            // 3.3 Aplicar el filtro de productos
            FilterProducts(selectedButton.Text);
        }

        // 4. Lógica de filtrado de productos
        private void FilterProducts(string category)
        {
            AmericanoGrid.IsVisible = false;
            MochaGrid.IsVisible = false;
            CarameloGrid.IsVisible = false;
            FresaGrid.IsVisible = false;
            ChocolateGrid.IsVisible = false;

            switch (category)
            {
                case "Todos":
                    CarameloGrid.IsVisible = true;
                    FresaGrid.IsVisible = true;
                    ChocolateGrid.IsVisible = true;
                    AmericanoGrid.IsVisible = true;
                    MochaGrid.IsVisible = true;
                    break;
                case "Especialidades":
                    CarameloGrid.IsVisible = true;
                    FresaGrid.IsVisible = true;
                    ChocolateGrid.IsVisible = true;
                    break;
                case "Clásicos":
                    AmericanoGrid.IsVisible = true;
                    break;
                case "De la Casa":
                    MochaGrid.IsVisible = true;
                    break;
            }
        }

        // 5. Mantiene la funcionalidad de la estrella
        private void OnFavoriteStarClicked(object sender, EventArgs e)
        {
            var button = sender as ImageButton;

            if (button != null)
            {
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
}