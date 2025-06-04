using Services;

namespace Kolbeh.Api.Models
{
    public class TokenResponseDTO
    {
        public UserDTO User { get; set; }
        public AccessToken AccessToken { get; set; }
    }
}
