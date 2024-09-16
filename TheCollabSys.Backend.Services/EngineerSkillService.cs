﻿using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
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

    public IAsyncEnumerable<EngineerSkillDetailDTO> GetAll(int companyId)
    {
        var data = (from ps in _unitOfWork.EngineerSkillRepository.GetAllQueryable()
                    join p in _unitOfWork.EngineerRepository.GetAllQueryable() on ps.EngineerId equals p.EngineerId
                    join s in _unitOfWork.SkillRepository.GetAllQueryable() on ps.SkillId equals s.SkillId
                    where p.CompanyId == companyId
                    group new { ps, p, s } by new { ps.EngineerId, p.EngineerName, p.FirstName, p.LastName } into grouped
                    select new EngineerSkillDetailDTO
                    {
                        EngineerId = grouped.Key.EngineerId,
                        EngineerName = grouped.Key.EngineerName,
                        FirstName = grouped.Key.FirstName,
                        LastName = grouped.Key.LastName,
                        Skills = grouped.Select(g => new SkillLevelDTO
                        {
                            SkillId = g.s.SkillId,
                            SkillName = g.s.SkillName,
                            LevelId = g.ps.LevelId
                        }).ToList()
                    }).AsAsyncEnumerable();

        return data;
    }

    public Task<EngineerSkillDetailDTO?> GetByIdAsync(int companyId, int id)
    {
        var data = (from ps in _unitOfWork.EngineerSkillRepository.GetAllQueryable()
                    join e in _unitOfWork.EngineerRepository.GetAllQueryable() on ps.EngineerId equals e.EngineerId
                    join s in _unitOfWork.SkillRepository.GetAllQueryable() on ps.SkillId equals s.SkillId
                    where e.CompanyId == companyId && ps.EngineerId == id
                    group new { ps, e, s } by new { ps.EngineerId, e.EngineerName, e.FirstName, e.LastName } into grouped
                    select new EngineerSkillDetailDTO
                    {
                        EngineerId = grouped.Key.EngineerId,
                        EngineerName = grouped.Key.EngineerName,
                        FirstName = grouped.Key.FirstName,
                        LastName = grouped.Key.LastName,
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
