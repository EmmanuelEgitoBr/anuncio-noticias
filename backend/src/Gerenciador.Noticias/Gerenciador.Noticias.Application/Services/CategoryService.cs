using AutoMapper;
using Gerenciador.Noticias.Application.Dtos;
using Gerenciador.Noticias.Application.Services.Interfaces;
using Gerenciador.Noticias.Domain.Entities;
using Gerenciador.Noticias.Domain.Interfaces;

namespace Gerenciador.Noticias.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly IMongoRepository<Category> _repository;

        public CategoryService(IMongoRepository<Category> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<List<CategoryDto>> GetCategoryListAsync()
        {
            var newsListEntity = await _repository.GetAsync();
            return _mapper.Map<List<CategoryDto>>(newsListEntity);
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(string id)
        {
            var newsEntity = await _repository.GetByIdAsync(id);
            return _mapper.Map<CategoryDto>(newsEntity);
        }

        public async Task<CategoryDto> CreateCategoryAsync(CategoryDto categoryDto)
        {
            var entity = new Category(categoryDto.CategoryName);
            await _repository.CreateAsync(entity);

            return _mapper.Map<CategoryDto>(entity);
        }

        public async Task UpdateCategoryAsync(string id, CategoryDto newsIn)
        {
            await _repository.UpdateAsync(id, _mapper.Map<Category>(newsIn));
        }

        public async Task RemoveCategoryAsync(string id) => await _repository.RemoveAsync(id);
    }
}
