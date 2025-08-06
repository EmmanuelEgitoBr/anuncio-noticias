using AutoMapper;
using Gerenciador.Noticias.Application.Dtos;
using Gerenciador.Noticias.Domain.Entities;

namespace Gerenciador.Noticias.Application.Mappings;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<News, NewsDto>().ReverseMap();
    }
}
