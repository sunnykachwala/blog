namespace Np.Admin.Service.Categories
{
    using Np.Admin.Service.Categories.Model;
    using Np.ViewModel;
    public interface ICategoryService
    {
        Task<Guid> Add(CreateCategoryDto model, Guid modifiedBy);
        Task Update(CategoryDto model, Guid modifiedBy);
        Task<List<CategoryDto>?> GetAllCachedParentCategory(FilterDto filter);
        Task<List<CategoryDto>?> AllCachedCategory(FilterDto filter);
        IQueryable<CategoryDto> GetCategory();
        void RemoveParentCategoryCache();
        void RemoveCategoryCache();
    }
}
