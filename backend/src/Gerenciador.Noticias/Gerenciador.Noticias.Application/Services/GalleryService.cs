using AutoMapper;
using Gerenciador.Noticias.Application.Dtos;
using Gerenciador.Noticias.Application.Dtos.Filters;
using Gerenciador.Noticias.Application.Dtos.Pagination;
using Gerenciador.Noticias.Application.Services.Interfaces;
using Gerenciador.Noticias.Domain.Entities;
using Gerenciador.Noticias.Domain.Interfaces;
using MongoDB.Driver;

namespace Gerenciador.Noticias.Application.Services;

public class GalleryService : IGalleryService
{
    private readonly IMapper _mapper;
    private readonly IMongoRepository<Gallery> _galleryRepository;
    private readonly IMongoRepository<News> _newsRepository;

    public GalleryService(IMongoRepository<Gallery> galleryRepository, 
        IMapper mapper,
        IMongoRepository<News> newsRepository)
    {
        _mapper = mapper;
        _galleryRepository = galleryRepository;
        _newsRepository = newsRepository;
    }

    public async Task<List<GalleryDto>> GetGalleryListAsync()
    {
        var galleryListEntity = await _galleryRepository.GetAsync();
        return _mapper.Map<List<GalleryDto>>(galleryListEntity);
    }

    public async Task<PaginatedResult<GalleryDto>> GetPaginatedGalleryListAsync(GalleryPaginationFilter filter)
    {
        var builder = Builders<Gallery>.Filter;
        var mongoFilter = builder.Empty;

        var filters = new List<FilterDefinition<Gallery>>();

        if (filter.CategoryId > 0)
            filters.Add(builder.Eq(x => x.CategoryId, filter.CategoryId));

        if (filter.DateFrom.HasValue)
            filters.Add(builder.Gte(x => x.CreatedAt, filter.DateFrom.Value));

        if (filter.DateTo.HasValue)
            filters.Add(builder.Lte(x => x.CreatedAt, filter.DateTo.Value));

        if (filters.Any())
            mongoFilter = builder.And(filters);

        // Monta sort dinâmico
        var sort = filter.OrderDirection?.ToLower() == "asc"
            ? Builders<Gallery>.Sort.Ascending(filter.OrderBy ?? "CreatedAt")
            : Builders<Gallery>.Sort.Descending(filter.OrderBy ?? "CreatedAt");

        // Paginação
        var total = await _galleryRepository.CountAsync(mongoFilter);

        var items = await _galleryRepository.GetPagedAsync(mongoFilter, sort, filter.Page, filter.PageSize);

        return new PaginatedResult<GalleryDto>
        {
            Items = _mapper.Map<List<GalleryDto>>(items.Items),
            TotalItems = (int)total,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }

    public async Task<GalleryDto> GetGalleryByIdAsync(string id)
    {
        var galleryEntity = await _galleryRepository.GetByIdAsync(id);
        return _mapper.Map<GalleryDto>(galleryEntity);
    }

    public async Task<GalleryDto> CreateGalleryAsync(GalleryDto galleryDto)
    {
        var entity = new Gallery(galleryDto.GalleryName, galleryDto.CategoryId);
        await _galleryRepository.CreateAsync(entity);

        return _mapper.Map<GalleryDto>(entity);
    }

    public async Task<GalleryDto> AddNewsToGalleryAsync(string galleryId, string newsId)
    {
        var gallery = await _galleryRepository.GetByIdAsync(galleryId);
        var news = await _newsRepository.GetByIdAsync(newsId);

        gallery!.News!.Add(news!);
        await _galleryRepository.UpdateAsync(galleryId, gallery);

        return _mapper.Map<GalleryDto>(gallery);
    }

    public async Task UpdateGalleryAsync(string id, GalleryDto galleryIn)
    {
        await _galleryRepository.UpdateAsync(id, _mapper.Map<Gallery>(galleryIn));
    }

    public async Task RemoveGalleryAsync(string id) => await _galleryRepository.RemoveAsync(id);
}
