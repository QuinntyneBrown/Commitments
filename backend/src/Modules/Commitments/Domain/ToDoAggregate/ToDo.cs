// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Commitments.Domain.ToDoAggregate;

public class ToDo : BaseEntity
{
    public Guid ToDoId { get; set; }
    public Guid ProfileId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DueOn { get; set; }
    public DateTime? CompletedOn { get; set; }
    public DateTime? DeletedOn { get; set; }
}
