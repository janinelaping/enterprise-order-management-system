using System;

namespace OrderManagement.API.Models
{
    public class Order
    {
        public int Id { get; set; }  // ✅ REQUIRED (Primary Key)

        public string CustomerName { get; set; } = string.Empty;

        public string ProductName { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal Price { get; set; }
		
		public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Pending";

        public string CreatedBy { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // ✅ REQUIRED
    }
}