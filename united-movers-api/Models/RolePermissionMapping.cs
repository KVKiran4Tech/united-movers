using System;
using System.Collections.Generic;

namespace united_movers_api.Models;

public partial class RolePermissionMapping
{
    public int MappingId { get; set; }

    public int RoleId { get; set; }

    public int PermissionId { get; set; }

    public virtual Permission Permission { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
