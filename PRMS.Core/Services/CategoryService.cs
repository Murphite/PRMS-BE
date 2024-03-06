using Microsoft.EntityFrameworkCore;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;
using PRMS.Domain.Entities;

namespace PRMS.Core.Services;

public class CategoryService : ICategoryService
{
    private readonly IRepository _repository;

    public CategoryService(IRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result<IEnumerable<CategoryDto>>> GetCategories()
    {
        var categories = await _repository.GetAll<MedicalCenter>()
            .Select(c => new CategoryDto(c.Name, c.ImageUrl!))
            .ToListAsync();

        return categories;
    }
}