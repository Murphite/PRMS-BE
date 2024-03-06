using PRMS.Core.Dtos;

namespace PRMS.Core.Abstractions;

public interface ICategoryService
{
    public Task<Result<IEnumerable<CategoryDto>>> GetCategories();
}