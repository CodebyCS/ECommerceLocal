using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ECommerceLocal.Domain.Models
{
    public class Product
    {
        public ObjectId Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }
    }
}
