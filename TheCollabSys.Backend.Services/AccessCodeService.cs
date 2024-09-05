using AutoMapper;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Data;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace TheCollabSys.Backend.Services;

public class AccessCodeService : IAccessCodeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapperService<AccessCodeDTO, DdAccessCode> _mapperService;

    public AccessCodeService(IUnitOfWork unitOfWork, IMapperService<AccessCodeDTO, DdAccessCode> mapperService)
    {
        _unitOfWork = unitOfWork;
        _mapperService = mapperService;
    }

    public IAsyncEnumerable<AccessCodeDTO> GetAll()
    {
        var data = _unitOfWork.AccessCodeRepsitory.GetAllQueryable()
            .Select(c => new AccessCodeDTO
            {
                Id = c.Id,
                AccessCode = c.AccessCode,
                Email = c.Email,
                RegAt = c.RegAt,
                ExpAt = c.ExpAt,
                IsValid = c.IsValid
            })
            .AsAsyncEnumerable();

        return data;
    }

    public async Task<AccessCodeDTO?> GetByIdAsync(int id)
    {
        var resp = await _unitOfWork.AccessCodeRepsitory.GetAllQueryable()
            .Where(c => c.Id == id)
            .Select(c => new AccessCodeDTO
            {
                Id = c.Id,
                AccessCode = c.AccessCode,
                Email = c.Email,
                RegAt = c.RegAt,
                ExpAt = c.ExpAt,
                IsValid = c.IsValid
            })
            .FirstOrDefaultAsync();

        return resp;
    }

    public async Task<DdAccessCode> Create(DdAccessCode entity)
    {
        _unitOfWork.AccessCodeRepsitory.Add(entity);
        await _unitOfWork.CompleteAsync();
        return entity;
    }

    public async Task Update(int id, AccessCodeDTO dto)
    {
        var existing = await _unitOfWork.AccessCodeRepsitory.GetByIdAsync(id);
        if (existing == null)
            throw new ArgumentException("access code not found");

        //dto.DateUpdate = DateTime.Now;
        //var excludeProperties = new List<string> { "DateCreated" };
        _mapperService.Map(dto, existing);

        _unitOfWork.AccessCodeRepsitory.Update(existing);

        await _unitOfWork.CompleteAsync();
    }

    public async Task Delete(int id)
    {
        var entity = await _unitOfWork.AccessCodeRepsitory.GetByIdAsync(id);
        if (entity == null)
        {
            throw new ArgumentException("access code not found");
        }

        _unitOfWork.AccessCodeRepsitory.Remove(entity);
        await _unitOfWork.CompleteAsync();
    }

    public async Task<AccessCodeDTO?> GetByEmail(string email)
    {
        var accessCode = await _unitOfWork.AccessCodeRepsitory.GetByEmail(email);
        if (accessCode == null)
            return null;

        var accessCodeDTO = _mapperService.MapToSource(accessCode);
        return accessCodeDTO;
    }

    public async Task<AccessCodeDTO?> GetByAccessCodeEmail(string accessCode, string email)
    {
        var existing = await _unitOfWork.AccessCodeRepsitory.GetByAccessCodeEmail(accessCode, email);
        if (existing == null)
            return null;

        var accessCodeDTO = _mapperService.MapToSource(existing);
        return accessCodeDTO;
    }
}
