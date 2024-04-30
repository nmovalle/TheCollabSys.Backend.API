using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCollabSys.Backend.Entity.Exceptions;

public class StoreNotFonudException : NotFoundException
{
    public StoreNotFonudException(int storeId) : base($"The store with the identifier {storeId} was not found.")
    {
    }
}
