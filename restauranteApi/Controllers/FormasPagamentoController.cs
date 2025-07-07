
using Microsoft.AspNetCore.Mvc;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace restauranteApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // This sets the base route, e.g., /api/FormasPagamento
    public class FormasPagamentoController : ControllerBase
    {
        private readonly IFormasPagamentoRepository _repository;

        public FormasPagamentoController(IFormasPagamentoRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retrieves all Payment Methods.
        /// GET /api/FormasPagamento
        /// </summary>
        /// <returns>A list of Payment Methods.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var formasPagamento = await _repository.GetAllAsync();
            return Ok(formasPagamento);
        }

        /// <summary>
        /// Retrieves a specific Payment Method by ID.
        /// GET /api/FormasPagamento/{id}
        /// </summary>
        /// <param name="id">The ID of the Payment Method to retrieve.</param>
        /// <returns>The Payment Method object if found, otherwise NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var formaPagamento = await _repository.GetByIdAsync(id);
            return formaPagamento == null ? NotFound() : Ok(formaPagamento);
        }

        /// <summary>
        /// Creates a new Payment Method.
        /// POST /api/FormasPagamento
        /// </summary>
        /// <param name="formaPagamento">The FormasPagamento object to create.</param>
        /// <returns>The created FormasPagamento with its assigned ID and a link to its location.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FormasPagamento formaPagamento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = await _repository.CreateAsync(formaPagamento);
            formaPagamento.Id = id; // Update the model with the generated ID
            return CreatedAtAction(nameof(GetById), new { id = id }, formaPagamento);
        }

        /// <summary>
        /// Updates an existing Payment Method.
        /// PUT /api/FormasPagamento/{id}
        /// </summary>
        /// <param name="id">The ID of the Payment Method to update.</param>
        /// <param name="formaPagamento">The updated FormasPagamento object.</param>
        /// <returns>NoContent if successful, BadRequest if IDs mismatch, NotFound if Payment Method not found.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] FormasPagamento formaPagamento)
        {
            if (id != formaPagamento.Id)
            {
                return BadRequest("Payment method ID in route does not match body.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _repository.UpdateAsync(formaPagamento);
            return updated ? NoContent() : NotFound();
        }

        /// <summary>
        /// Deletes a Payment Method by ID.
        /// DELETE /api/FormasPagamento/{id}
        /// </summary>
        /// <param name="id">The ID of the Payment Method to delete.</param>
        /// <returns>NoContent if successful, NotFound if Payment Method not found.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _repository.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}