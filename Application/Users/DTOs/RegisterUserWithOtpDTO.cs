using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.DTOs
{
    public record RegisterUserWithOtpDTO(string PhoneNumber, string FullName, string OtpCode);
}
