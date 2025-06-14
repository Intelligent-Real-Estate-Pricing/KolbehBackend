using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public bool EmailConfirmed { get; set; }
        public AuthenticationMethod AuthenticationMethod { get; set; }
        public RoleDTO[] Roles { get; set; }
    }
    public class RoleDTO
    {
        private RoleDTO(string name, string description)
        {
            Name = name;
            Description = description;
        }
        public RoleDTO()
        {

        }
        public string Name { get; set; }
        public string Description { get; set; }
        internal static RoleDTO Create(string name, string description) => new(name, description);

    }
}
