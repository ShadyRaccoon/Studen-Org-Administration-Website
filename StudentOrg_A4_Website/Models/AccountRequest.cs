using System;
using System.Collections.Generic;

namespace StudentOrg_A4_Website.Models;

public partial class AccountRequest
{
    public int RequestId { get; set; }

    public DateOnly RequestDate { get; set; }

    public string RequestStatus { get; set; } = null!;

    public string RequestAuthor { get; set; } = null!;

    public string RequestAuthorRole { get; set; } = null!;

    public string? RequestDescription { get; set; }

    public string RequestedFirstName { get; set; } = null!;

    public string RequestedLastName { get; set; } = null!;
}
