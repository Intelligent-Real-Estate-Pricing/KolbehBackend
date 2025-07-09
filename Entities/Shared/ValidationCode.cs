using Entities.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.Shared;

public class ValidationCode : BaseEntity<Guid>
{
    [Required]
    [MaxLength(11)]
    public string PhoneNumber { get; set; } = default!;

    [Required]
    [MaxLength(6)]
    public string Code { get; set; } = default!;

    public DateTime CreatedAt { get; set; }

    public DateTime ExpireAt { get; set; }

    public int TryCount { get; set; } = 0;

    public bool IsValid { get; set; } = true;

}