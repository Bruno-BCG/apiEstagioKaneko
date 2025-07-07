using Microsoft.AspNetCore.Mvc;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;

namespace restauranteApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoRepository _repository;

        public ProdutosController(IProdutoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var produtos = await _repository.GetAllAsync();
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var produto = await _repository.GetByIdAsync(id);
            return produto == null ? NotFound() : Ok(produto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Produtos produto)
        {
            var id = await _repository.CreateAsync(produto);
            produto.Id = id;
            return CreatedAtAction(nameof(GetById), new { id = id }, produto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Produtos produto)
        {
            produto.Id = id;
            var updated = await _repository.UpdateAsync(produto);
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
