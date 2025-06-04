using Common.Roles;

namespace Application.Users.DTOs
{
    public class GetAllUserByTypeDto
    {
        public long Id { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; internal set; }
        public string Email { get; internal set; }
        public DateTime ExpirationDate { get; set; }

        public bool HaveSubscription { get; set; }
        public string NationalCode { get; internal set; }
        public bool IsActive { get; internal set; }
        public List<string> Roles{ get; internal set; }

    }
    public class GetAllUserByTypeQueryDto
    {
        public string SearchItem { get; set; }
        public UserType? Type { get; set; }
    }
}
