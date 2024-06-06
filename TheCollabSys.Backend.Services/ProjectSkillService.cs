using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Data;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace TheCollabSys.Backend.Services;

public class ProjectSkillService : IProjectSkillService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapperService<ProjectSkillDTO, DdProjectSkill> _mapperService;
    private readonly TheCollabsysContext _context;

    public ProjectSkillService(
        IUnitOfWork unitOfWork,
        IMapperService<ProjectSkillDTO, DdProjectSkill> mapperService,
        TheCollabsysContext context
        )
    {
        _unitOfWork = unitOfWork;
        _mapperService = mapperService;
        _context = context;
    }

    public IAsyncEnumerable<ProjectSkillDetailDTO> GetAll()
    {
        var data = (from ps in _unitOfWork.ProjectSkillRepository.GetAllQueryable()
                    join p in _unitOfWork.ProjectRepository.GetAllQueryable() on ps.ProjectId equals p.ProjectId
                    join s in _unitOfWork.SkillRepository.GetAllQueryable() on ps.SkillId equals s.SkillId
                    group new { ps, p, s } by new { ps.ProjectId, p.ProjectName } into grouped
                    select new ProjectSkillDetailDTO
                    {
                        ProjectId = grouped.Key.ProjectId,
                        ProjectName = grouped.Key.ProjectName,
                        Skills = grouped.Select(g => new SkillLevelDTO
                        {
                            SkillId = g.s.SkillId,
                            SkillName = g.s.SkillName,
                            LevelId = g.ps.LevelId
                        }).ToList()
                    }).AsAsyncEnumerable();

        return data;
    }

    public Task<ProjectSkillDetailDTO?> GetByIdAsync(int id)
    {
        var data = (from ps in _unitOfWork.ProjectSkillRepository.GetAllQueryable()
                    join p in _unitOfWork.ProjectRepository.GetAllQueryable() on ps.ProjectId equals p.ProjectId
                    join s in _unitOfWork.SkillRepository.GetAllQueryable() on ps.SkillId equals s.SkillId
                    where ps.ProjectId == id
                    group new { ps, p, s } by new { ps.ProjectId, p.ProjectName } into grouped
                    select new ProjectSkillDetailDTO
                    {
                        ProjectId = grouped.Key.ProjectId,
                        ProjectName = grouped.Key.ProjectName,
                        Skills = grouped.Select(g => new SkillLevelDTO
                        {
                            SkillId = g.s.SkillId,
                            SkillName = g.s.SkillName,
                            LevelId = g.ps.LevelId
                        }).ToList()
                    }).FirstOrDefaultAsync();

        return data;
    }

    public async Task Create(ProjectSkillDetailDTO entity)
    {
        await UpdateOrDelete(entity.ProjectId, entity);
    }

    public async Task Update(int id, ProjectSkillDetailDTO dto)
    {
        await UpdateOrDelete(id, dto);
    }

    public async Task Delete(int id)
    {
        await UpdateOrDelete(id, null);
    }

    private async Task UpdateOrDelete(int projectId, ProjectSkillDetailDTO? dto)
    {
        // Verifica si existen skills asociadas al proyecto.
        var existingSkills = await _unitOfWork.ProjectSkillRepository.GetSkillsByProjectIdAsync(projectId);

        // Si existen skills asociadas al proyecto, elimina todas las habilidades existentes.
        if (existingSkills != null)
        {
            foreach (var skill in existingSkills)
            {
                _unitOfWork.ProjectSkillRepository.Remove(skill);
            }
        }

        // Si hay un DTO, agrega las nuevas skills proporcionadas en el DTO.
        if (dto != null)
        {
            foreach (var skill in dto.Skills)
            {
                var projectSkill = new DdProjectSkill
                {
                    ProjectId = dto.ProjectId,
                    SkillId = skill.SkillId,
                    LevelId = skill.LevelId
                };

                _unitOfWork.ProjectSkillRepository.Add(projectSkill);
            }
        }

        await _unitOfWork.CompleteAsync();
    }
}
