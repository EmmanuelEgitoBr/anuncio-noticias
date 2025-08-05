using Gerenciador.Noticias.Domain.Enums;
using System.Text.Json.Serialization;

namespace Gerenciador.Noticias.Application.Dtos;

public class NewsDto
{
    public string? Id { get; set; }
    public string Hat { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    [JsonIgnore]
    public DateTime PublishDate {  get; set; }
    public string Image { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
    public Status Status { get; set; }
}
