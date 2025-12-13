

namespace Identity.Infrastructure;
using Microsoft.AspNetCore.Identity;

public  class ApiUser:IdentityUser
{
    public ICollection<RefreshToken> RefreshToken = new List<RefreshToken>();
}
