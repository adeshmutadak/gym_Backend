using System;
using System.Collections.Generic;

namespace gym_application.Models;

public partial class Trainer
{
    public int TrainerId { get; set; }

    public int UserId { get; set; }

    public string? TrainerCode { get; set; }

    public string? Discipline { get; set; }

    public string? Certification { get; set; }

    public int? ExperienceYears { get; set; }

    public string? Bio { get; set; }

    public int? MaxClients { get; set; }

    public DateOnly? JoiningDate { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    public virtual User? User { get; set; }
}
