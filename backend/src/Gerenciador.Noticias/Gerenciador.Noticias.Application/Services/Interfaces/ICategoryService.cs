using Gerenciador.Noticias.Application.Dtos;

namespace Gerenciador.Noticias.Application.Services.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetCategoryListAsync();
    Task<CategoryDto> GetCategoryByIdAsync(string id);
    Task<CategoryDto> CreateCategoryAsync(CategoryDto categoryDto);
    Task UpdateCategoryAsync(string id, CategoryDto newsIn);
    Task RemoveCategoryAsync(string id);
}
