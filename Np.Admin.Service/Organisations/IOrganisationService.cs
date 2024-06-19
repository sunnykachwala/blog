namespace Np.Admin.Service.Organisations
{
    using Np.ViewModel;

    public interface IOrganisationService
    {
        Task<Guid> AddOrganisation(CreateOrganisationDto organisation, string modifiedBy);

        Task<Guid> UpdateOrganisation(OrganisationDto organisation, string modifiedBy);
        Task<PaginatedResult<OrganisationDto>> GetOrganisations(FilterDto filter);

        Task<OrganisationDto> GetOrganisationById(Guid id);

        Task<OrganisationDto> GetOrganisatonByName(string name);
    }
}
