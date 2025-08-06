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

        public FilesController(IWebHostEnvironment environment)
        {
            _environment = environment;
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

            // Exemplo: atualizar no banco de dados
            //var newsDto = await _newsService.GetNewsByIdAsync(id);
            //if (newsDto == null) return NotFound();
            //
            //newsDto.Image = imageUrl;
            //
            //await _newsService.UpdateNewsAsync(id, newsDto);

            return Ok(new
            {
                message = "Imagem enviada com sucesso.",
                imageUrl
            });
        }
    }
}
