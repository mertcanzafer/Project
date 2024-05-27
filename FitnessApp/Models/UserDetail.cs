using System;
using System.Collections.Generic;

namespace FitnessApp.Models;

public partial class UserDetail
{
    public int Id { get; set; }

    public string? City { get; set; }

    public byte[]? Photo { get; set; }

    public string? Bio { get; set; }

    public string? UserId { get; set; }
}
