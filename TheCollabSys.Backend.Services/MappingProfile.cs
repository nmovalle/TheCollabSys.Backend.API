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


    }
}
