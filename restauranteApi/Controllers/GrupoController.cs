// File: GrupoController.cs
using Microsoft.AspNetCore.Mvc;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace restauranteApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // This sets the base route, e.g., /api/Grupo
    public class GrupoController : ControllerBase
    {
        private readonly IGrupoRepository _repository;

        public GrupoController(IGrupoRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retrieves all Groups.
        /// GET /api/Grupo
        /// </summary>
        /// <returns>A list of Groups.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var grupos = await _repository.GetAllAsync();
            return Ok(grupos);
        }

        /// <summary>
        /// Retrieves a specific Group by ID.
        /// GET /api/Grupo/{id}
        /// </summary>
        /// <param name="id">The ID of the Group to retrieve.</param>
        /// <returns>The Group object if found, otherwise NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var grupo = await _repository.GetByIdAsync(id);
            return grupo == null ? NotFound() : Ok(grupo);
        }

        /// <summary>
        /// Creates a new Group.
        /// POST /api/Grupo
        /// </summary>
        /// <param name="grupo">The Grupo object to create.</param>
        /// <returns>The created Grupo with its assigned ID and a link to its location.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Grupos grupo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = await _repository.CreateAsync(grupo);
            grupo.Id = id; // Update the model with the generated ID
            return CreatedAtAction(nameof(GetById), new { id = id }, grupo);
        }

        /// <summary>
        /// Updates an existing Group.
        /// PUT /api/Grupo/{id}
        /// </summary>
        /// <param name="id">The ID of the Group to update.</param>
        /// <param name="grupo">The updated Grupo object.</param>
        /// <returns>NoContent if successful, BadRequest if IDs mismatch, NotFound if Group not found.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Grupos grupo)
        {
            if (id != grupo.Id)
            {
                return BadRequest("Group ID in route does not match body.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _repository.UpdateAsync(grupo);
            return updated ? NoContent() : NotFound();
        }

        /// <summary>
        /// Deletes a Group by ID.
        /// DELETE /api/Grupo/{id}
        /// </summary>
        /// <param name="id">The ID of the Group to delete.</param>
        /// <returns>NoContent if successful, NotFound if Group not found.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _repository.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}