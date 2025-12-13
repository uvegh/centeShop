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
        public DateTime Expires { get; set; }
        public bool Revoked { get; set; }
        public string Token { get; set; } = default!;

    }
}
