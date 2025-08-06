using Gerenciador.Noticias.Application.Dtos;
using Gerenciador.Noticias.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gerenciador.Noticias.Api.Controllers
{
    [Route("api/videos")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly IVideoService _videoService;

        public VideosController(IVideoService videoService)
        {
            _videoService = videoService;
        }

        /// <summary>
        /// Endpoint que retorna a lista com todas as notícias
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetAllVideo()
        {
            var result = await _videoService.GetVideoListAsync();
            return Ok(result);
        }

        /// <summary>
        /// Endpoint que retorna um video a partir do seu id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:length(24)}", Name = "GetVideo")]
        public async Task<ActionResult<VideoDto>> GetVideoById(string id)
        {
            var video = await _videoService.GetVideoByIdAsync(id);

            if (video is null)
                return NotFound();

            return video;
        }

        /// <summary>
        /// Endpoint que retorna um video a partir do seu slug
        /// </summary>
        /// <param name="slug"></param>
        /// <returns></returns>
        [HttpGet("{slug}")]
        public async Task<ActionResult<VideoDto>> GetVideoBySlug(string slug)
        {
            var video = await _videoService.GetVideoBySlugAsync(slug);

            if (video is null)
                return NotFound();

            return video;
        }

        /// <summary>
        /// Endpoint para criar um video
        /// </summary>
        /// <param name="video"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<ActionResult<VideoDto>> CreateVideo(VideoDto video)
        {
            var result = await _videoService.CreateVideoAsync(video);

            return CreatedAtRoute("GetVideo", new { id = result.Id!.ToString() }, result);
        }

        /// <summary>
        /// Endpoint para atualizar um video
        /// </summary>
        /// <param name="id"></param>
        /// <param name="videoIn"></param>
        /// <returns></returns>
        [HttpPut("update/{id:length(24)}")]
        public async Task<ActionResult<VideoDto>> UpdateVideo(string id, VideoDto videoIn)
        {
            var video = await _videoService.GetVideoByIdAsync(id);

            if (video is null)
                return NotFound();

            await _videoService.UpdateVideoAsync(id, videoIn);

            return CreatedAtRoute("GetVideo", new { id }, videoIn);

        }

        /// <summary>
        /// Endpoint para apagar um video
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id:length(24)}")]
        public async Task<IActionResult> DeleteVideo(string id)
        {
            var video = await _videoService.GetVideoByIdAsync(id);

            if (video is null)
                return NotFound();

            await _videoService.RemoveVideoAsync(video.Id!);

            return Ok("Video deletado com sucesso!");
        }
    }
}
