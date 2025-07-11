using AutoMapper;
using Entities.Users;
using WebFramework.Api;

namespace Kolbeh.Api.Models
{

    public class UserDTO : BaseDto<UserDTO, User, Guid>
    {
        public UserDTO()
        {

        }
        private UserDTO(
            Guid id,
            string fullName,
            bool isActive,
            string phoneNumber,
            string email,
            bool emailConfirmed,
            RoleDTO[] roles,
            string userName
           )
        {
            Id = id;
            FullName = fullName;
            IsActive = isActive;
            PhoneNumber = phoneNumber;
            Email = email;
            EmailConfirmed = emailConfirmed;
            Roles = roles;
            UserName = userName;

        }
        public string PhoneNumber { get; set; }

        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public RoleDTO[] Roles { get; set; }
        public DateTime ExpirationDate { get; set; }

        public bool HaveSubscription { get; set; }
        public string FullName { get; internal set; }
        public decimal WalletCredit { get; set; }
        public string ShabaNo { get; internal set; }
        public string IntroduceCode { get; internal set; }
        public bool WantsNotifications { get; internal set; }

        public string Email { get; internal set; }
        public bool EmailConfirmed { get; internal set; }
        public int TotalOfficerInsurancePapersCount { get; internal set; }
        public int TotalPayPandingOfficerInsurancePapersCount { get; internal set; }

        internal static UserDTO Create
            (
           Guid id,
            string fullName,
            bool isActive,
            string phoneNumber,
            string email,
            bool emailConfirmed,
            RoleDTO[] roles,
            string userName
            )
                  => new(
                id,
                fullName,
                isActive,
                phoneNumber,
                email,
                emailConfirmed,
                roles,
                userName
                );
        public override void CustomMappings(IMappingExpression<User, UserDTO> mapping)
        {
            mapping.ForMember(
                dest => dest.Roles,
                config => config.MapFrom(src => src.UserRoles.Select(ur => new RoleDTO
                {
                    Name = ur.Role.Name,
                    Description = ur.Role.Description
                })));
        }
    }
    ///
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
