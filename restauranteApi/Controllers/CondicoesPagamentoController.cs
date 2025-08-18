using Microsoft.AspNetCore.Mvc;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace restauranteApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CondicoesPagamentoController : ControllerBase
    {
        private readonly ICondicoesPagamentoRepository _repository;
        private readonly IParcelamentosRepository _parcelasRepo;

        public CondicoesPagamentoController(
            ICondicoesPagamentoRepository repository,
            IParcelamentosRepository parcelasRepo)
        {
            _repository = repository;
            _parcelasRepo = parcelasRepo;
        }

        // GET: api/condicoesPagamento  (já retorna com parcelamentos)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _repository.GetAllAsync();
            return Ok(list);
        }

        // GET: api/condicoesPagamento/2  (retorna com parcelamentos)
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CondicoesPagamento cond)
        {
            var newId = await _repository.CreateAsync(cond);

            // opcional: se vierem parcelamentos no body, faz upsert de cada um
            if (cond.parcelamentos is not null && cond.parcelamentos.Count > 0)
            {
                foreach (var p in cond.parcelamentos)
                {
                    p.condicoesPagamentoId = newId; // garante vínculo
                    await _parcelasRepo.UpdateAsync(p);
                }
            }

            var created = await _repository.GetByIdAsync(newId);
            return CreatedAtAction(nameof(GetById), new { id = newId }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CondicoesPagamento cond)
        {
            if (id != cond.id) return BadRequest("id do path difere do body.");

            var updated = await _repository.UpdateAsync(cond);

            // opcional: se vierem parcelamentos, aplica upsert (idempotente)
            if (updated is not null && cond.parcelamentos is not null)
            {
                foreach (var p in cond.parcelamentos)
                {
                    p.condicoesPagamentoId = id;
                    await _parcelasRepo.UpdateAsync(p);
                }
                updated = await _repository.GetByIdAsync(id); // recarrega com lista atualizada
            }

            return updated is null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _repository.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}