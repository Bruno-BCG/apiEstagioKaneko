using Microsoft.AspNetCore.Mvc;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace restauranteApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParcelamentosController : ControllerBase
    {
        private readonly IParcelamentosRepository _repository;

        public ParcelamentosController(IParcelamentosRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var parcelamentos = await _repository.GetAllAsync();
            return Ok(parcelamentos);
        }

        [HttpGet("{numeroParcela}/{condicaoPagamentoId}")]
        public async Task<IActionResult> GetById(int numeroParcela, int condicaoPagamentoId)
        {
            var parcelamento = await _repository.GetByIdAsync(numeroParcela, condicaoPagamentoId);
            return parcelamento == null ? NotFound() : Ok(parcelamento);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Parcelamentos parcelamento)
        {
            // For composite keys, you might need to handle ID generation or validation differently
            // depending on your database setup and business logic.
            // Assuming the composite key is provided in the body for creation.
            var created = await _repository.CreateAsync(parcelamento);
            return created ? CreatedAtAction(nameof(GetById), new { numeroParcela = parcelamento.NumeroParcela, condicaoPagamentoId = parcelamento.oCondicaoPagamento?.Id }, parcelamento) : BadRequest();
        }

        [HttpPut("{numeroParcela}/{condicaoPagamentoId}")]
        public async Task<IActionResult> Update(int numeroParcela, int condicaoPagamentoId, [FromBody] Parcelamentos parcelamento)
        {
            // Validate if the provided route IDs match the object's IDs
            if (numeroParcela != parcelamento.NumeroParcela || condicaoPagamentoId != parcelamento.oCondicaoPagamento?.Id)
            {
                return BadRequest("ID mismatch in route and body.");
            }

            var updated = await _repository.UpdateAsync(parcelamento);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{numeroParcela}/{condicaoPagamentoId}")]
        public async Task<IActionResult> Delete(int numeroParcela, int condicaoPagamentoId)
        {
            var deleted = await _repository.DeleteAsync(numeroParcela, condicaoPagamentoId);
            return deleted ? NoContent() : NotFound();
        }
    }
}