﻿using System;
using System.Collections.Generic;

namespace united_movers_api.Models;

public partial class UserRoleMapping
{
    public int MappingId { get; set; }

    public int UserId { get; set; }

    public int RoleId { get; set; }

    public DateTime? AssignedAt { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}