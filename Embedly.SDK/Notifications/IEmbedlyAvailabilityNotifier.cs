using System;

namespace Embedly.SDK.Notifications;

/// <summary>
///     Receives notifications from the SDK's HTTP circuit breaker when the upstream provider's
///     availability state changes. Apps register a singleton implementation in DI to surface
///     these transitions (Slack alerts, PagerDuty, etc.); the SDK falls back to a no-op
///     implementation when none is registered.
///     <para>
///         IMPORTANT: <see cref="OnCircuitBreak" /> fires on EVERY break transition. During a
///         sustained outage, Polly cycles Open ↔ HalfOpen every <c>durationOfBreak</c> seconds,
///         so OnCircuitBreak is invoked repeatedly. Implementations that emit external alerts
///         MUST deduplicate state transitions themselves — typical pattern is a Healthy/Outage
///         state machine that alerts only on the first break of a Healthy state and on the
///         first reset of an Outage state, with optional periodic "still down" reminders.
///     </para>
/// </summary>
public interface IEmbedlyAvailabilityNotifier
{
    /// <summary>
    ///     Invoked when the circuit transitions from Closed/HalfOpen to Open.
    /// </summary>
    void OnCircuitBreak(EmbedlyCircuitBreakInfo info);

    /// <summary>
    ///     Invoked when the circuit transitions from HalfOpen to Closed (recovery).
    /// </summary>
    void OnCircuitReset();

    /// <summary>
    ///     Invoked when the circuit transitions from Open to HalfOpen (about to probe). Most
    ///     implementations should treat this as a no-op — it does not represent a state change
    ///     that operators need to be alerted on.
    /// </summary>
    void OnCircuitHalfOpen();
}

/// <summary>
///     Details of the failure that tripped the circuit. Either <see cref="Exception" /> is
///     non-null (transport failure, timeout) or <see cref="StatusCode" /> is non-null (HTTP
///     5xx response handled as transient).
/// </summary>
public sealed record EmbedlyCircuitBreakInfo(
    Exception? Exception,
    int? StatusCode,
    TimeSpan BreakDuration);
