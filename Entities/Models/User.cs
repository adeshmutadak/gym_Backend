using System;
using System.Collections.Generic;

namespace gym_application.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? Role { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Gender { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? PasswordHash { get; set; }

    public int? MustResetPassword { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Client? Client { get; set; }

    public virtual Trainer? Trainer { get; set; }
}
