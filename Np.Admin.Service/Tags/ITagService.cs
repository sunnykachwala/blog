namespace Np.Admin.Service.Tags
{
    using Np.ViewModel;

    public interface ITagService
    {
        Task<Guid> Add(CreateTagDto model, Guid modifiedBy);
        Task Update(TagDto model, Guid modifiedBy);
        Task<List<TagDto>?> AllCachedTag(FilterDto filter);
        IQueryable<TagDto> GetTag();
        void RemoveTagCache();
    }
}
