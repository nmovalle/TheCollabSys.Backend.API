using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Data;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace TheCollabSys.Backend.Services;

public class ProjectService : IProjectService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapperService<ProjectDTO, DdProject> _mapperService;
    private readonly TheCollabsysContext _context;

    public ProjectService(
        IUnitOfWork unitOfWork,
        IMapperService<ProjectDTO, DdProject> mapperService,
        TheCollabsysContext context
        )
    {
        _unitOfWork = unitOfWork;
        _mapperService = mapperService;
        _context = context;
    }

    public IAsyncEnumerable<ProjectDTO> GetAll()
    {
        var data = (from p in _unitOfWork.ProjectRepository.GetAllQueryable()
                    join c in _unitOfWork.Clients.GetAllQueryable()
                    on p.ClientId equals c.ClientId
                    select new ProjectDTO
                    {
                        ProjectId = p.ProjectId,
                        ProjectName = p.ProjectName,
                        ClientId = p.ClientId,
                        ClientName = c.ClientName,
                        ProjectDescription = p.ProjectDescription,
                        NumberPositionTobeFill = p.NumberPositionTobeFill,
                        DateCreated = p.DateCreated,
                        StartDate = p.StartDate,
                        EndDate = p.EndDate,
                        StatusId = p.StatusId,
                        UserId = p.UserId,
                        DateUpdate = p.DateUpdate,
                    })
            .AsAsyncEnumerable();

        return data;
    }

    public async Task<ProjectDTO?> GetByIdAsync(int id)
    {
        var resp = await _unitOfWork.ProjectRepository.GetAllQueryable()
        .Where(p => p.ProjectId == id)
        .Join(
            _unitOfWork.Clients.GetAllQueryable(),
            project => project.ClientId,
            client => client.ClientId,
            (project, client) => new ProjectDTO
            {
                ProjectId = project.ProjectId,
                ProjectName = project.ProjectName,
                ClientId = project.ClientId,
                ClientName = client.ClientName,
                ProjectDescription = project.ProjectDescription,
                NumberPositionTobeFill = project.NumberPositionTobeFill,
                DateCreated = project.DateCreated,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                StatusId = project.StatusId,
                UserId = project.UserId,
                DateUpdate = project.DateUpdate,
            })
        .FirstOrDefaultAsync();

        return resp;
    }

    public async Task<DdProject> Create(DdProject entity)
    {
        entity.DateCreated = DateTime.Now;
        _unitOfWork.ProjectRepository.Add(entity);
        await _unitOfWork.CompleteAsync();
        return entity;
    }

    public async Task Update(int id, ProjectDTO dto)
    {
        var existing = await _unitOfWork.ProjectRepository.GetByIdAsync(id);
        if (existing == null)
            throw new ArgumentException("project not found");

        dto.DateUpdate = DateTime.Now;

        var excludeProperties = new List<string> { "DateCreated" };
        _mapperService.Map(dto, existing, excludeProperties);

        _unitOfWork.ProjectRepository.Update(existing);

        await _unitOfWork.CompleteAsync();
    }

    public async Task Delete(int id)
    {
        var entity = await _unitOfWork.ProjectRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new ArgumentException("project not found");
        }

        _unitOfWork.ProjectRepository.Remove(entity);
        await _unitOfWork.CompleteAsync();
    }
}
