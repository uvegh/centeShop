using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure
{
    public  class RefreshToken
    {
        public int Id { get; set; }
        public string TokenHash { get; set; } = default!;
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }
        public bool Revoked { get; set; }
 
        public string DeviceId { get; set; } = default!;
    }
}
