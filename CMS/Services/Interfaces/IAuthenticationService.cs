using CMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public AuthenticationResult LoginSuccessful (string username, string password);
    }
}
