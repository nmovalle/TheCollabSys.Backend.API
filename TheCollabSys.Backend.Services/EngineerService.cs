using Microsoft.EntityFrameworkCore;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public class EngineerService : IEngineerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapperService<EngineerDTO, DdEngineer> _mapperService;
    public EngineerService(
        IUnitOfWork unitOfWork,
        IMapperService<EngineerDTO, DdEngineer> mapperService
        )
    {
        _unitOfWork = unitOfWork;
        _mapperService = mapperService;
    }

    public IAsyncEnumerable<EngineerDTO> GetAll(int companyId)
    {
        var data = _unitOfWork.EngineerRepository.GetAllQueryable()
            .Where(c => c.CompanyId == companyId)
            .Select(c => new EngineerDTO
            {
                EngineerId = c.EngineerId,
                EmployerId = c.EmployerId,
                EmployerName = c.Employer.EmployerName,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                Phone = c.Phone,
                Image = c.Image,
                Filetype = c.Filetype,
                DateCreated = c.DateCreated,
                DateUpdate = c.DateUpdate,
                IsActive = c.IsActive,
                UserId = c.UserId
            })
            .AsAsyncEnumerable();

        return data;
    }

    public IAsyncEnumerable<EngineerSkillDetailDTO> GetDetail(int companyId)
    {
        var data = (from e in _unitOfWork.EngineerRepository.GetAllQueryable()
                    join em in _unitOfWork.EmployerRepository.GetAllQueryable() on e.EmployerId equals em.EmployerId
                    // Left join con EngineerSkill
                    join es in _unitOfWork.EngineerSkillRepository.GetAllQueryable() on e.EngineerId equals es.EngineerId into esGroup
                    from es in esGroup.DefaultIfEmpty()
                        // Left join con Skill
                    join s in _unitOfWork.SkillRepository.GetAllQueryable() on es.SkillId equals s.SkillId into sGroup
                    from s in sGroup.DefaultIfEmpty()
                    where e.CompanyId == companyId
                    group new { es, e, em, s } by new
                    {
                        e.EngineerId,
                    } into grouped
                    select new EngineerSkillDetailDTO
                    {
                        EngineerId = grouped.Key.EngineerId,
                        EmployerId = grouped.Select(g => g.em.EmployerId).FirstOrDefault(),
                        EmployerName = grouped.Select(g => g.em.EmployerName).FirstOrDefault(),
                        FirstName = grouped.Select(g => g.e.FirstName).FirstOrDefault(),
                        LastName = grouped.Select(g => g.e.LastName).FirstOrDefault(),
                        Email = grouped.Select(g => g.e.Email).FirstOrDefault(),
                        Phone = grouped.Select(g => g.e.Phone).FirstOrDefault(),
                        DateCreated = grouped.Select(g => g.e.DateCreated).FirstOrDefault(),
                        DateUpdate = grouped.Select(g => g.e.DateUpdate).FirstOrDefault(),
                        IsActive = grouped.Select(g => g.e.IsActive).FirstOrDefault(),
                        UserId = grouped.Select(g => g.e.UserId).FirstOrDefault(),
                        Skills = grouped.Select(g => new SkillLevelDTO
                        {
                            SkillId = g.s != null ? g.s.SkillId : 0,
                            SkillName = g.s != null ? g.s.SkillName : string.Empty,
                            LevelId = g.es != null ? g.es.LevelId : 0
                        }).Where(skill => skill.SkillId != 0 || !string.IsNullOrEmpty(skill.SkillName)).ToList()
                    }).AsAsyncEnumerable();

        return data;
    }

    public IAsyncEnumerable<EngineerDTO> GetEngineersByProjectSkillsAsync(int companyId, int projectId)
    {
        var data = _unitOfWork.EngineerRepository.GetAllQueryable()
            .Where(e => e.CompanyId == companyId)
            .Join(_unitOfWork.EngineerSkillRepository.GetAllQueryable(),
                  engineer => engineer.EngineerId,
                  engineerSkill => engineerSkill.EngineerId,
                  (engineer, engineerSkill) => new { engineer, engineerSkill })
            .Join(_unitOfWork.ProjectSkillRepository.GetAllQueryable(),
                  combined => combined.engineerSkill.SkillId,
                  projectSkill => projectSkill.SkillId,
                  (combined, projectSkill) => new { combined.engineer, combined.engineerSkill, projectSkill })
            .Join(_unitOfWork.ProjectRepository.GetAllQueryable()
            .Where(p => p.CompanyId == companyId),
                  combined => combined.projectSkill.ProjectId,
                  project => project.ProjectId,
                  (combined, project) => new { combined.engineer, project })
            .Where(result => result.project.CompanyId == companyId &&
                             result.engineer.CompanyId == companyId &&
                             result.project.ProjectId == projectId)
            .GroupBy(result => new
            {
                result.engineer.EngineerId,
                result.engineer.EngineerName,
                result.engineer.EmployerId,
                result.engineer.Employer.EmployerName,
                result.engineer.FirstName,
                result.engineer.LastName,
                result.engineer.Email,
                result.engineer.Phone,
                result.engineer.Image,
                result.engineer.DateCreated,
                result.engineer.IsActive,
                result.engineer.UserId,
                result.engineer.DateUpdate,
                result.engineer.Filetype
            })
            .Select(group => new EngineerDTO
            {
                EngineerId = group.Key.EngineerId,
                EmployerId = group.Key.EmployerId,
                EmployerName = group.Key.EmployerName,
                FirstName = group.Key.FirstName,
                LastName = group.Key.LastName,
                Email = group.Key.Email,
                Phone = group.Key.Phone,
                Image = group.Key.Image,
                DateCreated = group.Key.DateCreated,
                IsActive = group.Key.IsActive,
                UserId = group.Key.UserId,
                DateUpdate = group.Key.DateUpdate,
                Filetype = group.Key.Filetype,
                Rating = _unitOfWork.EngineerSkillRepository.GetAllQueryable()
                    .Where(es => es.Engineer.CompanyId == companyId && es.EngineerId == group.Key.EngineerId)
                    .Average(es => (double?)es.LevelId) ?? 0
            })
            .AsAsyncEnumerable();

        return data;
    }

    public async Task<EngineerDTO?> GetByIdAsync(int companyId, int id)
    {
        var resp = await _unitOfWork.EngineerRepository.GetAllQueryable()
            .Where(c => c.CompanyId == companyId && c.EngineerId == id)
            .Select(c => new EngineerDTO
            {
                EngineerId = c.EngineerId,
                EmployerId = c.EmployerId,
                EmployerName = c.Employer.EmployerName,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                Phone = c.Phone,
                Image = c.Image,
                Filetype = c.Filetype,
                DateCreated = c.DateCreated,
                DateUpdate = c.DateUpdate,
                IsActive = c.IsActive,
                UserId = c.UserId
            })
            .FirstOrDefaultAsync();

        return resp;
    }

    public async Task<DdEngineer> Create(DdEngineer entity)
    {
        entity.DateCreated = DateTime.Now;
        _unitOfWork.EngineerRepository.Add(entity);
        await _unitOfWork.CompleteAsync();
        return entity;
    }

    public async Task<DdEngineer> Update(int id, EngineerDTO dto)
    {
        var existing = await _unitOfWork.EngineerRepository.GetByIdAsync(id);
        if (existing == null)
            throw new ArgumentException("engineer not found");

        dto.DateUpdate = DateTime.Now;

        var excludeProperties = new List<string> { "DateCreated" };
        _mapperService.Map(dto, existing, excludeProperties);

        _unitOfWork.EngineerRepository.Update(existing);

        await _unitOfWork.CompleteAsync();
        return existing;
    }

    public async Task Delete(int id)
    {
        var entity = await _unitOfWork.EngineerRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new ArgumentException("engineer not found");
        }

        _unitOfWork.EngineerRepository.Remove(entity);
        await _unitOfWork.CompleteAsync();
    }
}
