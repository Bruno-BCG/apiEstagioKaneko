// File: GrupoController.cs
using Microsoft.AspNetCore.Mvc;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace restauranteApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GruposController : ControllerBase
    {
        private readonly IGruposRepository _repo;
        public GruposController(IGruposRepository repo) => _repo = repo;

        [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _repo.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
            => (await _repo.GetByIdAsync(id)) is { } x ? Ok(x) : NotFound();

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Grupos grupo)
        {
            var id = await _repo.CreateAsync(grupo);
            var created = await _repo.GetByIdAsync(id);
            return CreatedAtAction(nameof(GetById), new { id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Grupos grupo)
        {
            if (id != grupo.id) return BadRequest("id do path difere do body.");
            var updated = await _repo.UpdateAsync(grupo);
            return updated is null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
            => (await _repo.DeleteAsync(id)) ? NoContent() : NotFound();
    }
}