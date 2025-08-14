using Microsoft.AspNetCore.Mvc;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace restauranteApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaisesController : ControllerBase
    {
        private readonly IPaisesRepository _repository;

        public PaisesController(IPaisesRepository repository)
        {
            _repository = repository;
        }

        // GET: api/paises
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var paises = await _repository.GetAllAsync();
            return Ok(paises);
        }

        // GET: api/paises/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var pais = await _repository.GetByIdAsync(id);
            return pais is null ? NotFound() : Ok(pais);
        }

        // POST: api/paises
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Paises pais)
        {
            var newId = await _repository.CreateAsync(pais);
            pais.id = newId;
            return CreatedAtAction(nameof(GetById), new { id = newId }, pais);
        }

        // PUT: api/paises/1
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Paises pais)
        {
            if (id != pais.id) return BadRequest("id do path difere do body.");
            var ok = await _repository.UpdateAsync(pais);
            return ok ? NoContent() : NotFound();
        }

        // DELETE: api/paises/1
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _repository.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}