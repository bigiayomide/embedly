namespace Embedly.SDK.Notifications;

/// <summary>
///     Default no-op notifier. Registered by the SDK via <see cref="Microsoft.Extensions.DependencyInjection.Extensions.ServiceCollectionDescriptorExtensions.TryAddSingleton" />,
///     so apps that don't provide their own implementation get silent behaviour.
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
