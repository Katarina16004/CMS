using CMS.Models;
using CMS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services
{
    public class AuthenticationService:IAuthenticationService
    {
        private readonly XmlDataService<User> userDataService;
        private string filePath = "Data/Users.xml";

        public AuthenticationService()
        {
            userDataService = new XmlDataService<User>(filePath);
        }

        public AuthenticationResult LoginSuccessful(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return new AuthenticationResult
                {
                    Success = false,
                    IsValidationError = true,
                    Message = "Fill in both fields"
                };
            }

            var users = userDataService.LoadAll();
            var user = users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user == null)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    IsValidationError = false,
                    Message = "Invalid username or password!"
                };
            }

            return new AuthenticationResult
            {
                Success = true,
                Message = $"Welcome, {user.Name}!",
                AuthenticatedUser = user
            };
        }

    }
}
