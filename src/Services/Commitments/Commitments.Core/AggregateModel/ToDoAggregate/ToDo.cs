// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.ComponentModel.DataAnnotations.Schema;
using Commitments.Core.AggregateModel.ProfileAggregate;

namespace Commitments.Core.AggregateModel.ToDoAggregate;

public class ToDo : BaseEntity
{
    public Guid ToDoId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    [ForeignKey("Profile")]
    public Guid ProfileId { get; set; }

    public DateTime DueOn { get; set; }

    public DateTime? CompletedOn { get; set; }

    public Profile Profile { get; set; }
}

