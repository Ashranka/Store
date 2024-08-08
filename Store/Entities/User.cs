using Microsoft.AspNetCore.Identity;

namespace Store.Entities
{
    public class User : IdentityUser <int>
    {
        public UserAddress Address { get; set; }
    }
}
