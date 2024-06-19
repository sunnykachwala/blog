using Np.DAL.Domain;
using Np.DAL.Repository;
namespace Np.Admin.Service.Organisations
{
    using Np.ViewModel;
    using AutoMapper;
    using global::Np.Common;
    using Microsoft.EntityFrameworkCore;

    public class OrganisationService : IOrganisationService
    {
        private readonly IBaseRepository<Organisation> orgRepo;
        private readonly IMapper mapper;

        public OrganisationService(IBaseRepository<Organisation> _orgRepo
            , IMapper mapper)
        {
            this.orgRepo = _orgRepo;
            this.mapper = mapper;
        }

        public async Task<Guid> AddOrganisation(CreateOrganisationDto organisation, string modifiedBy)
        {
            var orgInfo = await this.GetOrganisatonByName(organisation.OrganisationName);

            if (orgInfo != null)
                throw new Exception($"Organisation with name {organisation.OrganisationName} already exists!");

            var org = mapper.Map<Organisation>(organisation);
            org.CreatedBy = modifiedBy;
            this.orgRepo.Insert(org);
            this.orgRepo.Save();
            return org.OrganisationGuid;
        }

        public async Task<Guid> UpdateOrganisation(OrganisationDto organisation, string modifiedBy)
        {
            var duplicateOrganisation = await this.orgRepo.GetFindByColumnAsync(x => x.OrganisationGuid != organisation.OrganisationGuid && x.OrganisationName == organisation.OrganisationName);

            if (duplicateOrganisation != null)
                throw new Exception($"Organisation with name {organisation.OrganisationName} already exists!");

            var org = await this.orgRepo.GetFindByColumnAsync(x => x.OrganisationGuid == organisation.OrganisationGuid);
            if (org == null)
                throw new Exception($"Organisation with id {organisation.OrganisationGuid} doese not exists!");

            var data = mapper.Map<Organisation>(organisation);
            this.orgRepo.Edit(data);
            this.orgRepo.Save();
            return org.OrganisationGuid;
        }

        public async Task<PaginatedResult<OrganisationDto>> GetOrganisations(FilterDto filter)
        {
            var skip = PageHelper.GetSkipCount(filter.Page, filter.PageSize);
            var result = new PaginatedResult<OrganisationDto>
            {
                TotalRecord = this.orgRepo.GetAllCustom()
                                .Count(x => x.IsActive == filter.IsActive),

                List = await this.orgRepo.GetAllCustom()
                   .Where(x => x.IsActive == filter.IsActive && string.IsNullOrEmpty(filter.Search) || x.OrganisationName == filter.Search)
                   .Select(x => mapper.Map<OrganisationDto>(x))
                   .Skip(skip)
                   .Take(filter.PageSize)
                   .ToListAsync()
            };
            return result;
        }

        public async Task<OrganisationDto> GetOrganisationById(Guid id)
        {
            var organisation = await this.orgRepo.GetFindByColumnAsync(x => x.OrganisationGuid == id);
            return this.mapper.Map<OrganisationDto>(organisation);
        }

        public async Task<OrganisationDto> GetOrganisatonByName(string name)
        {
            var organisation = await this.orgRepo.GetFindByColumnAsync(x => x.OrganisationName == name);
            return this.mapper.Map<OrganisationDto>(organisation);
        }

     
    }
}
