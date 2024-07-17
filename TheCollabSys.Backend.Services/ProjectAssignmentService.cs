using Microsoft.EntityFrameworkCore;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public class ProjectAssignmentService : IProjectAssignmentService
{
    private readonly IUnitOfWork _unitOfWork;
    public ProjectAssignmentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IAsyncEnumerable<ProjectAssignmentDetailDTO> GetAll()
    {
        var data = (from pa in _unitOfWork.ProjectAssignmentRepository.GetAllQueryable()
                    join p in _unitOfWork.ProjectRepository.GetAllQueryable() on pa.ProjectId equals p.ProjectId
                    join e in _unitOfWork.EngineerRepository.GetAllQueryable() on pa.EngineerId equals e.EngineerId
                    group new { pa, p, e } by new { pa.ProjectId, p.ProjectName } into grouped
                    select new ProjectAssignmentDetailDTO
                    {
                        ProjectId = grouped.Key.ProjectId,
                        ProjectName = grouped.Key.ProjectName,
                        Assignments = grouped.Select(g => new Assignments
                        {
                            EngineerId = g.e.EngineerId,
                            EngineerName = g.e.EngineerName,
                            FirstName = g.e.FirstName,
                            LastName = g.e.LastName,
                            Email = g.e.Email,
                            Phone = g.e.Phone,
                            Image = g.e.Image,
                            Rating = 5, //pendiente relación con DD_EngineerRatings
                            StartDate = g.pa.StartDate,
                            EndDate = g.pa.EndDate,
                            DateCreated = g.pa.DateCreated,
                            UpdatedDate = g.pa.UpdatedDate,
                        }).ToList()
                    }).AsAsyncEnumerable();

        return data;
    }

    public Task<ProjectAssignmentDetailDTO?> GetByIdAsync(int id)
    {
        var data = (from pa in _unitOfWork.ProjectAssignmentRepository.GetAllQueryable()
                    join p in _unitOfWork.ProjectRepository.GetAllQueryable() on pa.ProjectId equals p.ProjectId
                    join e in _unitOfWork.EngineerRepository.GetAllQueryable() on pa.EngineerId equals e.EngineerId
                    where pa.ProjectId == id
                    group new { pa, p, e } by new { pa.ProjectId, p.ProjectName } into grouped
                    select new ProjectAssignmentDetailDTO
                    {
                        ProjectId = grouped.Key.ProjectId,
                        ProjectName = grouped.Key.ProjectName,
                        Assignments = grouped.Select(g => new Assignments
                        {
                            EngineerId = g.e.EngineerId,
                            EngineerName = g.e.EngineerName,
                            FirstName = g.e.FirstName,
                            LastName = g.e.LastName,
                            Email = g.e.Email,
                            Phone = g.e.Phone,
                            Image = g.e.Image,
                            Rating = 5, //pendiente relación con DD_EngineerRatings
                            StartDate = g.pa.StartDate,
                            EndDate = g.pa.EndDate,
                            DateCreated = g.pa.DateCreated,
                            UpdatedDate = g.pa.UpdatedDate,
                        }).ToList()
                    }).FirstOrDefaultAsync();
        
        return data;
    }

    public async Task Create(ProjectAssignmentDetailDTO entity)
    {
        await UpdateOrDelete(entity.ProjectId, entity);
    }

    public async Task Update(int id, ProjectAssignmentDetailDTO dto)
    {
        await UpdateOrDelete(id, dto);
    }

    public async Task Delete(int id)
    {
        await UpdateOrDelete(id, null);
    }

    private async Task UpdateOrDelete(int projectId, ProjectAssignmentDetailDTO? dto)
    {
        var existing = await _unitOfWork.ProjectAssignmentRepository.GetAssignmentsByProjectIdAsync(projectId);
        if (existing != null)
        {
            foreach (var assignment in existing)
            {
                _unitOfWork.ProjectAssignmentRepository.Remove(assignment);
            }
        }

        if (dto != null)
        {
            foreach (var assignmentDto in dto.Assignments)
            {
                var existingAssignment = existing?.FirstOrDefault(e => e.EngineerId == assignmentDto.EngineerId);

                var projectAssignment = new DdProjectAssignment
                {
                    ProjectId = dto.ProjectId,
                    EngineerId = assignmentDto.EngineerId,
                    StartDate = assignmentDto.StartDate,
                    EndDate = assignmentDto.EndDate,
                    DateCreated = existingAssignment?.DateCreated ?? DateTime.Now,
                    UpdatedDate = existingAssignment?.DateCreated ?? DateTime.Now
                };

                _unitOfWork.ProjectAssignmentRepository.Add(projectAssignment);
            }
        }

        await _unitOfWork.CompleteAsync();
    }
}
