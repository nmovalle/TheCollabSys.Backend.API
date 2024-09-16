using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Data;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;

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

    public IAsyncEnumerable<ProjectDTO> GetAll(int companyId)
    {
        var data = (from p in _unitOfWork.ProjectRepository.GetAllQueryable()
                    join c in _unitOfWork.Clients.GetAllQueryable()
                    on p.ClientId equals c.ClientId
                    where p.CompanyId == companyId
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

    public async Task<ProjectDTO?> GetByIdAsync(int companyId, int id)
    {
        var resp = await _unitOfWork.ProjectRepository.GetAllQueryable()
        .Where(p => p.CompanyId == companyId && p.ProjectId == id)
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

    private async Task<IEnumerable<dynamic>> ExecuteStoredProcedure(string storedProcedureName)
    {
        using var command = _context.Database.GetDbConnection().CreateCommand();
        command.CommandText = $"EXEC {storedProcedureName}";
        await _context.Database.OpenConnectionAsync();
        using var result = await command.ExecuteReaderAsync();
        var dynamicResults = new List<dynamic>();
        while (await result.ReadAsync())
        {
            dynamic dynamicResult = new ExpandoObject();
            var rowDict = (IDictionary<string, object>)dynamicResult;
            for (int i = 0; i < result.FieldCount; i++)
            {
                rowDict[result.GetName(i)] = result.GetValue(i);
            }

            dynamicResults.Add(dynamicResult);
        }

        return dynamicResults;
    }

    public async Task<IEnumerable<dynamic>> GetSpGetOverviewProjects()
    {
        return await ExecuteStoredProcedure("SpGetOverviewProjects");
    }

    public async Task<IEnumerable<dynamic>> GetSpGetOnComingProjects()
    {
        return await ExecuteStoredProcedure("SpGetOnComingProjects");
    }

    public async Task<IEnumerable<dynamic>> GetSpGetProjectsPendingResources()
    {
        return await ExecuteStoredProcedure("SpGetProjectsPendingResources");
    }

    public async Task<IEnumerable<dynamic>> GetSpGetOpenProjects()
    {
        return await ExecuteStoredProcedure("SpGetOpenProjects");
    }

    public async Task<IEnumerable<dynamic>> GetSpGetPendingProjects()
    {
        return await ExecuteStoredProcedure("SpGetPendingProjects");
    }

    public async Task<dynamic> GetKPIs()
    {
        var overViewProjects = await this.GetSpGetOverviewProjects();
        var oncomingProjects = await this.GetSpGetOnComingProjects();
        var pendingResourcesProjects = await this.GetSpGetProjectsPendingResources();
        var openProjects = await this.GetSpGetOpenProjects();
        var pendingProjects = await this.GetSpGetPendingProjects();

        dynamic kpis = new ExpandoObject();
        kpis.overViewProjects = overViewProjects.FirstOrDefault();
        kpis.oncomingProjects = oncomingProjects.FirstOrDefault();
        kpis.pendingResourcesProjects = pendingResourcesProjects.FirstOrDefault();
        kpis.openProjects = openProjects;
        kpis.pendingProjects = pendingProjects;

        return kpis;
    }
}
