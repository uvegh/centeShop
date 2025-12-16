

namespace Identity.Infrastructure;
using Microsoft.AspNetCore.Identity;

public  class ApiUser:IdentityUser
{
    public ICollection<RefreshToken> RefreshToken = new List<RefreshToken>();
    //public string Username { get; set; } = string.Empty;
    //public string Email { get; set; } = string.Empty;
    //public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}
