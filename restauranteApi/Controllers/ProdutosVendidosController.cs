using Microsoft.AspNetCore.Mvc;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;

namespace restauranteApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosVendidosController : ControllerBase
    {
        private readonly IProdutosVendidosRepository _repository;

        public ProdutosVendidosController(IProdutosVendidosRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var produtosVendidos = await _repository.GetAllAsync();
            return Ok(produtosVendidos);
        }

        [HttpGet("{numeroItem}/{vendaId}")]
        public async Task<IActionResult> GetById(int numeroItem, int vendaId)
        {
            var produtoVendido = await _repository.GetByIdAsync(numeroItem, vendaId);
            return produtoVendido == null ? NotFound() : Ok(produtoVendido);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProdutosVendidos produtoVendido)
        {
            var created = await _repository.CreateAsync(produtoVendido);
            // For composite keys, CreatedAtAction might need to be adjusted or
            // you might return Ok with the created object if a GET endpoint for this composite key exists.
            // Assuming GetById can handle the composite key for CreatedAtAction.
            return created ? CreatedAtAction(nameof(GetById), new { numeroItem = produtoVendido.NumeroItem, vendaId = produtoVendido.oVenda.Id }, produtoVendido) : BadRequest();
        }

        [HttpPut("{numeroItem}/{vendaId}")]
        public async Task<IActionResult> Update(int numeroItem, int vendaId, [FromBody] ProdutosVendidos produtoVendido)
        {
            // Ensure the IDs from the route match the object's IDs
            produtoVendido.NumeroItem = numeroItem;
            // Assuming oVenda.Id will be set in the incoming object or needs to be set here as well
            if (produtoVendido.oVenda == null)
            {
                // Handle case where oVenda is not provided in the request body
                return BadRequest("Venda information is required for update.");
            }
            produtoVendido.oVenda.Id = vendaId;

            var updated = await _repository.UpdateAsync(produtoVendido);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{numeroItem}/{vendaId}")]
        public async Task<IActionResult> Delete(int numeroItem, int vendaId)
        {
            var deleted = await _repository.DeleteAsync(numeroItem, vendaId);
            return deleted ? NoContent() : NotFound();
        }
    }
}