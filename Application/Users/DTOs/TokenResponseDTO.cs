using Services;
namespace Application.Users.DTOs
{
    public class TokenResponseDTO
    {
        public AccessToken AccessToken { get; set; }
        public UserDTO User { get; set; }
    }
}
