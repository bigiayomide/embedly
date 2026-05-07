namespace Embedly.SDK.Notifications;

/// <summary>
///     Default no-op notifier. The SDK registers this via <c>TryAddSingleton</c> so apps that
///     don't provide their own <see cref="IEmbedlyAvailabilityNotifier" /> implementation get
///     silent behaviour.
/// </summary>
internal sealed class NullEmbedlyAvailabilityNotifier : IEmbedlyAvailabilityNotifier
{
    public void OnCircuitBreak(EmbedlyCircuitBreakInfo info)
    {
    }

    public void OnCircuitReset()
    {
    }

    public void OnCircuitHalfOpen()
    {
    }
}
