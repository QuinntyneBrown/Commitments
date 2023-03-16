using System;


namespace Commitments.Core.Interfaces;

public interface ILoggable
{
   DateTime CreatedOn { get; set; }
   DateTime LastModifiedOn { get; set; }
   bool IsDeleted { get; set; }
}
