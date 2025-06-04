using Entities.Common;
using System;

namespace Entities.Shared;

public class ValidationCode : BaseEntity<Guid>
{
    public string PhoneNumber { get; set; }
    public string Code { get; set; }
    public const int ExpirationSeconds = 2 * 60;
    public bool IsValid => (DateTime.Now - CreatedAt).TotalSeconds < ExpirationSeconds;
}