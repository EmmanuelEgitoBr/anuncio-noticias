using Gerenciador.Noticias.Application.Dtos;
using Gerenciador.Noticias.Application.Dtos.Filters;
using Gerenciador.Noticias.Application.Dtos.Pagination;

namespace Gerenciador.Noticias.Application.Services.Interfaces;

public interface INewsService
{
    Task<List<NewsDto>> GetNewsListAsync();
    Task<PaginatedResult<NewsDto>> GetPaginatedNewsListAsync(NewsPaginationFilter filter);
    Task<NewsDto> GetNewsByIdAsync(string id);
    Task<NewsDto> GetNewsBySlugAsync(string slug);
    Task<NewsDto> CreateNewsAsync(NewsDto newsDto);
    Task UpdateNewsAsync(string id, NewsDto newsIn);
    Task RemoveNewsAsync(string id);
}
