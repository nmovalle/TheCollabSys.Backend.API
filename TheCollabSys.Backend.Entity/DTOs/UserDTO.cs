﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCollabSys.Backend.Entity.DTOs;

public class UserDTO
{
    public string Id { get; set; } = null!;

    public string? UserName { get; set; }

    public string? Email { get; set; }
}
