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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var paises = await _repository.GetAllAsync();
            return Ok(paises);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var pais = await _repository.GetByIdAsync(id);
            return pais == null ? NotFound() : Ok(pais);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Paises pais)
        {
            var id = await _repository.CreateAsync(pais);
            pais.Id = id;
            return CreatedAtAction(nameof(GetById), new { id = id }, pais);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Paises pais)
        {
            if (id != pais.Id)
            {
                return BadRequest("ID mismatch");
            }
            var updated = await _repository.UpdateAsync(pais);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _repository.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}