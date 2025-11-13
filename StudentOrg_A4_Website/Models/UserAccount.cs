using Microsoft.AspNetCore.Identity;

namespace StudentOrg_A4_Website.Models
{
    public class UserAccount : IdentityUser
    {
        public int? MemberId { get; set; }
        public Member? Member { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? TerminationDate { get; set; }
    }
}
