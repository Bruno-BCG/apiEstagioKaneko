using Microsoft.AspNetCore.Mvc;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic; // Added for IEnumerable
using System.Threading.Tasks;

namespace restauranteApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Route will be /api/Cidades
    public class CidadesController : ControllerBase
    {
        private readonly ICidadesRepository _repository;

        public CidadesController(ICidadesRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cidades = await _repository.GetAllAsync();
            return Ok(cidades);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cidade = await _repository.GetByIdAsync(id);
            return cidade == null ? NotFound() : Ok(cidade);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Cidades cidade)
        {
            var id = await _repository.CreateAsync(cidade);
            cidade.Id = id; // Set the ID after creation
            return CreatedAtAction(nameof(GetById), new { id = id }, cidade);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Cidades cidade)
        {
            // Ensure the ID from the route matches the ID in the body
            if (id != cidade.Id)
            {
                return BadRequest("City ID in route does not match body.");
            }

            var updated = await _repository.UpdateAsync(cidade);
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