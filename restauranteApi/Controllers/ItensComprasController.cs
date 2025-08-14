// File: ItensComprasController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace restauranteApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // e.g., /api/ItensCompras
    public class ItensComprasController : ControllerBase
    {
        private readonly IItensComprasRepository _repository;

        public ItensComprasController(IItensComprasRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retrieves all Purchase Items.
        /// GET /api/ItensCompras
        /// </summary>
        /// <returns>A list of Purchase Items.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var itensCompras = await _repository.GetAllAsync();
            return Ok(itensCompras);
        }

        /// <summary>
        /// Retrieves a specific Purchase Item by ID.
        /// GET /api/ItensCompras/{id}
        /// </summary>
        /// <param name="id">The ID of the Purchase Item to retrieve.</param>
        /// <returns>The ItensCompras object if found, otherwise NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var itemCompra = await _repository.GetByIdAsync(id);
            return itemCompra == null ? NotFound() : Ok(itemCompra);
        }

        /// <summary>
        /// Creates a new Purchase Item.
        /// POST /api/ItensCompras
        /// </summary>
        /// <param name="itemCompra">The ItensCompras object to create. Requires oCompra and oProduto to have their IDs/keys set.</param>
        /// <returns>The created ItensCompras with its assigned ID and a link to its location.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] itensPedidos itemCompra)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Ensure related IDs are present if they are part of required data for creation
            if (itemCompra.oCompra == null || itemCompra.oProduto == null || itemCompra.oProduto.Id == 0)
            {
                // For Compras, its composite key (Modelo, Serie, Numero, FornecedorId) must be set
                if (string.IsNullOrEmpty(itemCompra.oCompra.Modelo.ToString()) || string.IsNullOrEmpty(itemCompra.oCompra.Serie.ToString()) || itemCompra.oCompra.Numero == 0 || itemCompra.oCompra.oFornecedor?.Id == 0)
                {
                    return BadRequest("Associated Compras and Produto details (including Compras composite key) are required for creating a purchase item.");
                }
            }

            var id = await _repository.CreateAsync(itemCompra);
            itemCompra.Id = id; // Update the model with the generated ID
            return CreatedAtAction(nameof(GetById), new { id = id }, itemCompra);
        }

        /// <summary>
        /// Updates an existing Purchase Item.
        /// PUT /api/ItensCompras/{id}
        /// </summary>
        /// <param name="id">The ID of the Purchase Item to update.</param>
        /// <param name="itemCompra">The updated ItensCompras object.</param>
        /// <returns>NoContent if successful, BadRequest if IDs mismatch, NotFound if Purchase Item not found.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] itensPedidos itemCompra)
        {
            if (id != itemCompra.Id)
            {
                return BadRequest("Purchase Item ID in route does not match body.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _repository.UpdateAsync(itemCompra);
            return updated ? NoContent() : NotFound();
        }

        /// <summary>
        /// Deletes a Purchase Item by ID.
        /// DELETE /api/ItensCompras/{id}
        /// </summary>
        /// <param name="id">The ID of the Purchase Item to delete.</param>
        /// <returns>NoContent if successful, NotFound if Purchase Item not found.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _repository.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}