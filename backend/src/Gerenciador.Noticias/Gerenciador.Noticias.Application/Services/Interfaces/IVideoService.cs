using Gerenciador.Noticias.Application.Dtos;

namespace Gerenciador.Noticias.Application.Services.Interfaces;

public interface IVideoService
{
    Task<List<VideoDto>> GetVideoListAsync();
    Task<VideoDto> GetVideoByIdAsync(string id);
    Task<VideoDto> GetVideoBySlugAsync(string slug);
    Task<VideoDto> CreateVideoAsync(VideoDto videoDto);
    Task UpdateVideoAsync(string id, VideoDto videoIn);
    Task RemoveVideoAsync(string id);
}
