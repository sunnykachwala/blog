namespace Np.Admin.Service.Categories
{
    using Np.ViewModel;
    public interface ICategoryService
    {
        Task<Guid> Add(CreateCategoryDto model, string modifiedBy);
        Task Update(CategoryDto model, string modifiedBy);
        Task<List<CategoryDto>?> GetAllCachedParentCategory(FilterDto filter);
        Task<List<CategoryDto>?> AllCachedCategory(FilterDto filter);
        IQueryable<CategoryDto> GetCategory();
        void RemoveParentCategoryCache();
        void RemoveCategoryCache();
    }
}
