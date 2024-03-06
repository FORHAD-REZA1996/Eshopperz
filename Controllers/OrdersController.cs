using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eshopperz.Models;

// Define the namespace for the OrderController class within the Eshopperz.Controllers namespace.
namespace Eshopperz.Controllers
{
    // Declare a public class named OrderController that inherits from ControllerBase.
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        // Private field to store the database context injected through the constructor.
        private readonly EshopperzContext _context;

        // Constructor to initialize the database context.
        public OrderController(EshopperzContext context)
        {
            _context = context;
        }

        // Action to retrieve all orders.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            // Retrieve and return all orders from the database.
            return await _context.Orders.ToListAsync();
        }

        // Action to retrieve a specific order by its ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            // Finding the order entry in the database based on the provided ID.
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                // Returning a NotFound response if the order with the provided ID cannot be found.
                return NotFound();
            }

            // Returning the found order entry.
            return order;
        }

        // Action to update an existing order.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            // Checking if the provided ID matches the OrderId of the order object.
            if (id != order.OrderId)
            {
                // Returning a BadRequest response if the IDs do not match.
                return BadRequest();
            }

            // Setting the state of the order object in the context to Modified.
            _context.Entry(order).State = EntityState.Modified;

            try
            {
                // Saving changes to the database.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    // Returning a NotFound response if the order with the provided ID cannot be found.
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Returning an Ok response indicating successful update of the order.
            // Also providing a message indicating the changes made.
            return Ok(new { message = $"Changes made to account with orderId {order.OrderId}" });
        }

        // Action to create a new order.
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            // Check if an order with the same ID already exists.
            var existingOrder = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == order.OrderId);

            if (existingOrder != null)
            {
                // Order with the same ID already exists, return a conflict response.
                return Conflict($"A Order with the OREDERID {order.OrderId} is already added to the Cart with id {order.CartId}");
            }

            // No existing order with the same ID, proceed with adding the new order.
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Returning a response indicating successful creation of the order.
            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        }

        // Action to delete a specific order by its ID.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            // Finding the order entry in the database based on the provided ID.
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                // Returning a Conflict response if the order with the provided ID cannot be found.
                return Conflict($"A Order with id {id} not found");
            }

            // Removing the order from the context and saving changes to the database.
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            // Returning a response indicating successful deletion of the order.
            return NoContent();
        }

        // Private method to check if an order with a given ID exists.
        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}