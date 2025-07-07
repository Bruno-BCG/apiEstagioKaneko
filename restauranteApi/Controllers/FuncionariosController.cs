// File: FuncionariosController.cs
using Microsoft.AspNetCore.Mvc;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace restauranteApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // This sets the base route, e.g., /api/Funcionarios
    public class FuncionariosController : ControllerBase
    {
        private readonly IFuncionariosRepository _repository;

        public FuncionariosController(IFuncionariosRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retrieves all Employees.
        /// GET /api/Funcionarios
        /// </summary>
        /// <returns>A list of Employees.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var funcionarios = await _repository.GetAllAsync();
            return Ok(funcionarios);
        }

        /// <summary>
        /// Retrieves a specific Employee by ID.
        /// GET /api/Funcionarios/{id}
        /// </summary>
        /// <param name="id">The ID of the Employee to retrieve.</param>
        /// <returns>The Employee object if found, otherwise NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var funcionario = await _repository.GetByIdAsync(id);
            return funcionario == null ? NotFound() : Ok(funcionario);
        }

        /// <summary>
        /// Creates a new Employee.
        /// POST /api/Funcionarios
        /// </summary>
        /// <param name="funcionario">The Funcionarios object to create.</param>
        /// <returns>The created Funcionarios with its assigned ID and a link to its location.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Funcionarios funcionario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = await _repository.CreateAsync(funcionario);
            funcionario.Id = id; // Update the model with the generated ID
            return CreatedAtAction(nameof(GetById), new { id = id }, funcionario);
        }

        /// <summary>
        /// Updates an existing Employee.
        /// PUT /api/Funcionarios/{id}
        /// </summary>
        /// <param name="id">The ID of the Employee to update.</param>
        /// <param name="funcionario">The updated Funcionarios object.</param>
        /// <returns>NoContent if successful, BadRequest if IDs mismatch, NotFound if Employee not found.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Funcionarios funcionario)
        {
            if (id != funcionario.Id)
            {
                return BadRequest("Employee ID in route does not match body.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _repository.UpdateAsync(funcionario);
            return updated ? NoContent() : NotFound();
        }

        /// <summary>
        /// Deletes an Employee by ID.
        /// DELETE /api/Funcionarios/{id}
        /// </summary>
        /// <param name="id">The ID of the Employee to delete.</param>
        /// <returns>NoContent if successful, NotFound if Employee not found.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _repository.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}