﻿namespace TrackIt.Core.Models
{
    public interface IAuditableEntity
    {
        string? CreatedBy { get; set; }
        string? UpdatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime UpdatedDate { get; set; }
    }
}
