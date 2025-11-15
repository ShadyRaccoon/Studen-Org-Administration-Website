using System.ComponentModel.DataAnnotations;

namespace StudentOrg_A4_Website.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username or E-Mail required!")]
        public string UsernameOrEmail { get; set; }
        [Required(ErrorMessage = "Password required!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
