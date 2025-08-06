using AutoMapper;
using Gerenciador.Noticias.Application.Dtos;
using Gerenciador.Noticias.Application.Dtos.Filters;
using Gerenciador.Noticias.Application.Dtos.Pagination;
using Gerenciador.Noticias.Application.Services.Interfaces;
using Gerenciador.Noticias.Domain.Entities;
using Gerenciador.Noticias.Domain.Enums;
using Gerenciador.Noticias.Domain.Interfaces;
using MongoDB.Driver;

namespace Gerenciador.Noticias.Application.Services;

public class NewsService : INewsService
{
    private readonly IMapper _mapper;
    private readonly IMongoRepository<News> _repository;

    public NewsService(IMongoRepository<News> repository, IMapper mapper)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<List<NewsDto>> GetNewsListAsync()
    {
        var newsListEntity = await _repository.GetAsync();
        return _mapper.Map<List<NewsDto>>(newsListEntity);
    }

    public async Task<PaginatedResult<NewsDto>> GetPaginatedNewsListAsync(NewsPaginationFilter filter)
    {
        var builder = Builders<News>.Filter;
        var mongoFilter = builder.Empty;

        var filters = new List<FilterDefinition<News>>();

        if (!string.IsNullOrWhiteSpace(filter.Title))
            filters.Add(builder.Regex(x => x.Title, new MongoDB.Bson.BsonRegularExpression(filter.Title, "i")));

        if (!string.IsNullOrWhiteSpace(filter.Author))
            filters.Add(builder.Regex(x => x.Author, new MongoDB.Bson.BsonRegularExpression(filter.Author, "i")));

        //if (!string.IsNullOrWhiteSpace(filter.Status))
        //    filters.Add(builder.Eq(x => x.Status.ToString(), filter.Status));

        if (!string.IsNullOrWhiteSpace(filter.Status) && 
            Enum.TryParse<Status>(filter.Status, out var parsedStatus))
        {
            filters.Add(builder.Eq(x => x.Status, parsedStatus));
        }

        if (filter.DateFrom.HasValue)
            filters.Add(builder.Gte(x => x.PublishDate, filter.DateFrom.Value));

        if (filter.DateTo.HasValue)
            filters.Add(builder.Lte(x => x.PublishDate, filter.DateTo.Value));

        if (filters.Any())
            mongoFilter = builder.And(filters);

        // Monta sort dinâmico
        var sort = filter.OrderDirection?.ToLower() == "asc"
            ? Builders<News>.Sort.Ascending(filter.OrderBy ?? "PublishDate")
            : Builders<News>.Sort.Descending(filter.OrderBy ?? "PublishDate");

        // Paginação
        var total = await _repository.CountAsync(mongoFilter);

        var items = await _repository.GetPagedAsync(mongoFilter, sort, filter.Page, filter.PageSize);

        return new PaginatedResult<NewsDto>
        {
            Items = _mapper.Map<List<NewsDto>>(items),
            TotalItems = (int)total,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }


    public async Task<NewsDto> GetNewsByIdAsync(string id)
    {
        var newsEntity = await _repository.GetByIdAsync(id);
        return _mapper.Map<NewsDto>(newsEntity);
    }

    public async Task<NewsDto> GetNewsBySlugAsync(string slug)
    {
        var newsEntity = await _repository.GetBySlugAsync(slug);
        return _mapper.Map<NewsDto>(newsEntity);
    }

    public async Task<NewsDto> CreateNewsAsync(NewsDto newsDto)
    {
        var entity = new News(newsDto.Hat, newsDto.Title, newsDto.Text, newsDto.Author, newsDto.Image, newsDto.Link, newsDto.Status);
        await _repository.CreateAsync(entity);

        return _mapper.Map<NewsDto>(entity);
    }

    public async Task UpdateNewsAsync(string id, NewsDto newsIn)
    {
        await _repository.UpdateAsync(id, _mapper.Map<News>(newsIn));
    }

    public async Task RemoveNewsAsync(string id) => await _repository.RemoveAsync(id);
}
