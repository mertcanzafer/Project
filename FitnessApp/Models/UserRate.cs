﻿using System;
using System.Collections.Generic;

namespace FitnessApp.Models;

public partial class UserRate
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public int? TodoId { get; set; }

    public short? Rate { get; set; }
}
