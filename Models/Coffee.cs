using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AROMADICOFFE.Models
{
    public class Coffee
    {
        public string Name { get; set; }
        public string Subtitle { get; set; }
        public string Price { get; set; }
        public string ImageSource { get; set; }
        public string Category { get; set; } // Útil para filtrar en CafesHeladitos

        // Constructor para facilitar la creación de objetos
        public Coffee(string name, string subtitle, string price, string imageSource, string category)
        {
            Name = name;
            Subtitle = subtitle;
            Price = price;
            ImageSource = imageSource;
            Category = category;
        }
    }
}
