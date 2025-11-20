using Microsoft.EntityFrameworkCore;
using StudentOrg_A4_Website.Data;
using System.Threading.Tasks;

namespace StudentOrg_A4_Website.Services
{
    public class UserServices
    {
        private readonly StudentOrgContext _context;

        public UserServices(StudentOrgContext context   )
        {
            _context = context;
        }

        public async Task<Member> FindById(int id) 
        {
            var member = await _context.Members
                .Where(m => m.MemberId == id)
                .FirstOrDefaultAsync();

            return member;
        }

        public async Task<Member> FindByName(string firstName, string lastName)
        { 
            var member = await _context.Members
                .Where( m =>
                    m.FirstName.ToLower().Trim() == firstName.ToLower().Trim() 
                &&  m.LastName.ToLower().Trim() == lastName.ToLower().Trim())
                .FirstOrDefaultAsync();

            return member;
        }

    }
}
