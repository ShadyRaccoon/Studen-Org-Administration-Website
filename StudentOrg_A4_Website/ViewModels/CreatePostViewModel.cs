using System.ComponentModel.DataAnnotations;

namespace StudentOrg_A4_Website.ViewModels
{
    public class CreatePostViewModel
    {
        public int PostId { get; set; }

        public string? PostAuthor { get; set; }

        [Required(ErrorMessage = "The post needs a thumbnail.")]
        [Display(Name = "ID of Google Drive Item")]
        public string PostBanner { get; set; } = null!;

        [Required(ErrorMessage = "The post needs a title.")]
        [StringLength(500, ErrorMessage = "The title of the post must not exceed 500 characters.")]
        [Display(Name = "Post Title")]
        public string PostTitle { get; set; } = null!;

        [Required(ErrorMessage = "The post needs a description.")]
        [StringLength(500, ErrorMessage = "The description of the post must not exceed 500 characters.")]
        [Display(Name = "Post Description")]
        public string PostDescription { get; set; } = null!;

        [Required(ErrorMessage = "The post needs content.")]
        [Display(Name = "Post Content")]
        public string PostContent { get; set; } = null!;
    }
}
