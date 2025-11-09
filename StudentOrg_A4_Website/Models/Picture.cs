using System;
using System.Collections.Generic;

namespace StudentOrg_A4_Website.Models;

public partial class Picture
{
    public int PictureId { get; set; }

    public string Location { get; set; } = null!;

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
