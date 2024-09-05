using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Repositories;

public class CompanyRepository : Repository<DdCompany>, ICompanyRepository
{
    public CompanyRepository(TheCollabsysContext context) : base(context)
    {
    }
}