using Microsoft.AspNetCore.Mvc;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic; // Added for IEnumerable
using System.Threading.Tasks;

namespace restauranteApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CidadesController : ControllerBase
    {
        private readonly ICidadesRepository _repository;

        public CidadesController(ICidadesRepository repository)
        {
            _repository = repository;
        }

        // GET: api/cidades
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _repository.GetAllAsync();
            return Ok(list);
        }

        // GET: api/cidades/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        // POST: api/cidades
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Cidades cidade)
        {
            var newId = await _repository.CreateAsync(cidade);
            var created = await _repository.GetByIdAsync(newId);
            return CreatedAtAction(nameof(GetById), new { id = newId }, created);
        }

        // PUT: api/cidades/1
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Cidades cidade)
        {
            if (id != cidade.id) return BadRequest("id do path difere do body.");
            var updated = await _repository.UpdateAsync(cidade);
            return updated is null ? NotFound() : Ok(updated);
        }

        // DELETE: api/cidades/1
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _repository.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}