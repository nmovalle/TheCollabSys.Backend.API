using AutoMapper;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<DdClient, ClientDTO>();
        CreateMap<ClientDTO, DdClient>();

        CreateMap<UserDTO, AspNetUser>();
        CreateMap<AspNetUser, UserDTO>();

        CreateMap<UserRoleDTO, AspNetUserRole>();
        CreateMap<AspNetUserRole, UserRoleDTO>();

        CreateMap<AspNetRole, RoleDTO>();
        CreateMap<RoleDTO, AspNetRole>();

        CreateMap<DdProposalRole, ProposalRoleDTO>();
        CreateMap<ProposalRoleDTO, DdProposalRole>();

        CreateMap<EmployerDTO, DdEmployer>();

        CreateMap<SkillDTO, DdSkill>();

        CreateMap<ProjectDTO, DdProject>();

        CreateMap<ProjectSkillDTO, DdProjectSkill>();

        CreateMap<DdEngineer, EngineerDTO>();
        CreateMap<EngineerDTO, DdEngineer>();

        CreateMap<DdEngineerSkill, EngineerSkillDTO>();
        CreateMap<EngineerSkillDTO, DdEngineerSkill>();

        CreateMap<SkillCategoryDTO, DdSkillCategory>();
        CreateMap<DdSkillCategory, SkillCategoryDTO>();

        CreateMap<SkillSubcategoryDTO, DdSkillSubcategory>();
        CreateMap<DdSkillSubcategory, SkillSubcategoryDTO>();

        CreateMap<MenuDTO, DdMenu>();
        CreateMap<DdMenu, MenuDTO>();

        CreateMap<SubMenuDTO, DdSubMenu>();
        CreateMap<DdSubMenu, SubMenuDTO>();

        CreateMap<MenuRoleDTO, DdMenuRole>();
        CreateMap<DdMenuRole, MenuRoleDTO>();

        CreateMap<AspNetUserTokensDTO, AspNetUserToken>();
        CreateMap<AspNetUserToken, AspNetUserTokensDTO>();

        CreateMap<AccessCodeDTO, DdAccessCode>();
        CreateMap<DdAccessCode, AccessCodeDTO>();

        CreateMap<CompanyDTO, DdCompany>();
        CreateMap<DdCompany, CompanyDTO>();

        CreateMap<UserCompanyDTO, DdUserCompany>();
        CreateMap<DdUserCompany, UserCompanyDTO>();

        CreateMap<DomainMasterDTO, DdDomainMaster>();
        CreateMap<DdDomainMaster, DomainMasterDTO>();

        CreateMap<InvitationDTO, DdInvitation>();
        CreateMap<DdInvitation, InvitationDTO>();

        CreateMap<WireListDTO, DdWireList>();
        CreateMap<DdWireList, WireListDTO>();
    }
}
