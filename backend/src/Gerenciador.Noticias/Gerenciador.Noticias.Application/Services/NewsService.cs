using AutoMapper;
using Gerenciador.Noticias.Application.Dtos;
using Gerenciador.Noticias.Application.Dtos.Filters;
using Gerenciador.Noticias.Application.Dtos.Pagination;
using Gerenciador.Noticias.Application.Services.Cache.Interfaces;
using Gerenciador.Noticias.Application.Services.Interfaces;
using Gerenciador.Noticias.Domain.Entities;
using Gerenciador.Noticias.Domain.Enums;
using Gerenciador.Noticias.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Gerenciador.Noticias.Application.Services;

public class NewsService : INewsService
{
    private readonly IMapper _mapper;
    private readonly IMongoRepository<News> _repository;
    private readonly ICacheService _cacheService;
    private readonly IConfiguration _configuration;
    private readonly string _cacheKeyNewsList;
    private readonly string _cacheKeyNewsByIdPrefix;

    public NewsService(IMongoRepository<News> repository, 
        IMapper mapper,
        ICacheService cacheService,
        IConfiguration configuration)
    {
        _mapper = mapper;
        _repository = repository;
        _cacheService = cacheService;
        _configuration = configuration;
        _cacheKeyNewsList = _configuration.GetSection("AppCache:KeyNewsList").Value ?? "news_list";
        _cacheKeyNewsByIdPrefix = _configuration.GetSection("AppCache:KeyNewsByIdPrefix").Value ?? "news_";
    }

    public async Task<List<NewsDto>> GetNewsListAsync()
    {
        var cachedLsit = await _cacheService.GetAsync<List<NewsDto>>(_cacheKeyNewsList);

        if(cachedLsit != null) return cachedLsit;

        var newsListEntity = await _repository.GetAsync();
        var result = _mapper.Map<List<NewsDto>>(newsListEntity);

        await _cacheService.SetAsync(_cacheKeyNewsList, result, TimeSpan.FromMinutes(60));

        return result;
    }

    public async Task<PaginatedResult<NewsDto>> GetPaginatedNewsListAsync(NewsPaginationFilter filter)
    {
        var cacheKey = $"{_cacheKeyNewsList}_{filter.Page}_{filter.PageSize}_{filter.CategoryId}";
        var cached = await _cacheService.GetAsync<PaginatedResult<NewsDto>>(cacheKey);

        if(cached != null) return cached;

        var builder = Builders<News>.Filter;
        var mongoFilter = builder.Empty;

        var filters = new List<FilterDefinition<News>>();

        if (!string.IsNullOrWhiteSpace(filter.Title))
            filters.Add(builder.Regex(x => x.Title, new MongoDB.Bson.BsonRegularExpression(filter.Title, "i")));

        if (!string.IsNullOrWhiteSpace(filter.Author))
            filters.Add(builder.Regex(x => x.Author, new MongoDB.Bson.BsonRegularExpression(filter.Author, "i")));

        if (!string.IsNullOrWhiteSpace(filter.Status) && 
            Enum.TryParse<Status>(filter.Status, out var parsedStatus))
        {
            filters.Add(builder.Eq(x => x.Status, parsedStatus));
        }

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
            ? Builders<News>.Sort.Ascending(filter.OrderBy ?? "CreatedAt")
            : Builders<News>.Sort.Descending(filter.OrderBy ?? "CreatedAt");

        // Paginação
        var total = await _repository.CountAsync(mongoFilter);

        var items = await _repository.GetPagedAsync(mongoFilter, sort, filter.Page, filter.PageSize);
        var result = new PaginatedResult<NewsDto>
        {
            Items = _mapper.Map<List<NewsDto>>(items.Items),
            TotalItems = (int)total,
            Page = filter.Page,
            PageSize = filter.PageSize
        };

        await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(60));

        return result;
    }

    public async Task<NewsDto> GetNewsByIdAsync(string id)
    {
        var cacheKey = $"{_cacheKeyNewsByIdPrefix}_{id}";
        var cached = await _cacheService.GetAsync<NewsDto>(cacheKey);

        if (cached != null) return cached;

        var newsEntity = await _repository.GetByIdAsync(id);
        var result = _mapper.Map<NewsDto>(newsEntity);

        await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(60));

        return result;
    }

    public async Task<NewsDto> GetNewsBySlugAsync(string slug)
    {
        var cacheKey = $"{_cacheKeyNewsByIdPrefix}_{slug}";
        var cached = await _cacheService.GetAsync<NewsDto>(cacheKey);

        var newsEntity = await _repository.GetByPropertyAsync(e => e.Slug == slug);
        var result = _mapper.Map<NewsDto>(newsEntity);

        await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(60));

        return result;
    }

    public async Task<NewsDto> CreateNewsAsync(NewsDto newsDto)
    {
        var entity = new News(newsDto.Summary, 
            newsDto.Title, 
            newsDto.Text!, 
            newsDto.Author, 
            newsDto.Link!,
            newsDto.CategoryId);
        await _repository.CreateAsync(entity);

        await _cacheService.RemoveAsync(_cacheKeyNewsList);

        return _mapper.Map<NewsDto>(entity);
    }

    public async Task UpdateNewsAsync(string id, NewsDto newsIn)
    {
        await _cacheService.RemoveAsync($"{_cacheKeyNewsByIdPrefix}/{id}");
        await _cacheService.RemoveAsync(_cacheKeyNewsList);

        await _repository.UpdateAsync(id, _mapper.Map<News>(newsIn));
    }

    public async Task RemoveNewsAsync(string id)
    {
        await _cacheService.RemoveAsync($"{_cacheKeyNewsByIdPrefix}/{id}");
        await _cacheService.RemoveAsync(_cacheKeyNewsList);

        await _repository.RemoveAsync(id);
    }
}
