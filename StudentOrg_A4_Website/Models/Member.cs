using StudentOrg_A4_Website.Models;
using System;
using System.Collections.Generic;

namespace StudentOrg_A4_Website;

public partial class Member
{
    public int MemberId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Faculty { get; set; } = null!;

    public DateOnly JoinDate { get; set; }

    public DateOnly? LeaveDate { get; set; }

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();
}
