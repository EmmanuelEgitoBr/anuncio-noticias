using MongoDB.Bson;

namespace Gerenciador.Noticias.Application.Dtos;

public class GalleryDto
{
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string GalleryName { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public List<NewsDto> News { get; set; } = new List<NewsDto>();
}
