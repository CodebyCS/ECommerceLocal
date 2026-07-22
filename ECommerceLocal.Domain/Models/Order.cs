using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ECommerceLocal.Domain.Models
{
    public class Order
    {
        public ObjectId Id { get; set; }

        public ObjectId UserId { get; set; }

        public List<OrderItem> Items { get; set; } = new();

        public decimal Total { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Pendente";
    }
}
