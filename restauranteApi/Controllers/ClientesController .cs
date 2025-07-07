using Microsoft.AspNetCore.Mvc;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic; // Added for IEnumerable
using System.Threading.Tasks;

namespace restauranteApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Route will be /api/Clientes
    public class ClientesController : ControllerBase
    {
        private readonly IClientesRepository _repository;

        public ClientesController(IClientesRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clientes = await _repository.GetAllAsync();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cliente = await _repository.GetByIdAsync(id);
            return cliente == null ? NotFound() : Ok(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Clientes cliente)
        {
            var id = await _repository.CreateAsync(cliente);
            cliente.Id = id; // Set the ID after creation
            return CreatedAtAction(nameof(GetById), new { id = id }, cliente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Clientes cliente)
        {
            // Ensure the ID from the route matches the ID in the body
            if (id != cliente.Id)
            {
                return BadRequest("Client ID in route does not match body.");
            }

            var updated = await _repository.UpdateAsync(cliente);
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