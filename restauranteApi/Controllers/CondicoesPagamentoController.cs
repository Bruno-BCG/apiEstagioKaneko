using Microsoft.AspNetCore.Mvc;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace restauranteApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // This sets the base route, e.g., /api/CondicoesPagamento
    public class CondicoesPagamentoController : ControllerBase
    {
        private readonly ICondicoesPagamentoRepository _repository;

        public CondicoesPagamentoController(ICondicoesPagamentoRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retrieves all Payment Conditions.
        /// GET /api/CondicoesPagamento
        /// </summary>
        /// <returns>A list of Payment Conditions.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var condicoesPagamento = await _repository.GetAllAsync();
            return Ok(condicoesPagamento);
        }

        /// <summary>
        /// Retrieves a specific Payment Condition by ID.
        /// GET /api/CondicoesPagamento/{id}
        /// </summary>
        /// <param name="id">The ID of the Payment Condition to retrieve.</param>
        /// <returns>The Payment Condition object if found, otherwise NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var condicaoPagamento = await _repository.GetByIdAsync(id);
            return condicaoPagamento == null ? NotFound() : Ok(condicaoPagamento);
        }

        /// <summary>
        /// Creates a new Payment Condition.
        /// POST /api/CondicoesPagamento
        /// </summary>
        /// <param name="condicaoPagamento">The CondicoesPagamento object to create.</param>
        /// <returns>The created CondicoesPagamento with its assigned ID and a link to its location.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CondicoesPagamento condicaoPagamento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = await _repository.CreateAsync(condicaoPagamento);
            condicaoPagamento.Id = id; // Update the model with the generated ID
            return CreatedAtAction(nameof(GetById), new { id = id }, condicaoPagamento);
        }

        /// <summary>
        /// Updates an existing Payment Condition.
        /// PUT /api/CondicoesPagamento/{id}
        /// </summary>
        /// <param name="id">The ID of the Payment Condition to update.</param>
        /// <param name="condicaoPagamento">The updated CondicoesPagamento object.</param>
        /// <returns>NoContent if successful, BadRequest if IDs mismatch, NotFound if Payment Condition not found.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CondicoesPagamento condicaoPagamento)
        {
            if (id != condicaoPagamento.Id)
            {
                return BadRequest("Payment condition ID in route does not match body.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _repository.UpdateAsync(condicaoPagamento);
            return updated ? NoContent() : NotFound();
        }

        /// <summary>
        /// Deletes a Payment Condition by ID.
        /// DELETE /api/CondicoesPagamento/{id}
        /// </summary>
        /// <param name="id">The ID of the Payment Condition to delete.</param>
        /// <returns>NoContent if successful, NotFound if Payment Condition not found.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _repository.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}