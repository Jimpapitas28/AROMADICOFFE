using AROMADICOFFE.Models; // Asegúrate de que la ruta del namespace es correcta
using System.ComponentModel; // Necesario para PropertyChangedEventHandler

namespace AROMADICOFFE.Models
{
    // Opcional: Implementar INotifyPropertyChanged si la cantidad o el precio total
    // necesita actualizarse en tiempo real en la página de Compras sin recargarla.
    public class CartItem : INotifyPropertyChanged
    {
        // El producto base
        public Coffee Product { get; set; }

        // Propiedades de personalización del pedido
        public string SelectedSize { get; set; } // Ejemplo: Pequeño, Mediano, Grande
        public string SugarLevel { get; set; }   // Ejemplo: Sin Azúcar, Normal, Extra

        // Propiedad de cantidad (con lógica de notificación si es necesario)
        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                    OnPropertyChanged(nameof(TotalItemPrice)); // Notificar el cambio de precio
                }
            }
        }

        // Propiedad calculada para el precio total del ítem (Cantidad * Precio base)
        public double TotalItemPrice
        {
            get
            {
                // Extrae el precio base del string (ej: "$10.75") y lo multiplica por la cantidad.
                if (double.TryParse(
                    Product.Price.Replace("$", "").Trim(),
                    System.Globalization.NumberStyles.Currency,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out double priceValue))
                {
                    // Nota: Aquí podrías añadir lógica de precio por tamaño si la tienes.
                    return priceValue * Quantity;
                }
                return 0; // En caso de error de parseo
            }
        }

        // Implementación de INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Propiedad de solo lectura para mostrar el precio total formateado en el XAML de Compras
        public string FormattedTotalItemPrice => $"${TotalItemPrice:N2}";

        // Propiedad para mostrar la selección completa en Compras
        public string ItemDetails => $"{SelectedSize}, Azúcar: {SugarLevel} (x{Quantity})";
    }
}