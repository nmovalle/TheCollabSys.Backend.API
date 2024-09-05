using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Repositories;

public class UserCompanyRepository : Repository<DdUserCompany>, IUserCompanyRepository
{
    public UserCompanyRepository(TheCollabsysContext context) : base(context)
    {
    }
}