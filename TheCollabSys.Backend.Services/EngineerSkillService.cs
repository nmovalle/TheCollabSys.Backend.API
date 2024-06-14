using Microsoft.EntityFrameworkCore;
using TheCollabSys.Backend.Data;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public class EngineerSkillService : IEngineerSkillService
{
    private readonly IUnitOfWork _unitOfWork;
    public EngineerSkillService(
        IUnitOfWork unitOfWork
        )
    {
        _unitOfWork = unitOfWork;
    }

    public IAsyncEnumerable<EngineerSkillDetailDTO> GetAll()
    {
        var data = (from ps in _unitOfWork.EngineerSkillRepository.GetAllQueryable()
                    join p in _unitOfWork.EngineerRepository.GetAllQueryable() on ps.EngineerId equals p.EngineerId
                    join s in _unitOfWork.SkillRepository.GetAllQueryable() on ps.SkillId equals s.SkillId
                    group new { ps, p, s } by new { ps.EngineerId, p.EngineerName } into grouped
                    select new EngineerSkillDetailDTO
                    {
                        EngineerId = grouped.Key.EngineerId,
                        EngineerName = grouped.Key.EngineerName,
                        Skills = grouped.Select(g => new SkillLevelDTO
                        {
                            SkillId = g.s.SkillId,
                            SkillName = g.s.SkillName,
                            LevelId = g.ps.LevelId
                        }).ToList()
                    }).AsAsyncEnumerable();

        return data;
    }

    public Task<EngineerSkillDetailDTO?> GetByIdAsync(int id)
    {
        var data = (from ps in _unitOfWork.EngineerSkillRepository.GetAllQueryable()
                    join p in _unitOfWork.EngineerRepository.GetAllQueryable() on ps.EngineerId equals p.EngineerId
                    join s in _unitOfWork.SkillRepository.GetAllQueryable() on ps.SkillId equals s.SkillId
                    where ps.EngineerId == id
                    group new { ps, p, s } by new { ps.EngineerId, p.EngineerName } into grouped
                    select new EngineerSkillDetailDTO
                    {
                        EngineerId = grouped.Key.EngineerId,
                        EngineerName = grouped.Key.EngineerName,
                        Skills = grouped.Select(g => new SkillLevelDTO
                        {
                            SkillId = g.s.SkillId,
                            SkillName = g.s.SkillName,
                            LevelId = g.ps.LevelId
                        }).ToList()
                    }).FirstOrDefaultAsync();

        return data;
    }

    public async Task Create(EngineerSkillDetailDTO entity)
    {
        await UpdateOrDelete(entity.EngineerId, entity);
    }

    public async Task Update(int id, EngineerSkillDetailDTO dto)
    {
        await UpdateOrDelete(id, dto);
    }

    public async Task Delete(int id)
    {
        await UpdateOrDelete(id, null);
    }

    private async Task UpdateOrDelete(int engineerId, EngineerSkillDetailDTO? dto)
    {
        var existingSkills = await _unitOfWork.EngineerSkillRepository.GetSkillsByEngineerIdAsync(engineerId);
        if (existingSkills != null)
        {
            foreach (var skill in existingSkills)
            {
                _unitOfWork.EngineerSkillRepository.Remove(skill);
            }
        }

        if (dto != null)
        {
            foreach (var skill in dto.Skills)
            {
                var engineerSkill = new DdEngineerSkill
                {
                    EngineerId = dto.EngineerId,
                    SkillId = skill.SkillId,
                    LevelId = skill.LevelId
                };

                _unitOfWork.EngineerSkillRepository.Add(engineerSkill);
            }
        }

        await _unitOfWork.CompleteAsync();
    }
}
