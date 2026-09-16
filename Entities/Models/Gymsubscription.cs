using System;
using System.Collections.Generic;

namespace gym_application.Models;

public partial class Gymsubscription
{
    public int SubscriptionId { get; set; }

    public string? PlanName { get; set; }

    public string? PlanType { get; set; }

    public int? DurationDays { get; set; }

    public decimal? Price { get; set; }

    public int? IncludesPt { get; set; }

    public int? PtSessionsWeek { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();
}
