using Microsoft.EntityFrameworkCore;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public class EmployerProjectAssignmentService : IEmployerProjectAssignmentService
{
    private readonly IUnitOfWork _unitOfWork;
    public EmployerProjectAssignmentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IAsyncEnumerable<EmployerProjectAssignmentDetailDTO> GetAll(int companyId)
    {
        var data = (from epa in _unitOfWork.EmployerProjectAssignmentRepository.GetAllQueryable()
                    join e in _unitOfWork.EmployerRepository.GetAllQueryable() on epa.EmployerId equals e.EmployerId
                    join p in _unitOfWork.ProjectRepository.GetAllQueryable() on epa.ProjectId equals p.ProjectId
                    join c in _unitOfWork.Clients.GetAllQueryable() on p.ClientId equals c.ClientId
                    where p.CompanyId == companyId
                    group new { epa, e, p, c } by new { epa.EmployerId, e.EmployerName } into grouped
                    select new EmployerProjectAssignmentDetailDTO
                    {
                        EmployerId = grouped.Key.EmployerId,
                        EmployerName = grouped.Key.EmployerName,
                        Assignments = grouped.Select(g => new ProjectAssignments
                        {
                            ProjectId = g.p.ProjectId,
                            ProjectName = g.p.ProjectName,
                            ClientId = g.p.ClientId,
                            ClientName = g.c.ClientName,
                            ProjectDescription = g.p.ProjectDescription,
                            NumberPositionTobeFill = g.p.NumberPositionTobeFill,
                            DateCreated = g.p.DateCreated,
                            StartDate = g.p.StartDate,
                            EndDate = g.p.EndDate,
                            StatusId = g.p.StatusId,
                            DateUpdate = g.p.DateUpdate,
                            DateAssigned = g.epa.DateAssigned
                        }).ToList()
                    }).AsAsyncEnumerable();

        return data;
    }

    public Task<EmployerProjectAssignmentDetailDTO?> GetByIdAsync(int companyId, int id)
    {
        var data = (from epa in _unitOfWork.EmployerProjectAssignmentRepository.GetAllQueryable()
                    join e in _unitOfWork.EmployerRepository.GetAllQueryable() on epa.EmployerId equals e.EmployerId
                    join p in _unitOfWork.ProjectRepository.GetAllQueryable() on epa.ProjectId equals p.ProjectId
                    join c in _unitOfWork.Clients.GetAllQueryable() on p.ClientId equals c.ClientId
                    where p.CompanyId == companyId && epa.EmployerId == id
                    group new { epa, e, p, c } by new { epa.EmployerId, e.EmployerName } into grouped
                    select new EmployerProjectAssignmentDetailDTO
                    {
                        EmployerId = grouped.Key.EmployerId,
                        EmployerName = grouped.Key.EmployerName,
                        Assignments = grouped.Select(g => new ProjectAssignments
                        {
                            ProjectId = g.p.ProjectId,
                            ProjectName = g.p.ProjectName,
                            ClientId = g.p.ClientId,
                            ClientName = g.c.ClientName,
                            ProjectDescription = g.p.ProjectDescription,
                            NumberPositionTobeFill = g.p.NumberPositionTobeFill,
                            DateCreated = g.p.DateCreated,
                            StartDate = g.p.StartDate,
                            EndDate = g.p.EndDate,
                            StatusId = g.p.StatusId,
                            DateUpdate = g.p.DateUpdate,
                            DateAssigned = g.epa.DateAssigned
                        }).ToList()
                    }).FirstOrDefaultAsync();

        return data;
    }

    public async Task Create(EmployerProjectAssignmentDetailDTO entity)
    {
        await UpdateOrDelete(entity.EmployerId, entity);
    }

    public async Task Update(int id, EmployerProjectAssignmentDetailDTO dto)
    {
        await UpdateOrDelete(id, dto);
    }

    public async Task Delete(int id)
    {
        await UpdateOrDelete(id, null);
    }

    private async Task UpdateOrDelete(int employerId, EmployerProjectAssignmentDetailDTO? dto)
    {
        var existing = await _unitOfWork.EmployerProjectAssignmentRepository.GetAssignmentsByEmployerIdAsync(employerId);
        if (existing != null)
        {
            foreach (var assignment in existing)
            {
                _unitOfWork.EmployerProjectAssignmentRepository.Remove(assignment);
            }
        }

        if (dto != null)
        {
            foreach (var assignmentDto in dto.Assignments)
            {
                var existingAssignment = existing?.FirstOrDefault(e => e.ProjectId == assignmentDto.ProjectId);

                var employerAssignment = new DdEmployerProjectAssignment
                {
                    EmployerId = dto.EmployerId,
                    ProjectId = assignmentDto.ProjectId,
                    DateAssigned = assignmentDto.DateAssigned,
                };

                _unitOfWork.EmployerProjectAssignmentRepository.Add(employerAssignment);
            }
        }

        await _unitOfWork.CompleteAsync();
    }
}
