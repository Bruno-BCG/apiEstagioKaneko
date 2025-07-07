using Microsoft.AspNetCore.Mvc;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic; // Required for IEnumerable
using System.Threading.Tasks;

namespace restauranteApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // This sets the base route, e.g., /api/Estados
    public class EstadosController : ControllerBase
    {
        private readonly IEstadosRepository _repository;

        public EstadosController(IEstadosRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retrieves all States.
        /// GET /api/Estados
        /// </summary>
        /// <returns>A list of States.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var estados = await _repository.GetAllAsync();
            return Ok(estados);
        }

        /// <summary>
        /// Retrieves a specific State by ID.
        /// GET /api/Estados/{id}
        /// </summary>
        /// <param name="id">The ID of the State to retrieve.</param>
        /// <returns>The State object if found, otherwise NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var estado = await _repository.GetByIdAsync(id);
            return estado == null ? NotFound() : Ok(estado);
        }

        /// <summary>
        /// Creates a new State.
        /// POST /api/Estados
        /// </summary>
        /// <param name="estado">The State object to create.</param>
        /// <returns>The created State with its assigned ID and a link to its location.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Estados estado)
        {
            // Basic validation check; more complex validation can be done with DataAnnotations
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = await _repository.CreateAsync(estado);
            estado.Id = id; // Update the model with the generated ID
            return CreatedAtAction(nameof(GetById), new { id = id }, estado);
        }

        /// <summary>
        /// Updates an existing State.
        /// PUT /api/Estados/{id}
        /// </summary>
        /// <param name="id">The ID of the State to update.</param>
        /// <param name="estado">The updated State object.</param>
        /// <returns>NoContent if successful, BadRequest if IDs mismatch, NotFound if State not found.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Estados estado)
        {
            if (id != estado.Id)
            {
                return BadRequest("State ID in route does not match body.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _repository.UpdateAsync(estado);
            return updated ? NoContent() : NotFound();
        }

        /// <summary>
        /// Deletes a State by ID.
        /// DELETE /api/Estados/{id}
        /// </summary>
        /// <param name="id">The ID of the State to delete.</param>
        /// <returns>NoContent if successful, NotFound if State not found.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _repository.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}