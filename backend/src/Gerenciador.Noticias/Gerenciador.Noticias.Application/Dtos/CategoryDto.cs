namespace Gerenciador.Noticias.Application.Dtos;

public class CategoryDto
{
    public string Id { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string CategoryName { get; set; } = string.Empty;
}
