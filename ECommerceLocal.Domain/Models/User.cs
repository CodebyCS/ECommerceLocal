using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ECommerceLocal.Domain.Models
{
    public class User
    {
        public ObjectId Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
    }
}
