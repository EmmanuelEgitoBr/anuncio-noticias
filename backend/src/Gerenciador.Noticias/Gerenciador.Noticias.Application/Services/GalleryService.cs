using AutoMapper;
using Gerenciador.Noticias.Application.Dtos;
using Gerenciador.Noticias.Application.Dtos.Filters;
using Gerenciador.Noticias.Application.Dtos.Pagination;
using Gerenciador.Noticias.Application.Services.Cache.Interfaces;
using Gerenciador.Noticias.Application.Services.Interfaces;
using Gerenciador.Noticias.Domain.Entities;
using Gerenciador.Noticias.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Gerenciador.Noticias.Application.Services;

public class GalleryService : IGalleryService
{
    private readonly IMapper _mapper;
    private readonly IMongoRepository<Gallery> _galleryRepository;
    private readonly IMongoRepository<News> _newsRepository;
    private readonly ICacheService _cacheService;
    private readonly IConfiguration _configuration;
    private readonly string cacheKeyGalleryList;
    private readonly string cacheKeyGalleryByIdPrefix;

    public GalleryService(IMongoRepository<Gallery> galleryRepository, 
        IMapper mapper,
        IMongoRepository<News> newsRepository,
        ICacheService cacheService,
        IConfiguration configuration)
    {
        _mapper = mapper;
        _galleryRepository = galleryRepository;
        _newsRepository = newsRepository;
        _cacheService = cacheService;
        _configuration = configuration;
        cacheKeyGalleryList = _configuration.GetSection("AppCache:KeyGalleryList").Value ?? "gallery_list";
        cacheKeyGalleryByIdPrefix = _configuration.GetSection("AppCache:KeyGalleryById").Value ?? "gallery_";
    }

    public async Task<List<GalleryDto>> GetGalleryListAsync()
    {
        var cached = await _cacheService.GetAsync<List<GalleryDto>>(cacheKeyGalleryList);

        if (cached != null) return cached;

        var galleryListEntity = await _galleryRepository.GetAsync();

        await _cacheService.SetAsync(cacheKeyGalleryList, galleryListEntity, TimeSpan.FromMinutes(5));

        return _mapper.Map<List<GalleryDto>>(galleryListEntity);
    }

    public async Task<PaginatedResult<GalleryDto>> GetPaginatedGalleryListAsync(GalleryPaginationFilter filter)
    {
        var cacheKey = $"{cacheKeyGalleryList}_page_{filter.Page}_size_{filter.PageSize}_cat_{filter.CategoryId}";

        var cached = await _cacheService.GetAsync<PaginatedResult<GalleryDto>>(cacheKey);

        if (cached != null) return cached;

        var builder = Builders<Gallery>.Filter;
        var mongoFilter = builder.Empty;

        var filters = new List<FilterDefinition<Gallery>>();

        if (!String.IsNullOrEmpty(filter.CategoryId))
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

        var result = new PaginatedResult<GalleryDto>
        {
            Items = _mapper.Map<List<GalleryDto>>(items.Items),
            TotalItems = (int)total,
            Page = filter.Page,
            PageSize = filter.PageSize
        };

        await _cacheService.SetAsync<PaginatedResult<GalleryDto>>(cacheKey, result);

        return result;
    }

    public async Task<GalleryDto> GetGalleryByIdAsync(string id)
    {
        var cacheKey = $"{cacheKeyGalleryByIdPrefix}/{id}";
        var cached = await _cacheService.GetAsync<GalleryDto>(cacheKey);

        if (cached != null) return cached;

        var galleryEntity = await _galleryRepository.GetByIdAsync(id);
        var galleryDto = _mapper.Map<GalleryDto>(galleryEntity);

        if (galleryEntity != null)
        {
            await _cacheService.SetAsync<GalleryDto>(cacheKey, galleryDto);
        }

        return galleryDto;
    }

    public async Task<GalleryDto> CreateGalleryAsync(GalleryDto galleryDto)
    {
        Gallery entity = new Gallery(galleryDto.GalleryName, galleryDto.CategoryId, galleryDto.News);
        
        await _galleryRepository.CreateAsync(entity);

        galleryDto.Id= entity.Id;
        galleryDto.CreatedAt = entity.CreatedAt;

        await _cacheService.RemoveAsync(cacheKeyGalleryList);

        return galleryDto;
    }

    public async Task<GalleryDto> AddNewsToGalleryAsync(string galleryId, string newsId)
    {
        var gallery = await _galleryRepository.GetByIdAsync(galleryId);
        var news = await _newsRepository.GetByIdAsync(newsId);

        gallery!.News!.Add(news!);
        await _galleryRepository.UpdateAsync(galleryId, gallery);

        await _cacheService.RemoveAsync($"{cacheKeyGalleryByIdPrefix}/{galleryId}");
        await _cacheService.RemoveAsync(cacheKeyGalleryList);

        return _mapper.Map<GalleryDto>(gallery);
    }

    public async Task UpdateGalleryAsync(string id, GalleryDto galleryIn)
    {
        await _galleryRepository.UpdateAsync(id, _mapper.Map<Gallery>(galleryIn));

        await _cacheService.RemoveAsync($"{cacheKeyGalleryByIdPrefix}/{id}");
        await _cacheService.RemoveAsync(cacheKeyGalleryList);
    }

    public async Task RemoveGalleryAsync(string id)
    {

        await _galleryRepository.RemoveAsync(id);

        await _cacheService.RemoveAsync($"{cacheKeyGalleryByIdPrefix}/{id}");
        await _cacheService.RemoveAsync(cacheKeyGalleryList);
    }
}
