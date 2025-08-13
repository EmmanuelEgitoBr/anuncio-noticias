namespace Gerenciador.Noticias.Application.Dtos.Filters;

public class GalleryPaginationFilter
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public string CategoryId { get; set; } = string.Empty;

    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }

    public string? OrderBy { get; set; } = "CreatedAt";
    public string? OrderDirection { get; set; } = "desc";
}
