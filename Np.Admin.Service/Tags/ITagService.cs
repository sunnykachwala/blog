namespace Np.Admin.Service.Tags
{
    using Np.ViewModel;

    public interface ITagService
    {
        Task<Guid> Add(CreateTagDto model, string modifiedBy);
        Task Update(TagDto model, string modifiedBy);
        Task<List<TagDto>?> AllCachedTag(FilterDto filter);
        IQueryable<TagDto> GetTag();
        void RemoveTagCache();
    }
}
