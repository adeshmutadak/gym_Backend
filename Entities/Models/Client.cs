using System;
using System.Collections.Generic;

namespace gym_application.Models;

public partial class Client
{
    public int ClientId { get; set; }

    public int? UserId { get; set; }

    public string? ClientCode { get; set; }

    public int? SubscriptionId { get; set; }

    public string? PlanName { get; set; }

    public string? PlanType { get; set; }

    public int? DurationDays { get; set; }

    public decimal? PricePaid { get; set; }

    public int? IncludesPt { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string? MembershipStatus { get; set; }

    public int? TrainerId { get; set; }

    public DateOnly? AssignedOn { get; set; }

    public string? Goal { get; set; }

    public decimal? HeightCm { get; set; }

    public decimal? StartingWeightKg { get; set; }

    public decimal? TargetWeightKg { get; set; }

    public string? ActivityLevel { get; set; }

    public string? MedicalNotes { get; set; }

    public string? EmergencyContact { get; set; }

    public DateOnly? JoiningDate { get; set; }

    public string? Status { get; set; }

    public virtual Gymsubscription? Subscription { get; set; }

    public virtual Trainer? Trainer { get; set; }

    public virtual User? User { get; set; }
}
