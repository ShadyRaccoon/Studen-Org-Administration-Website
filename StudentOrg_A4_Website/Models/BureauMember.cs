using System;
using System.Collections.Generic;

namespace StudentOrg_A4_Website.Models;

public partial class BureauMember
{
    public int MemberId { get; set; }

    public int PositionId { get; set; }

    public DateOnly StartTermDate { get; set; }

    public DateOnly? EndTermDate { get; set; }

    public virtual Member? Member { get; set; }

    public virtual BureauPosition? Position { get; set; }
}
