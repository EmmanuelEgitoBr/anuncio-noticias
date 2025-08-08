using Gerenciador.Noticias.Domain.Entities.Base;

namespace Gerenciador.Noticias.Domain.Entities;

public class Gallery : BaseEntity
{
    public string GalleryName { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public IEnumerable<News> News { get; set; } = new List<News>();
}
