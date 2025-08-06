using AutoMapper;
using Gerenciador.Noticias.Application.Dtos;
using Gerenciador.Noticias.Application.Services.Interfaces;
using Gerenciador.Noticias.Domain.Entities;
using Gerenciador.Noticias.Domain.Interfaces;

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
