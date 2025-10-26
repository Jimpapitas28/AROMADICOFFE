using AROMADICOFFE.Models; // Asegúrate de que la ruta del namespace es correcta
using System.Collections.ObjectModel;
using System.Linq;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace AROMADICOFFE.Pages
{
    public partial class Compras : ContentPage
    {
        // ?? CAMBIO 1: La colección es de tipo CartItem ??
        public ObservableCollection<CartItem> CartItems { get; set; }

        public Compras()
        {
            InitializeComponent();

            // Cargar los CartItems de la lista estática compartida
            CartItems = new ObservableCollection<CartItem>(CafesHeladitos.ShoppingCart);

            CartCollectionView.ItemsSource = CartItems;

            UpdateTotal();
        }

        // =======================================================
        // LÓGICA DE ELIMINACIÓN DE PRODUCTOS
        // =======================================================

        private void RemoveItemClicked(object sender, EventArgs e)
        {
            var button = sender as ImageButton;
            // ?? CAMBIO 2: El item a remover es un CartItem ??
            var itemToRemove = button?.BindingContext as CartItem;

            if (itemToRemove != null)
            {
                // 1. Quitar de la lista local
                CartItems.Remove(itemToRemove);

                // 2. Quitar de la lista estática compartida (Carrito de CafesHeladitos)
                CafesHeladitos.ShoppingCart.Remove(itemToRemove);

                // 3. Recalcular el total
                UpdateTotal();

                DisplayAlert("Eliminado", $"{itemToRemove.Product.Name} removido del carrito.", "OK");
            }
        }

        // =======================================================
        // LÓGICA DE CÁLCULO DE TOTALES
        // =======================================================

        private void UpdateTotal()
        {
            double subtotal = 0;

            // ?? CAMBIO 3: Sumar el precio total de cada CartItem ??
            subtotal = CartItems.Sum(item => item.TotalItemPrice);

            // Muestra los totales formateados
            SubtotalLabel.Text = $"${subtotal:N2}";
            TotalLabel.Text = $"${subtotal:N2}";

            // Lógica para mostrar mensajes si el carrito está vacío
            bool isEmpty = CartItems.Count == 0;
            EmptyCartMessageLabel.IsVisible = isEmpty;
            CartCollectionView.IsVisible = !isEmpty;
            TotalSummaryFrame.IsVisible = !isEmpty;
        }

        // Constructor para compatibilidad
        public Compras(Coffee initialCoffee) : this()
        {
        }
    }
}