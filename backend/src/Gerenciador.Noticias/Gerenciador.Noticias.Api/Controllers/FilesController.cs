using Gerenciador.Noticias.Application.Services.Interfaces;
using Gerenciador.Noticias.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace Gerenciador.Noticias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly INewsService _newsService;

        public FilesController(IWebHostEnvironment environment, INewsService newsService)
        {
            _environment = environment;
            _newsService = newsService;
        }

        /// <summary>
        /// Endpoint para upload de mídia para uma notícia
        /// </summary>
        /// <param name="id"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("{id}/upload-media")]
        public async Task<IActionResult> UploadMedia(string id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Nenhum arquivo enviado.");

            var newsDto = await _newsService.GetNewsByIdAsync(id);
            if (newsDto == null) return NotFound();

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

            if (!allowedExtensions.Contains(extension))
                return BadRequest("Extensão de arquivo inválida.");

            // Nome único para o arquivo
            var webpFileName = $"{Guid.NewGuid()}.webp";
            var imagePath = Path.Combine(_environment.WebRootPath, "images");

            if (!Directory.Exists(imagePath))
                Directory.CreateDirectory(imagePath);

            var webpFilePath = Path.Combine(imagePath, webpFileName);

            using (var image = await Image.LoadAsync(file.OpenReadStream()))
            {
                image.Mutate(x => x.AutoOrient()); // Corrige rotação
                await image.SaveAsync(webpFilePath, new WebpEncoder
                {
                    Quality = 75 // Ajuste de compressão (0 a 100)
                });
            }

            var imageUrl = $"{Request.Scheme}://{Request.Host}/images/{webpFileName}";
            var media = new Media(imageUrl, Domain.Enums.MediaType.Image);
            
            newsDto.Medias!.Add(media);
            await _newsService.UpdateNewsAsync(id, newsDto);

            return Ok(new
            {
                message = "Imagem enviada com sucesso.",
                imageUrl
            });
        }

        /// <summary>
        /// Endpoint para upload de vídeo para uma notícia
        /// </summary>
        /// <param name="id"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("{id}/upload-video")]
        public IActionResult UploadVideo(string id, IFormFile file)
        {
            return Ok();
        }
    }
}
