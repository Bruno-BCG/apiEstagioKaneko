using Microsoft.AspNetCore.Mvc;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic; // Added for IEnumerable
using System.Threading.Tasks;

namespace restauranteApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Route will be /api/Compras
    public class ComprasController : ControllerBase
    {
        private readonly IComprasRepository _repository;

        public ComprasController(IComprasRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var compras = await _repository.GetAllAsync();
            return Ok(compras);
        }

        // GET by composite key
        // Example: GET /api/Compras?modelo=ABC&serie=001&numero=123&fornecedorId=5
        [HttpGet("{modelo}/{serie}/{numero}/{fornecedorId}")]
        public async Task<IActionResult> GetById(char modelo, char serie, int numero, int fornecedorId)
        {
            var compra = await _repository.GetByIdAsync(modelo, serie, numero, fornecedorId);
            return compra == null ? NotFound() : Ok(compra);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Compras compra)
        {
            // Note: Compras CreateAsync returns bool, not an ID, as it uses a composite key.
            // The client is expected to provide the full composite key in the body.
            // You might want to add validation here to ensure the composite key is present.
            var created = await _repository.CreateAsync(compra);

            if (created)
            {
                // For composite keys, CreatedAtAction is trickier.
                // You need to pass all key components to the anonymous object.
                return CreatedAtAction(
                    nameof(GetById),
                    new
                    {
                        modelo = compra.Modelo,
                        serie = compra.Serie,
                        numero = compra.Numero,
                        fornecedorId = compra.oFornecedor?.Id // Assuming oFornecedor is populated or Id is directly available
                    },
                    compra
                );
            }
            return BadRequest("Could not create the purchase. It might already exist or data is invalid.");
        }

        // PUT by composite key
        // Example: PUT /api/Compras/ABC/001/123/5
        [HttpPut("{modelo}/{serie}/{numero}/{fornecedorId}")]
        public async Task<IActionResult> Update(char modelo, char serie, int numero, int fornecedorId, [FromBody] Compras compra)
        {
            // Ensure route parameters match body for composite key
            if (modelo != compra.Modelo || serie != compra.Serie || numero != compra.Numero || fornecedorId != compra.oFornecedor?.Id)
            {
                return BadRequest("Composite key parameters in route do not match body.");
            }

            var updated = await _repository.UpdateAsync(compra);
            return updated ? NoContent() : NotFound();
        }

        // DELETE by composite key
        // Example: DELETE /api/Compras/ABC/001/123/5
        [HttpDelete("{modelo}/{serie}/{numero}/{fornecedorId}")]
        public async Task<IActionResult> Delete(char modelo, char serie, int numero, int fornecedorId)
        {
            var deleted = await _repository.DeleteAsync(modelo, serie, numero, fornecedorId);
            return deleted ? NoContent() : NotFound();
        }
    }
}