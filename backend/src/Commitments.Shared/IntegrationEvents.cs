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
