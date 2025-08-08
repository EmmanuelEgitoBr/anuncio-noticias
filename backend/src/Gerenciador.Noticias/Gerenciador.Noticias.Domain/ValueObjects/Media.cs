using Gerenciador.Noticias.Domain.Enums;

namespace Gerenciador.Noticias.Domain.ValueObjects;

public class Media
{
    public string? MediaUrl { get; set; }
    public MediaType MediaType { get; set; }

    public Media(string mediaUrl, MediaType mediaType)
    {
        MediaUrl = mediaUrl;
        MediaType = mediaType;
    }
}
