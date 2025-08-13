using Gerenciador.Noticias.Domain.Entities;
using MongoDB.Bson;

namespace Gerenciador.Noticias.Application.Dtos;

public class GalleryDto
{
    public string Id { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string GalleryName { get; set; } = string.Empty;
    public string CategoryId { get; set; } = string.Empty;
    public List<News>? News { get; set; }
}
