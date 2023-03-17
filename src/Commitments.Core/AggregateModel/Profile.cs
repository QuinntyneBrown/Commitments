// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace Commitments.Core.AggregateModel;

public class Profile: BaseEntity
{        
    public int ProfileId { get; set; }           
    public string Name { get; set; }
    public string AvatarUrl { get; set; }
    public ICollection<Behaviour> Commitments { get; set; } = new HashSet<Behaviour>();        
    public User User { get; set; }
    [ForeignKey("User")]
    public int UserId { get; set; }
}

