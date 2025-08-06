using AutoMapper;
using Gerenciador.Noticias.Application.Dtos;
using Gerenciador.Noticias.Application.Services.Interfaces;
using Gerenciador.Noticias.Domain.Entities;
using Gerenciador.Noticias.Domain.Interfaces;

namespace Gerenciador.Noticias.Application.Services;

public class VideoService : IVideoService
{
    private readonly IMongoRepository<Video> _repository;
    private readonly IMapper _mapper;

    public VideoService(IMongoRepository<Video> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<VideoDto>> GetVideoListAsync()
    {
        var videoListEntity = await _repository.GetAsync();
        return _mapper.Map<List<VideoDto>>(videoListEntity);
    }

    public async Task<VideoDto> GetVideoByIdAsync(string id)
    {
        var videoEntity = await _repository.GetByIdAsync(id);
        return _mapper.Map<VideoDto>(videoEntity);
    }

    public async Task<VideoDto> GetVideoBySlugAsync(string slug)
    {
        var videoEntity = await _repository.GetBySlugAsync(slug);
        return _mapper.Map<VideoDto>(videoEntity);
    }

    public async Task<VideoDto> CreateVideoAsync(VideoDto videoDto)
    {
        var entity = new Video(videoDto.Hat, videoDto.Title, videoDto.Author, videoDto.Thumbnail, videoDto.Status);
        await _repository.CreateAsync(entity);

        return _mapper.Map<VideoDto>(entity);
    }

    public async Task UpdateVideoAsync(string id, VideoDto videoIn)
    {
        await _repository.UpdateAsync(id, _mapper.Map<Video>(videoIn));
    }

    public async Task RemoveVideoAsync(string id) => await _repository.RemoveAsync(id);
}
