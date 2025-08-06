using Gerenciador.Noticias.Domain.Entities.Base;

namespace Gerenciador.Noticias.Domain.Entities;

public class Category : BaseEntity
{
    public string CategoryName { get; set; } = string.Empty;

    public Category(string categoryName)
    {
        CategoryName = categoryName;
    }
}
