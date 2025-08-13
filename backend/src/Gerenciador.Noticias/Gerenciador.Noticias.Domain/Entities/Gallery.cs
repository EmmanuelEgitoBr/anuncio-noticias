using Gerenciador.Noticias.Domain.Entities.Base;

namespace Gerenciador.Noticias.Domain.Entities;

public class Gallery : BaseEntity
{
    public string GalleryName { get; set; } = string.Empty;
    public string CategoryId { get; set; }
    public List<News>? News { get; set; }

    public Gallery(string galleryName, string categoryId, List<News>? news)
    {
        GalleryName = galleryName;
        CategoryId = categoryId;
        News = news;
    }
}
