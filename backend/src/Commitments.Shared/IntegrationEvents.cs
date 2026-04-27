// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Commitments.Shared;

public class ProfileCreatedEvent : IntegrationEvent
{
    public Guid ProfileId { get; set; }
}

public class ProfileDeletedEvent : IntegrationEvent
{
    public Guid ProfileId { get; set; }
}

public class UserCreatedEvent : IntegrationEvent
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = null!;
}

public class ActivityRecordedEvent : IntegrationEvent
{
    public Guid ActivityId { get; set; }
    public Guid BehaviourId { get; set; }
    public Guid ProfileId { get; set; }
    public DateTimeOffset PerformedOn { get; set; }
    public ActivityChangeReason Reason { get; set; }
}

public enum ActivityChangeReason
{
    Created,
    Updated,
    Deleted
}

public class CommitmentChangedEvent : IntegrationEvent
{
    public Guid CommitmentId { get; set; }
    public Guid ProfileId { get; set; }
    public ChangeKind Kind { get; set; }
}

public class FrequencyChangedEvent : IntegrationEvent
{
    public Guid CommitmentId { get; set; }
    public Guid ProfileId { get; set; }
}

public class ToDoChangedEvent : IntegrationEvent
{
    public Guid ToDoId { get; set; }
    public Guid ProfileId { get; set; }
    public ChangeKind Kind { get; set; }
}

public enum ChangeKind
{
    Created,
    Updated,
    Removed,
    Completed
}

public class NoteSavedEvent : IntegrationEvent
{
    public Guid NoteId { get; set; }
    public Guid ProfileId { get; set; }
    public string Title { get; set; } = null!;
    public ChangeKind Kind { get; set; }
}

public class NoteRemovedEvent : IntegrationEvent
{
    public Guid NoteId { get; set; }
    public Guid ProfileId { get; set; }
}

public class TagSavedEvent : IntegrationEvent
{
    public Guid TagId { get; set; }
    public Guid ProfileId { get; set; }
    public string Name { get; set; } = null!;
}

public class TagRemovedEvent : IntegrationEvent
{
    public Guid TagId { get; set; }
    public Guid ProfileId { get; set; }
}
