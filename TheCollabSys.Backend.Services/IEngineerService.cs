﻿using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public interface IEngineerService
{
    IAsyncEnumerable<EngineerDTO> GetAll(int companyId, string? email = null); //se agrega filtro por email
    IAsyncEnumerable<EngineerSkillDetailDTO> GetDetail(int companyId, string? email = null);
    IAsyncEnumerable<EngineerDTO> GetEngineersByProjectSkillsAsync(int companyId, int projectId);
    Task<EngineerDTO?> GetByIdAsync(int companyId, int id);
    Task<DdEngineer> Create(DdEngineer entity);
    Task<DdEngineer> Update(int id, EngineerDTO dto);
    Task Delete(int id);
}
