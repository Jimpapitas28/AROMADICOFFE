using AROMADICOFFE.Models; // Aseg�rate de que la ruta del namespace es correcta
using System.Collections.ObjectModel;
using System.Linq;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace AROMADICOFFE.Pages
{
    public partial class Compras : ContentPage
    {
        // ?? CAMBIO 1: La colecci�n es de tipo CartItem ??
        public ObservableCollection<CartItem> CartItems { get; set; }

        public Compras()
        {
            InitializeComponent();

            // Cargar los CartItems de la lista est�tica compartida
            CartItems = new ObservableCollection<CartItem>(CafesHeladitos.ShoppingCart);

            CartCollectionView.ItemsSource = CartItems;

            UpdateTotal();
        }

        // =======================================================
        // L�GICA DE ELIMINACI�N DE PRODUCTOS
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

                // 2. Quitar de la lista est�tica compartida (Carrito de CafesHeladitos)
                CafesHeladitos.ShoppingCart.Remove(itemToRemove);

                // 3. Recalcular el total
                UpdateTotal();

                DisplayAlert("Eliminado", $"{itemToRemove.Product.Name} removido del carrito.", "OK");
            }
        }

        // =======================================================
        // L�GICA DE C�LCULO DE TOTALES
        // =======================================================

        private void UpdateTotal()
        {
            double subtotal = 0;

            // ?? CAMBIO 3: Sumar el precio total de cada CartItem ??
            subtotal = CartItems.Sum(item => item.TotalItemPrice);

            // Muestra los totales formateados
            SubtotalLabel.Text = $"${subtotal:N2}";
            TotalLabel.Text = $"${subtotal:N2}";

            // L�gica para mostrar mensajes si el carrito est� vac�o
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