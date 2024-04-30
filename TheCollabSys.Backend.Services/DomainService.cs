using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public class DomainService : IDomainService
{
    private readonly IUnitOfWork _unitOfWork;

    public DomainService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<DdDomainMaster> AddDomainMaster(DdDomainMaster domainMaster)
    {
        _unitOfWork.DomainRepository.Add(domainMaster);
        await _unitOfWork.CompleteAsync();
        return domainMaster;
    }

    public async Task<IEnumerable<DdDomainMaster>> GetAllDomainMastersAsync()
    {
        return await _unitOfWork.DomainRepository.GetAllAsync();
    }

    public async Task<DdDomainMaster?> GetDomainMasterByDomain(string domain)
    {
        return await _unitOfWork.DomainRepository.GetDomainMasterByDomain(domain);
    }

    public async Task<DdDomainMaster?> GetDomainMasterById(int id)
    {
        return await _unitOfWork.DomainRepository.GetByIdAsync(id);
    }
}
