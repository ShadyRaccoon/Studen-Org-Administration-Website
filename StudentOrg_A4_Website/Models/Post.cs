using System;
using System.Collections.Generic;

namespace StudentOrg_A4_Website.Models;

public partial class Post
{
    public int PostId { get; set; }

    public string PostAuthor { get; set; } = null!;

    public string PostBanner { get; set; } = null!;

    public string PostTitle { get; set; } = null!;

    public string PostDescription { get; set; } = null!;

    public string PostContent { get; set; } = null!;

    public DateOnly? PostDate { get; set; }

    public virtual ICollection<Picture> Pictures { get; set; } = new List<Picture>();
}
