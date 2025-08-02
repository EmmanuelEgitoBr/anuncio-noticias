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

        [HttpGet]
        public async Task<ActionResult> GetAllNews()
        {
            var result = await _newsService.GetNewsListAsync();
            return Ok(result);
        }
    }
}
