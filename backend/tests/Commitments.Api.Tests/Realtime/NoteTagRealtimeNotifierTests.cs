using Commitments.Api.Realtime;
using Commitments.Shared;
using Commitments.Shared.Realtime;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Realtime;

public class NoteTagRealtimeNotifierTests
{
    private readonly Mock<IRealtimePublisher> _publisher = new();
    private readonly Mock<IEventBus> _bus = new();
    private readonly Guid _profileId = Guid.NewGuid();

    private NoteTagRealtimeNotifier CreateNotifier() =>
        new(_bus.Object, _publisher.Object,
            NullLogger<NoteTagRealtimeNotifier>.Instance);

    [Fact]
    public async Task NoteSavedEvent_PublishesNoteSavedEnvelope()
    {
        var notifier = CreateNotifier();
        var noteId = Guid.NewGuid();

        await notifier.HandleAsync(new NoteSavedEvent
        {
            NoteId = noteId,
            ProfileId = _profileId,
            Title = "Weekly review",
            Kind = ChangeKind.Updated
        });

        _publisher.Verify(p => p.PublishToProfileAsync(
            _profileId,
            "noteSaved",
            It.Is<NoteSavedPayload>(payload =>
                payload.NoteId == noteId &&
                payload.Title == "Weekly review" &&
                payload.Kind == "Updated"),
            It.IsAny<Guid?>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task NoteRemovedEvent_PublishesNoteRemovedEnvelope()
    {
        var notifier = CreateNotifier();
        var noteId = Guid.NewGuid();

        await notifier.HandleAsync(new NoteRemovedEvent { NoteId = noteId, ProfileId = _profileId });

        _publisher.Verify(p => p.PublishToProfileAsync(
            _profileId,
            "noteRemoved",
            It.Is<NoteRemovedPayload>(payload => payload.NoteId == noteId),
            It.IsAny<Guid?>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task TagSavedEvent_PublishesTagSavedEnvelope()
    {
        var notifier = CreateNotifier();
        var tagId = Guid.NewGuid();

        await notifier.HandleAsync(new TagSavedEvent
        {
            TagId = tagId,
            ProfileId = _profileId,
            Name = "weekly"
        });

        _publisher.Verify(p => p.PublishToProfileAsync(
            _profileId,
            "tagSaved",
            It.Is<TagSavedPayload>(payload =>
                payload.TagId == tagId && payload.Name == "weekly"),
            It.IsAny<Guid?>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task TagRemovedEvent_PublishesTagRemovedEnvelope()
    {
        var notifier = CreateNotifier();
        var tagId = Guid.NewGuid();

        await notifier.HandleAsync(new TagRemovedEvent { TagId = tagId, ProfileId = _profileId });

        _publisher.Verify(p => p.PublishToProfileAsync(
            _profileId,
            "tagRemoved",
            It.Is<TagRemovedPayload>(payload => payload.TagId == tagId),
            It.IsAny<Guid?>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task StartAsync_SubscribesToAllFourEvents()
    {
        var notifier = CreateNotifier();

        await notifier.StartAsync(CancellationToken.None);

        _bus.Verify(b => b.SubscribeAsync(It.IsAny<Func<NoteSavedEvent, Task>>(), It.IsAny<CancellationToken>()), Times.Once);
        _bus.Verify(b => b.SubscribeAsync(It.IsAny<Func<NoteRemovedEvent, Task>>(), It.IsAny<CancellationToken>()), Times.Once);
        _bus.Verify(b => b.SubscribeAsync(It.IsAny<Func<TagSavedEvent, Task>>(), It.IsAny<CancellationToken>()), Times.Once);
        _bus.Verify(b => b.SubscribeAsync(It.IsAny<Func<TagRemovedEvent, Task>>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
