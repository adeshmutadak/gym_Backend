using System;
using System.Collections.Generic;

namespace gym_application.Models;

public partial class Food
{
    public int FoodId { get; set; }

    public string? FoodName { get; set; }

    public string? Category { get; set; }

    public string? ServingUnit { get; set; }

    public decimal? ServingSize { get; set; }

    public decimal? Kcal { get; set; }

    public decimal? ProteinG { get; set; }

    public decimal? CarbsG { get; set; }

    public decimal? FatG { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }
}
