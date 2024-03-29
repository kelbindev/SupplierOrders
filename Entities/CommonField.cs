﻿namespace Entities;
public abstract class CommonField
{
    public DateOnly CreatedDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public string CreatedBy { get; set; }
    public DateOnly UpdatedDate { get; set; }
    public string UpdatedBy { get; set; }
}
