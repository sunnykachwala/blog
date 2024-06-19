namespace Np.Admin.Service.UrlRecords
{
    using Np.ViewModel;
    public interface IUrlRecordService
    {
        Task<bool> IsSlugUnique(string slug);
        Guid AddUrlRecord(CreateUrlReordDto model, string modifiedbBy);
    }
}
