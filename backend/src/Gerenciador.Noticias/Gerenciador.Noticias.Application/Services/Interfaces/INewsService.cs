using Gerenciador.Noticias.Application.Dtos;

namespace Gerenciador.Noticias.Application.Services.Interfaces;

public interface INewsService
{
    Task<List<NewsDto>> GetNewsListAsync();
    Task<NewsDto> GetNewsByIdAsync(string id);
    Task<NewsDto> CreateNewsAsync(NewsDto newsDto);
    Task UpdateNewsAsync(string id, NewsDto newsIn);
    Task RemoveNewsAsync(string id);
}
