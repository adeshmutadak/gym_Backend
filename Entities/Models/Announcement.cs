using System;
using System.Collections.Generic;

namespace gym_application.Models;

public partial class Announcement
{
    public int AnnouncementId { get; set; }

    public string? Title { get; set; }

    public string? Body { get; set; }

    public string? Audience { get; set; }

    public DateTime? VisibleFrom { get; set; }

    public DateTime? VisibleUntil { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }
}
