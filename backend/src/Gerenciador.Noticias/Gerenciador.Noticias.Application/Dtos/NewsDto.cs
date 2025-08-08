using Gerenciador.Noticias.Domain.Enums;
using Gerenciador.Noticias.Domain.ValueObjects;

namespace Gerenciador.Noticias.Application.Dtos;

public class NewsDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string? Text { get; set; }
    public string Author { get; set; } = string.Empty;
    public List<Media>? Medias { get; set; }
    public string? Link { get; set; }
    public string Slug { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int CategoryId { get; set; }
    public Status Status { get; set; }
}
