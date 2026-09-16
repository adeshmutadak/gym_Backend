using System;
using System.Collections.Generic;

namespace gym_application.Models;

public partial class Exercise
{
    public int ExerciseId { get; set; }

    public string? ExerciseName { get; set; }

    public string? MuscleGroup { get; set; }

    public string? Equipment { get; set; }

    public int? DefaultSets { get; set; }

    public string? DefaultReps { get; set; }

    public string? Instructions { get; set; }

    public string? VideoUrl { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }
}
