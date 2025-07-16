using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models
{
    public class AuthenticationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsValidationError { get; set; }
        public User? AuthenticatedUser { get; set; }
    }
}
