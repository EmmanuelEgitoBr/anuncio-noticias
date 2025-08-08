using Gerenciador.Noticias.Application.Dtos;
using Gerenciador.Noticias.Application.Dtos.Filters;
using Gerenciador.Noticias.Application.Dtos.Pagination;

namespace Gerenciador.Noticias.Application.Services.Interfaces;

public interface IGalleryService
{
    Task<List<GalleryDto>> GetGalleryListAsync();
    Task<PaginatedResult<GalleryDto>> GetPaginatedGalleryListAsync(GalleryPaginationFilter filter);
    Task<GalleryDto> GetGalleryByIdAsync(string id);
    Task<GalleryDto> CreateGalleryAsync(GalleryDto galleryDto);
    Task<GalleryDto> AddNewsToGalleryAsync(string galleryId, string newsId);
    Task UpdateGalleryAsync(string id, GalleryDto galleryIn);
    Task RemoveGalleryAsync(string id);
}
