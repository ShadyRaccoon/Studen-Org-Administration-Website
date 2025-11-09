using System;
using System.Collections.Generic;

namespace StudentOrg_A4_Website.Models;

public partial class BureauPosition
{
    public int PositionId { get; set; }

    public string PositionName { get; set; } = null!;

    public string PositionAlias { get; set; } = null!;
}
