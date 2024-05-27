using System;
using System.Collections.Generic;

namespace FitnessApp.Models;

public partial class TblTodo
{
    public int Id { get; set; }

    public string? ChallengeName { get; set; }

    public string? Category { get; set; }

    public string? DifficultyLevel { get; set; }

    public int Period { get; set; }

    public bool? IsDeleted { get; set; }

    public string? UserId { get; set; }
}
