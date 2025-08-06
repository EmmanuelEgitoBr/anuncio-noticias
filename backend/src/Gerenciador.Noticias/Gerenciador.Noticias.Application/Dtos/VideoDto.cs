using Gerenciador.Noticias.Domain.Enums;

namespace Gerenciador.Noticias.Application.Dtos;

public class VideoDto
{
    public string? Id { get; set; }
    public bool IsDeleted { get; set; }
    public string Slug { get; set; } = string.Empty;
    public DateTime PublishDate { get; set; }
    public Status Status { get; set; }
    public string Hat { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Thumbnail { get; set; } = string.Empty;
}
