using Commitments.Shared;
using Commitments.Shared.Realtime;

namespace Commitments.Api.Realtime;

public sealed record NoteSavedPayload(Guid NoteId, string Title, string Kind);
public sealed record NoteRemovedPayload(Guid NoteId);
public sealed record TagSavedPayload(Guid TagId, string Name);
public sealed record TagRemovedPayload(Guid TagId);

public sealed class NoteTagRealtimeNotifier : IHostedService
{
    private readonly IEventBus _bus;
    private readonly IRealtimePublisher _publisher;
    private readonly ILogger<NoteTagRealtimeNotifier> _log;

    public NoteTagRealtimeNotifier(
        IEventBus bus,
        IRealtimePublisher publisher,
        ILogger<NoteTagRealtimeNotifier> log)
    {
        _bus = bus;
        _publisher = publisher;
        _log = log;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _bus.SubscribeAsync<NoteSavedEvent>(HandleAsync, cancellationToken);
        await _bus.SubscribeAsync<NoteRemovedEvent>(HandleAsync, cancellationToken);
        await _bus.SubscribeAsync<TagSavedEvent>(HandleAsync, cancellationToken);
        await _bus.SubscribeAsync<TagRemovedEvent>(HandleAsync, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public Task HandleAsync(NoteSavedEvent evt) =>
        _publisher.PublishToProfileAsync(evt.ProfileId, "noteSaved",
            new NoteSavedPayload(evt.NoteId, evt.Title, evt.Kind.ToString()));

    public Task HandleAsync(NoteRemovedEvent evt) =>
        _publisher.PublishToProfileAsync(evt.ProfileId, "noteRemoved",
            new NoteRemovedPayload(evt.NoteId));

    public Task HandleAsync(TagSavedEvent evt) =>
        _publisher.PublishToProfileAsync(evt.ProfileId, "tagSaved",
            new TagSavedPayload(evt.TagId, evt.Name));

    public Task HandleAsync(TagRemovedEvent evt) =>
        _publisher.PublishToProfileAsync(evt.ProfileId, "tagRemoved",
            new TagRemovedPayload(evt.TagId));
}
