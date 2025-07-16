using CMS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models
{
    public class User
    {
        public string? Name { get; set; } = string.Empty;
        public string? Surname { get; set; } = string.Empty;
        public string? Username { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Visitor;

        public User()
        {
        }

        public User(string? name, string? surname, string? username, string? password, UserRole role)
        {
            Name = name;
            Surname = surname;
            Username = username;
            Password = password;
            Role = role;
        }

        public override string? ToString()
        {
            return $"Person: \nName: {Name}\tSurname: {Surname}";
        }
    }
}
