using Microsoft.AspNetCore.Mvc;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;

namespace restauranteApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendasController : ControllerBase
    {
        private readonly IVendasRepository _repository;

        public VendasController(IVendasRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var vendas = await _repository.GetAllAsync();
            return Ok(vendas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var venda = await _repository.GetByIdAsync(id);
            return venda == null ? NotFound() : Ok(venda);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Vendas venda)
        {
            var id = await _repository.CreateAsync(venda);
            venda.Id = id;
            return CreatedAtAction(nameof(GetById), new { id = id }, venda);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Vendas venda)
        {
            venda.Id = id;
            var updated = await _repository.UpdateAsync(venda);
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