using System;

namespace OrderManagement.API.Models
{
    public class Order
    {
        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } // Pending, Processing, Completed, Cancelled

		public DateTime? UpdatedAt { get; set; }
		
		public string CreatedBy { get; set; }    }
}