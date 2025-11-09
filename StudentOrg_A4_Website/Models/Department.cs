using System;
using System.Collections.Generic;

namespace StudentOrg_A4_Website.Models;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = null!;

    public string DepartmentAlias { get; set; } = null!;

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}
