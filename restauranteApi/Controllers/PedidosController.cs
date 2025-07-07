using Microsoft.AspNetCore.Mvc;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace restauranteApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidosRepository _repository;

        public PedidosController(IPedidosRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pedidos = await _repository.GetAllAsync();
            return Ok(pedidos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var pedido = await _repository.GetByIdAsync(id);
            return pedido == null ? NotFound() : Ok(pedido);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Pedidos pedido)
        {
            var id = await _repository.CreateAsync(pedido);
            pedido.Id = id;
            return CreatedAtAction(nameof(GetById), new { id = id }, pedido);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Pedidos pedido)
        {
            if (id != pedido.Id)
            {
                return BadRequest("ID mismatch");
            }
            var updated = await _repository.UpdateAsync(pedido);
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