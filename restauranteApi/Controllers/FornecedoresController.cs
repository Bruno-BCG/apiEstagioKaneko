using Microsoft.AspNetCore.Mvc;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace restauranteApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // This sets the base route, e.g., /api/Fornecedores
    public class FornecedoresController : ControllerBase
    {
        private readonly IFornecedoresRepository _repository;

        public FornecedoresController(IFornecedoresRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retrieves all Suppliers.
        /// GET /api/Fornecedores
        /// </summary>
        /// <returns>A list of Suppliers.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var fornecedores = await _repository.GetAllAsync();
            return Ok(fornecedores);
        }

        /// <summary>
        /// Retrieves a specific Supplier by ID.
        /// GET /api/Fornecedores/{id}
        /// </summary>
        /// <param name="id">The ID of the Supplier to retrieve.</param>
        /// <returns>The Supplier object if found, otherwise NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var fornecedor = await _repository.GetByIdAsync(id);
            return fornecedor == null ? NotFound() : Ok(fornecedor);
        }

        /// <summary>
        /// Creates a new Supplier.
        /// POST /api/Fornecedores
        /// </summary>
        /// <param name="fornecedor">The Fornecedores object to create.</param>
        /// <returns>The created Fornecedores with its assigned ID and a link to its location.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Fornecedores fornecedor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = await _repository.CreateAsync(fornecedor);
            fornecedor.Id = id; // Update the model with the generated ID
            return CreatedAtAction(nameof(GetById), new { id = id }, fornecedor);
        }

        /// <summary>
        /// Updates an existing Supplier.
        /// PUT /api/Fornecedores/{id}
        /// </summary>
        /// <param name="id">The ID of the Supplier to update.</param>
        /// <param name="fornecedor">The updated Fornecedores object.</param>
        /// <returns>NoContent if successful, BadRequest if IDs mismatch, NotFound if Supplier not found.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Fornecedores fornecedor)
        {
            if (id != fornecedor.Id)
            {
                return BadRequest("Supplier ID in route does not match body.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _repository.UpdateAsync(fornecedor);
            return updated ? NoContent() : NotFound();
        }

        /// <summary>
        /// Deletes a Supplier by ID.
        /// DELETE /api/Fornecedores/{id}
        /// </summary>
        /// <param name="id">The ID of the Supplier to delete.</param>
        /// <returns>NoContent if successful, NotFound if Supplier not found.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _repository.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}