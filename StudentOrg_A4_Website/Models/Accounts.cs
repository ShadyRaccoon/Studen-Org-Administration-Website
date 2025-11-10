using System;
using System.Collections.Generic;

namespace StudentOrg_A4_Website.Models;

public partial class Accounts
{
    public int AccountId { get; set; }

    public bool IsActive { get; set; }

    public int? MemberId { get; set; }

    public string AccountPassword { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public DateOnly CreationDate { get; set; }

    public DateOnly? TerminationDate { get; set; }

    public virtual Member? Member { get; set; }
}
