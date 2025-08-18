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

        // GET: api/parcelamentos/condicao/2
        [HttpGet("condicao/{condicoesPagamentoId:int}")]
        public async Task<IActionResult> GetByCondicao(int condicoesPagamentoId)
        {
            var list = await _repository.GetByCondicaoIdAsync(condicoesPagamentoId);
            return Ok(list);
        }

        // GET: api/parcelamentos/condicao/2/parcela/1
        [HttpGet("condicao/{condicoesPagamentoId:int}/parcela/{numeroParcela:int}")]
        public async Task<IActionResult> GetByChave(int condicoesPagamentoId, int numeroParcela)
        {
            var item = await _repository.GetByAsync(condicoesPagamentoId, numeroParcela);
            return item is null ? NotFound() : Ok(item);
        }

        // POST: api/parcelamentos
        // Body deve conter: condicoesPagamentoId, numeroParcela, formaPagamento{id}, prazoDias, porcentagemValor...
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Parcelamentos parcela)
        {
            var newId = await _repository.CreateAsync(parcela);

            // Opcional: retornar pela chave composta para garantir consistência do payload
            var created = await _repository.GetByAsync(parcela.condicoesPagamentoId, parcela.numeroParcela);
            return CreatedAtAction(
                nameof(GetByChave),
                new { condicoesPagamentoId = parcela.condicoesPagamentoId, numeroParcela = parcela.numeroParcela },
                created);
        }

        // PUT: api/parcelamentos/condicao/2/parcela/1
        [HttpPut("condicao/{condicoesPagamentoId:int}/parcela/{numeroParcela:int}")]
        public async Task<IActionResult> Update(int condicoesPagamentoId, int numeroParcela, [FromBody] Parcelamentos parcela)
        {
            if (condicoesPagamentoId != parcela.condicoesPagamentoId || numeroParcela != parcela.numeroParcela)
                return BadRequest("Chave composta do path difere do body.");

            var updated = await _repository.UpdateAsync(parcela);
            return updated is null ? NotFound() : Ok(updated);
        }

        // DELETE: api/parcelamentos/condicao/2/parcela/1
        [HttpDelete("condicao/{condicoesPagamentoId:int}/parcela/{numeroParcela:int}")]
        public async Task<IActionResult> Delete(int condicoesPagamentoId, int numeroParcela)
        {
            var ok = await _repository.DeleteAsync(condicoesPagamentoId, numeroParcela);
            return ok ? NoContent() : NotFound();
        }
    }
}