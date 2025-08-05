using Gerenciador.Noticias.Application.Dtos;
using Gerenciador.Noticias.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gerenciador.Noticias.Api.Controllers
{
    [Route("api/news")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        private readonly IWebHostEnvironment _environment;

        public NewsController(INewsService newsService, IWebHostEnvironment environment)
        {
            _newsService = newsService;
            _environment = environment;
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

        /// <summary>
        /// Endpoint para upload de imagem para uma notícia
        /// </summary>
        /// <param name="id"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("{id}/upload-image")]
        public async Task<IActionResult> UploadImage(string id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Nenhum arquivo enviado.");

            // Apenas extensões válidas
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                return BadRequest("Extensão de arquivo inválida.");

            // Nome único para o arquivo
            var fileName = $"{Guid.NewGuid()}{extension}";
            var imagePath = Path.Combine(_environment.WebRootPath, "images");

            if (!Directory.Exists(imagePath))
                Directory.CreateDirectory(imagePath);

            var filePath = Path.Combine(imagePath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var imageUrl = $"{Request.Scheme}://{Request.Host}/images/{fileName}";

            // Exemplo: atualizar no banco de dados
            var newsDto = await _newsService.GetNewsByIdAsync(id);
            if (newsDto == null) return NotFound();
            
            newsDto.Image = imageUrl;
            newsDto.Link = imageUrl;

            await _newsService.UpdateNewsAsync(id, newsDto);

            return Ok(new
            {
                message = "Imagem enviada com sucesso.",
                imageUrl
            });
        }
    }
}
