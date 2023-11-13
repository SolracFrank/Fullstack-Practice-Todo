using Microsoft.AspNetCore.Identity;

namespace Infrastructure.CustomEntities
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
     
    }
}
