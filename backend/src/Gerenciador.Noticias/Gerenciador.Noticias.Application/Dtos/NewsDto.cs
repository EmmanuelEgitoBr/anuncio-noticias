using Gerenciador.Noticias.Domain.Enums;

namespace Gerenciador.Noticias.Application.Dtos;

public class NewsDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Hat { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
    public Status Status { get; set; }
}
