// File: ItensPedidosController.cs
using Microsoft.AspNetCore.Mvc;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace restauranteApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // e.g., /api/ItensPedidos
    public class ItensPedidosController : ControllerBase
    {
        private readonly IItensPedidosRepository _repository;

        public ItensPedidosController(IItensPedidosRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retrieves all Order Items.
        /// GET /api/ItensPedidos
        /// </summary>
        /// <returns>A list of Order Items.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var itensPedidos = await _repository.GetAllAsync();
            return Ok(itensPedidos);
        }

        /// <summary>
        /// Retrieves a specific Order Item by its composite key.
        /// GET /api/ItensPedidos/{pedidosId}/{numeroItem}
        /// </summary>
        /// <param name="pedidosId">The ID of the associated Pedido.</param>
        /// <param name="numeroItem">The item number within the Pedido.</param>
        /// <returns>The ItensPedidos object if found, otherwise NotFound.</returns>
        [HttpGet("{pedidosId}/{numeroItem}")]
        public async Task<IActionResult> GetById(int pedidosId, int numeroItem)
        {
            var itemPedido = await _repository.GetByIdAsync(pedidosId, numeroItem);
            return itemPedido == null ? NotFound() : Ok(itemPedido);
        }

        /// <summary>
        /// Creates a new Order Item.
        /// POST /api/ItensPedidos
        /// </summary>
        /// <param name="itemPedido">The ItensPedidos object to create. Requires oPedido and oMesa to have their IDs set.</param>
        /// <returns>The created ItensPedidos and a link to its location, or BadRequest/Conflict.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ItensPedidos itemPedido)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Ensure related IDs are present for the composite key
            if (itemPedido.oPedido == null || itemPedido.oPedido.Id == 0 || itemPedido.oMesa == null || itemPedido.oMesa.NumeroMesa == 0)
            {
                return BadRequest("Pedido ID and Mesa Number are required for creating an order item.");
            }

            // Check if an item with this composite key already exists
            var existingItem = await _repository.GetByIdAsync(itemPedido.oPedido.Id, itemPedido.NumeroItem);
            if (existingItem != null)
            {
                return Conflict($"Order item with Pedido ID {itemPedido.oPedido.Id} and Item Number {itemPedido.NumeroItem} already exists.");
            }

            var created = await _repository.CreateAsync(itemPedido);
            return created ? CreatedAtAction(nameof(GetById), new { pedidosId = itemPedido.oPedido.Id, numeroItem = itemPedido.NumeroItem }, itemPedido) : BadRequest("Could not create the order item.");
        }

        /// <summary>
        /// Updates an existing Order Item.
        /// PUT /api/ItensPedidos/{pedidosId}/{numeroItem}
        /// </summary>
        /// <param name="pedidosId">The ID of the associated Pedido in the route.</param>
        /// <param name="numeroItem">The item number within the Pedido in the route.</param>
        /// <param name="itemPedido">The updated ItensPedidos object.</param>
        /// <returns>NoContent if successful, BadRequest if keys mismatch, NotFound if Order Item not found.</returns>
        [HttpPut("{pedidosId}/{numeroItem}")]
        public async Task<IActionResult> Update(int pedidosId, int numeroItem, [FromBody] ItensPedidos itemPedido)
        {
            // Ensure route keys match body keys
            if (itemPedido.oPedido == null || pedidosId != itemPedido.oPedido.Id || numeroItem != itemPedido.NumeroItem)
            {
                return BadRequest("Composite key in route does not match body.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _repository.UpdateAsync(itemPedido);
            return updated ? NoContent() : NotFound();
        }

        /// <summary>
        /// Deletes an Order Item by its composite key.
        /// DELETE /api/ItensPedidos/{pedidosId}/{numeroItem}
        /// </summary>
        /// <param name="pedidosId">The ID of the associated Pedido.</param>
        /// <param name="numeroItem">The item number within the Pedido.</param>
        /// <returns>NoContent if successful, NotFound if Order Item not found.</returns>
        [HttpDelete("{pedidosId}/{numeroItem}")]
        public async Task<IActionResult> Delete(int pedidosId, int numeroItem)
        {
            var deleted = await _repository.DeleteAsync(pedidosId, numeroItem);
            return deleted ? NoContent() : NotFound();
        }
    }
}