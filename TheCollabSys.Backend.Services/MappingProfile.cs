﻿using AutoMapper;
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

    }
}
