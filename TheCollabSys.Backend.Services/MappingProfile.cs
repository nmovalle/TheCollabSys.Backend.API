using AutoMapper;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<DD_Clients, ClientDTO>();
        CreateMap<ClientDTO, DD_Clients>();
    }
}
