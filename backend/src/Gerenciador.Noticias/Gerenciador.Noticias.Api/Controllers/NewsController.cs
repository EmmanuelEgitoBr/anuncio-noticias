using Gerenciador.Noticias.Application.Dtos;
using Gerenciador.Noticias.Application.Dtos.Filters;
using Gerenciador.Noticias.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gerenciador.Noticias.Api.Controllers
{
    [Route("api/news")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        /// <summary>
        /// Endpoint que retorna a lista com todas as notícias
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetAllNews()
        {
            var result = await _newsService.GetNewsListAsync();
            return Ok(result);
        }

        /// <summary>
        /// Endpoint que retorna a lista com todas as notícias
        /// </summary>
        /// <returns></returns>
        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedNews([FromQuery] NewsPaginationFilter filter)
        {
            var result = await _newsService.GetPaginatedNewsListAsync(filter);
            return Ok(result);
        }

        /// <summary>
        /// Endpoint que retorna uma notícia a partir do seu id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:length(24)}", Name = "GetNews")]
        public async Task<ActionResult<NewsDto>> GetNewsById(string id)
        {
            var news = await _newsService.GetNewsByIdAsync(id);

            if (news is null)
                return NotFound();

            return news;
        }

        /// <summary>
        /// Endpoint que retorna uma notícia a partir do seu slug
        /// </summary>
        /// <param name="slug"></param>
        /// <returns></returns>
        [HttpGet("{slug}")]
        public async Task<ActionResult<NewsDto>> GetNewsBySlug(string slug)
        {
            var news = await _newsService.GetNewsBySlugAsync(slug);

            if (news is null)
                return NotFound();

            return news;
        }

        /// <summary>
        /// Endpoint para criar uma notícia
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<ActionResult<NewsDto>> CreateNews(NewsDto news)
        {
            var result = await _newsService.CreateNewsAsync(news);

            return CreatedAtRoute("GetNews", new { id = result.Id!.ToString() }, result);
        }

        /// <summary>
        /// Endpoint para atualizar uma notícia
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newsIn"></param>
        /// <returns></returns>
        [HttpPut("update/{id:length(24)}")]
        public async Task<ActionResult<NewsDto>> UpdateNews(string id, NewsDto newsIn)
        {
            var news = await _newsService.GetNewsByIdAsync(id);

            if (news is null)
                return NotFound();

            await _newsService.UpdateNewsAsync(id, newsIn);

            return CreatedAtRoute("GetNews", new { id }, newsIn);

        }

        /// <summary>
        /// Endpoint para apagar uma notícia
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id:length(24)}")]
        public async Task<IActionResult> DeleteNews(string id)
        {
            var news = await _newsService.GetNewsByIdAsync(id);

            if (news is null)
                return NotFound();

            await _newsService.RemoveNewsAsync(news.Id!);

            return Ok("Noticia deletada com sucesso!");
        }
    }
}
