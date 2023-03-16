using Commitments.Core.Interfaces;
using System;


namespace Commitments.Core.Entities;

public class BaseEntity: ILoggable
{
    public string Name { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime LastModifiedOn { get; set; }
    public bool IsDeleted { get; set; }
}
