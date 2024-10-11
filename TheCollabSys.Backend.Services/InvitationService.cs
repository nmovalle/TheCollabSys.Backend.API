using Microsoft.EntityFrameworkCore;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Enums;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public class InvitationService : IInvitationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapperService<InvitationDTO, DdInvitation> _mapperService;

    public InvitationService(IUnitOfWork unitOfWork, IMapperService<InvitationDTO, DdInvitation> mapperService)
    {
        _unitOfWork = unitOfWork;
        _mapperService = mapperService;
    }

    public IEnumerable<InvitationModelDTO> GetAll(int companyId)
    {
        var resp = (from i in _unitOfWork.InvitationRepository.GetAllQueryable()
                    join w in _unitOfWork.WireListRepository.GetAllQueryable() on i.Email equals w.Email
                    join r in _unitOfWork.RoleRepository.GetAllQueryable() on w.RoleId.ToString() equals r.Id
                    where i.CompanyId == companyId
                    group new { i, w, r } by new { i.Id, i.Email, i.Token, i.ExpirationDate, i.Status, i.Permissions, i.IsExternal, w.Domain, w.RoleId, r.Name, w.IsBlackList, i.CompanyId, i.UserId } into grouped
                    select new
                    {
                        grouped.Key.Id,
                        grouped.Key.Email,
                        grouped.Key.Token,
                        grouped.Key.ExpirationDate,
                        grouped.Key.Status,
                        grouped.Key.Permissions,
                        grouped.Key.IsExternal,
                        grouped.Key.Domain,
                        grouped.Key.RoleId,
                        grouped.Key.Name,
                        grouped.Key.IsBlackList,
                        grouped.Key.CompanyId,
                        grouped.Key.UserId,
                    }).AsEnumerable()
                    .Select(grouped => new InvitationModelDTO
                    {
                        Id = grouped.Id,
                        Email = grouped.Email,
                        Token = grouped.Token,
                        ExpirationDate = grouped.ExpirationDate,
                        Status = grouped.Status,
                        StatusName = Enum.GetName(typeof(InvitationStatus), grouped.Status),
                        Permissions = grouped.Permissions,
                        IsExternal = grouped.IsExternal,
                        Domain = grouped.Domain,
                        RoleId = grouped.RoleId,
                        RoleName = grouped.Name,
                        IsBlackList = grouped.IsBlackList,
                        CompanyId = grouped.CompanyId,
                        UserId = grouped.UserId,
                    })
                    .ToList();

        return resp;
    }

    public async Task<InvitationModelDTO?> GetByIdAsync(int id, int companyId)
    {
        var resp = await (from i in _unitOfWork.InvitationRepository.GetAllQueryable()
                          join w in _unitOfWork.WireListRepository.GetAllQueryable() on i.Email equals w.Email
                          join r in _unitOfWork.RoleRepository.GetAllQueryable() on w.RoleId.ToString() equals r.Id
                          where i.Id == id && i.CompanyId == companyId
                          group new { i, w, r } by new { i.Id, i.Email, i.Token, i.ExpirationDate, i.Status, i.Permissions, i.IsExternal, w.Domain, w.RoleId, r.Name, w.IsBlackList, i.CompanyId, i.UserId } into grouped
                          select new InvitationModelDTO
                          {
                              Id = grouped.Key.Id,
                              Email = grouped.Key.Email,
                              Token = grouped.Key.Token,
                              ExpirationDate = grouped.Key.ExpirationDate,
                              Status = grouped.Key.Status,
                              Permissions = grouped.Key.Permissions,
                              IsExternal = grouped.Key.IsExternal,
                              Domain = grouped.Key.Domain,
                              RoleId = grouped.Key.RoleId,
                              RoleName = grouped.Key.Name,
                              IsBlackList = grouped.Key.IsBlackList,
                              CompanyId = grouped.Key.CompanyId,
                              UserId = grouped.Key.UserId,
                          }).FirstOrDefaultAsync();

        if (resp != null)
        {
            resp.StatusName = Enum.GetName(typeof(InvitationStatus), resp.Status);
        }

        return resp;
    }

    public async Task<DdInvitation> Create(DdInvitation entity)
    {
        _unitOfWork.InvitationRepository.Add(entity);
        await _unitOfWork.CompleteAsync();
        return entity;
    }

    public async Task Update(int id, InvitationDTO dto)
    {
        var existing = await _unitOfWork.InvitationRepository.GetByIdAsync(id);
        if (existing == null)
            throw new ArgumentException("invitation not found");

        //dto.DateUpdate = DateTime.Now;
        //var excludeProperties = new List<string> { "DateCreated" };
        _mapperService.Map(dto, existing);

        _unitOfWork.InvitationRepository.Update(existing);

        await _unitOfWork.CompleteAsync();
    }

    public async Task Delete(int id)
    {
        var entity = await _unitOfWork.InvitationRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new ArgumentException("invitation not found");
        }

        _unitOfWork.InvitationRepository.Remove(entity);
        await _unitOfWork.CompleteAsync();
    }

    public async Task<InvitationDTO?> GetByToken(Guid token)
    {
        var tokenEntity = await _unitOfWork.InvitationRepository.GetByToken(token);
        if (tokenEntity == null)
            return null;

        var tokenDTO = _mapperService.MapToSource(tokenEntity);
        return tokenDTO;
    }
    public async Task<InvitationDTO?> GetByEmail(string email)
    {
        var tokenEntity = await _unitOfWork.InvitationRepository.GetByEmail(email);
        if (tokenEntity == null)
            return null;

        var tokenDTO = _mapperService.MapToSource(tokenEntity);
        return tokenDTO;
    }
}
