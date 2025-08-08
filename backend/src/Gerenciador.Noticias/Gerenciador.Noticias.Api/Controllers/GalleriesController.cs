using Gerenciador.Noticias.Application.Dtos.Filters;
using Gerenciador.Noticias.Application.Dtos;
using Gerenciador.Noticias.Application.Services;
using Gerenciador.Noticias.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gerenciador.Noticias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GalleriesController : ControllerBase
    {
        private readonly IGalleryService _galleryService;

        public GalleriesController(IGalleryService galleryService)
        {
            _galleryService = galleryService;
        }


        /// <summary>
        /// Endpoint que retorna a lista com todas as galerias
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetAllGallery()
        {
            var result = await _galleryService.GetGalleryListAsync();
            return Ok(result);
        }

        /// <summary>
        /// Endpoint que retorna a lista paginada com todas as galerias
        /// </summary>
        /// <returns></returns>
        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedGallery([FromQuery] GalleryPaginationFilter filter)
        {
            var result = await _galleryService.GetPaginatedGalleryListAsync(filter);
            return Ok(result);
        }

        /// <summary>
        /// Endpoint que retorna uma galeria a partir do seu id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:length(24)}", Name = "GetGallery")]
        public async Task<ActionResult<GalleryDto>> GetGalleryById(string id)
        {
            var news = await _galleryService.GetGalleryByIdAsync(id);

            if (news is null)
                return NotFound();

            return news;
        }

        /// <summary>
        /// Endpoint para criar uma galeria
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<ActionResult<GalleryDto>> CreateGallery(GalleryDto news)
        {
            var result = await _galleryService.CreateGalleryAsync(news);

            return CreatedAtRoute("GetGallery", new { id = result.Id!.ToString() }, result);
        }

        /// <summary>
        /// Endpoint para criar uma galeria
        /// </summary>
        /// <param name="galleryId"></param>
        /// <param name="newsId"></param>
        /// <returns></returns>
        [HttpPut("add-news")]
        public async Task<ActionResult<GalleryDto>> AddNews([FromQuery] string galleryId, 
            [FromQuery] string newsId)
        {
            var result = await _galleryService.AddNewsToGalleryAsync(galleryId, newsId);

            return CreatedAtRoute("GetGallery", new { id = result.Id!.ToString() }, result);
        }

        /// <summary>
        /// Endpoint para atualizar uma galeria
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newsIn"></param>
        /// <returns></returns>
        [HttpPut("update/{id:length(24)}")]
        public async Task<ActionResult<GalleryDto>> UpdateGallery(string id, GalleryDto newsIn)
        {
            var news = await _galleryService.GetGalleryByIdAsync(id);

            if (news is null)
                return NotFound();

            await _galleryService.UpdateGalleryAsync(id, newsIn);

            return CreatedAtRoute("GetGallery", new { id }, newsIn);

        }

        /// <summary>
        /// Endpoint para apagar uma galeria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id:length(24)}")]
        public async Task<IActionResult> DeleteGallery(string id)
        {
            var news = await _galleryService.GetGalleryByIdAsync(id);

            if (news is null)
                return NotFound();

            await _galleryService.RemoveGalleryAsync(news.Id!);

            return Ok("Noticia deletada com sucesso!");
        }
    }
}
