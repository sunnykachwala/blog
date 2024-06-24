namespace Np.Admin.Service.Organisations
{
    using Np.ViewModel;

    public interface IOrganisationService
    {
        Task<Guid> AddOrganisation(CreateOrganisationDto organisation, Guid modifiedBy);

        Task<Guid> UpdateOrganisation(OrganisationDto organisation, Guid modifiedBy);
        Task<PaginatedResult<OrganisationDto>> GetOrganisations(FilterDto filter);

        Task<OrganisationDto> GetOrganisationById(Guid id);

        Task<OrganisationDto> GetOrganisatonByName(string name);
    }
}
