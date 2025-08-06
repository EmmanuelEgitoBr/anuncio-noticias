namespace Gerenciador.Noticias.Application.Dtos.Filters;

public class NewsPaginationFilter
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Status { get; set; }

    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }

    public string? OrderBy { get; set; } = "PublishDate";
    public string? OrderDirection { get; set; } = "desc";
}
