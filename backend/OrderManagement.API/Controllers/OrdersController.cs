using Microsoft.AspNetCore.Mvc;
using OrderManagement.API.Models;
using System.Collections.Generic;
using System.Linq;

namespace OrderManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        // TEMP in-memory storage (until MySQL is connected)
        private static List<Order> _orders = new List<Order>();
        private static int _nextId = 1;

        // GET: api/orders
        [HttpGet]
        public ActionResult<List<Order>> GetAll()
        {
            return Ok(_orders);
        }

        // GET: api/orders/{id}
        [HttpGet("{id}")]
        public ActionResult<Order> GetById(int id)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);

            if (order == null)
                return NotFound($"Order with ID {id} not found.");

            return Ok(order);
        }

        // POST: api/orders
        [HttpPost]
        public ActionResult<Order> Create(Order order)
        {
            order.Id = _nextId++;
            order.CreatedAt = System.DateTime.UtcNow;

            _orders.Add(order);

            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        // PUT: api/orders/{id}
        [HttpPut("{id}")]
        public ActionResult Update(int id, Order updatedOrder)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);

            if (order == null)
                return NotFound($"Order with ID {id} not found.");

            order.CustomerName = updatedOrder.CustomerName;
            order.ProductName = updatedOrder.ProductName;
            order.Quantity = updatedOrder.Quantity;
            order.TotalAmount = updatedOrder.TotalAmount;
            order.Status = updatedOrder.Status;

            return NoContent();
        }

        // DELETE: api/orders/{id}
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);

            if (order == null)
                return NotFound($"Order with ID {id} not found.");

            _orders.Remove(order);

            return NoContent();
        }
    }
}