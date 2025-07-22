using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models
{
    public class AuthenticationResult:ValidationResult
    {
        public User? AuthenticatedUser { get; set; }
    }
}
