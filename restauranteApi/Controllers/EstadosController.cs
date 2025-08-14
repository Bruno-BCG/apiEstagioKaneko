using Microsoft.AspNetCore.Mvc;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic; // Required for IEnumerable
using System.Threading.Tasks;

namespace restauranteApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadosController : ControllerBase
    {
        private readonly IEstadosRepository _repository;

        public EstadosController(IEstadosRepository repository)
        {
            _repository = repository;
        }

        // GET: api/estados
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var estados = await _repository.GetAllAsync();
            return Ok(estados);
        }

        // GET: api/estados/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var estado = await _repository.GetByIdAsync(id);
            return estado is null ? NotFound() : Ok(estado);
        }

        // POST: api/estados
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Estados estado)
        {
            var newId = await _repository.CreateAsync(estado);
            var created = await _repository.GetByIdAsync(newId);
            return CreatedAtAction(nameof(GetById), new { id = newId }, created);
        }

        // PUT: api/estados/1
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Estados estado)
        {
            if (id != estado.id) return BadRequest("id do path difere do body.");
            var updated = await _repository.UpdateAsync(estado);
            return updated is null ? NotFound() : Ok(updated);
        }

        // DELETE: api/estados/1
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _repository.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}