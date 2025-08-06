using Gerenciador.Noticias.Application.Dtos;
using Gerenciador.Noticias.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gerenciador.Noticias.Api.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Endpoint que retorna a lista com todas as categorias
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetAllCategory()
        {
            var result = await _categoryService.GetCategoryListAsync();
            return Ok(result);
        }

        /// <summary>
        /// Endpoint que retorna uma categoria a partir do seu id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:length(24)}", Name = "GetCategory")]
        public async Task<ActionResult<CategoryDto>> GetCategoryById(string id)
        {
            var news = await _categoryService.GetCategoryByIdAsync(id);

            if (news is null)
                return NotFound();

            return news;
        }

        /// <summary>
        /// Endpoint para criar uma categoria
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<ActionResult<CategoryDto>> CreateCategory(CategoryDto news)
        {
            var result = await _categoryService.CreateCategoryAsync(news);

            return CreatedAtRoute("GetCategory", new { id = result.Id!.ToString() }, result);
        }

        /// <summary>
        /// Endpoint para atualizar uma categoria
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newsIn"></param>
        /// <returns></returns>
        [HttpPut("update/{id:length(24)}")]
        public async Task<ActionResult<CategoryDto>> UpdateCategory(string id, CategoryDto newsIn)
        {
            var news = await _categoryService.GetCategoryByIdAsync(id);

            if (news is null)
                return NotFound();

            await _categoryService.UpdateCategoryAsync(id, newsIn);

            return CreatedAtRoute("GetCategory", new { id }, newsIn);

        }

        /// <summary>
        /// Endpoint para apagar uma categoria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id:length(24)}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            var news = await _categoryService.GetCategoryByIdAsync(id);

            if (news is null)
                return NotFound();

            await _categoryService.RemoveCategoryAsync(news.Id!);

            return Ok("Noticia deletada com sucesso!");
        }
    }
}
