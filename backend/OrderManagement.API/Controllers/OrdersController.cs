using Microsoft.AspNetCore.Mvc;
using OrderManagement.API.Models;
using System.Collections.Generic;
using System.Linq;
using AppDbContext = OrderManagement.API.Data.AppDbContext;

namespace OrderManagement.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class OrdersController : ControllerBase
	{
		// TEMP in-memory storage (until MySQL is connected)
		private readonly AppDbContext _context;

		public OrdersController(AppDbContext context)
		{
			_context = context;
		}

		// GET: api/orders
		[HttpGet]
		public ActionResult<List<Order>> GetAll()
		{
			return Ok(_context.Orders.ToList());
		}

		// GET: api/orders/{id}
		[HttpGet("{id}")]
		public ActionResult<Order> GetById(int id)
		{
			var order = _context.Orders.FirstOrDefault(o => o.Id == id);

			if (order == null)
				return NotFound($"Order with ID {id} not found.");

			return Ok(order);
		}

		// POST: api/orders
		[HttpPost]
		public ActionResult<Order> Create(Order order)
		{
			order.CreatedAt = System.DateTime.UtcNow;
			order.TotalAmount = order.Quantity * order.Price;

			_context.Orders.Add(order);
			_context.SaveChanges();

			return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
		}

		// PUT: api/orders/{id}
		[HttpPut("{id}")]
		public ActionResult Update(int id, Order updatedOrder)
		{
			var order = _context.Orders.FirstOrDefault(o => o.Id == id);

			if (order == null)
				return NotFound($"Order with ID {id} not found.");

			order.CustomerName = updatedOrder.CustomerName;
			order.ProductName = updatedOrder.ProductName;
			order.Quantity = updatedOrder.Quantity;
			order.TotalAmount = order.Quantity * order.Price;
			order.Status = updatedOrder.Status;

			_context.SaveChanges();

			return NoContent();
		}

		// DELETE: api/orders/{id}
		[HttpDelete("{id}")]
		public ActionResult Delete(int id)
		{
			var order = _context.Orders.FirstOrDefault(o => o.Id == id);

			if (order == null)
				return NotFound($"Order with ID {id} not found.");

			_context.Orders.Remove(order);
			_context.SaveChanges();

			return NoContent();
		}

		public async Task<ActionResult<Order>> PostOrder(CreateOrderDto dto)
		{
			var order = new Order
			{
				CustomerName = dto.CustomerName,
				ProductName = dto.ProductName,
				Quantity = dto.Quantity,
				Price = dto.Price,
				CreatedBy = dto.CreatedBy,
				Status = "Pending",
				CreatedAt = DateTime.UtcNow,
				TotalAmount = dto.Quantity * dto.Price
			};

			_context.Orders.Add(order);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
		}
	}
}