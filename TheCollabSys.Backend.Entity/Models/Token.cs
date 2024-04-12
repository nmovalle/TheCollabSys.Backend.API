using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCollabSys.Backend.Entity.Models;

public class Token
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string RefreshToken { get; set; }
    public bool IsRevoked { get; set; }
}
