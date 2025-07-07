// File: MesasController.cs
using Microsoft.AspNetCore.Mvc;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace restauranteApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // e.g., /api/Mesas
    public class MesasController : ControllerBase
    {
        private readonly IMesasRepository _repository;

        public MesasController(IMesasRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retrieves all Tables.
        /// GET /api/Mesas
        /// </summary>
        /// <returns>A list of Tables.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var mesas = await _repository.GetAllAsync();
            return Ok(mesas);
        }

        /// <summary>
        /// Retrieves a specific Table by its number.
        /// GET /api/Mesas/{numeroMesa}
        /// </summary>
        /// <param name="numeroMesa">The number of the Table to retrieve.</param>
        /// <returns>The Mesas object if found, otherwise NotFound.</returns>
        [HttpGet("{numeroMesa}")]
        public async Task<IActionResult> GetByNumeroMesa(int numeroMesa)
        {
            var mesa = await _repository.GetByIdAsync(numeroMesa);
            return mesa == null ? NotFound() : Ok(mesa);
        }

        /// <summary>
        /// Creates a new Table.
        /// POST /api/Mesas
        /// </summary>
        /// <param name="mesa">The Mesas object to create.</param>
        /// <returns>NoContent if successful (since PK is not identity), BadRequest if validation fails, or Conflict if key already exists.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Mesas mesa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if a mesa with this NumeroMesa already exists
            var existingMesa = await _repository.GetByIdAsync(mesa.NumeroMesa);
            if (existingMesa != null)
            {
                return Conflict($"Table with number {mesa.NumeroMesa} already exists.");
            }

            var created = await _repository.CreateAsync(mesa);
            return created ? CreatedAtAction(nameof(GetByNumeroMesa), new { numeroMesa = mesa.NumeroMesa }, mesa) : BadRequest("Could not create the table.");
        }

        /// <summary>
        /// Updates an existing Table.
        /// PUT /api/Mesas/{numeroMesa}
        /// </summary>
        /// <param name="numeroMesa">The number of the Table to update.</param>
        /// <param name="mesa">The updated Mesas object.</param>
        /// <returns>NoContent if successful, BadRequest if IDs mismatch, NotFound if Table not found.</returns>
        [HttpPut("{numeroMesa}")]
        public async Task<IActionResult> Update(int numeroMesa, [FromBody] Mesas mesa)
        {
            if (numeroMesa != mesa.NumeroMesa)
            {
                return BadRequest("Table number in route does not match body.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _repository.UpdateAsync(mesa);
            return updated ? NoContent() : NotFound();
        }

        /// <summary>
        /// Deletes a Table by its number.
        /// DELETE /api/Mesas/{numeroMesa}
        /// </summary>
        /// <param name="numeroMesa">The number of the Table to delete.</param>
        /// <returns>NoContent if successful, NotFound if Table not found.</returns>
        [HttpDelete("{numeroMesa}")]
        public async Task<IActionResult> Delete(int numeroMesa)
        {
            var deleted = await _repository.DeleteAsync(numeroMesa);
            return deleted ? NoContent() : NotFound();
        }
    }
}