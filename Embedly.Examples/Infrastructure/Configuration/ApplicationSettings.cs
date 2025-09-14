namespace Embedly.Examples.Infrastructure.Configuration;

/// <summary>
///     Application-wide configuration settings.
/// </summary>
public class ApplicationSettings
{
    public const string SectionName = "Application";

    public string Name { get; set; } = "Embedly SDK Examples";
    public string Version { get; set; } = "1.0.0";
    public string Environment { get; set; } = "Development";
    public int DefaultPageSize { get; set; } = 20;
    public int MaxPageSize { get; set; } = 100;
    public TimeSpan DefaultTimeout { get; set; } = TimeSpan.FromMinutes(2);
    public bool EnableDetailedErrors { get; set; } = true;
    public string[] AllowedOrigins { get; set; } = [];
}

/// <summary>
///     Embedly SDK specific configuration.
/// </summary>
public class EmbedlySettings
{
    public const string SectionName = "Embedly";

    public string ApiKey { get; set; } = string.Empty;
    public string OrganizationId { get; set; } = string.Empty;
    public string Environment { get; set; } = "Staging";
    public string BaseUrl { get; set; } = string.Empty;
    public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(2);
    public bool EnableLogging { get; set; } = true;
    public bool LogRequestBodies { get; set; } = false;
    public string WebhookSecret { get; set; } = string.Empty;
    public int MaxRetryAttempts { get; set; } = 3;
    public TimeSpan RetryDelay { get; set; } = TimeSpan.FromSeconds(1);

    /// <summary>
    ///     Validates the configuration and returns any validation errors.
    /// </summary>
    public IEnumerable<string> Validate()
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(ApiKey))
            errors.Add("ApiKey is required");

        if (string.IsNullOrWhiteSpace(OrganizationId))
            errors.Add("OrganizationId is required");

        if (!ApiKey.StartsWith("BSK-", StringComparison.OrdinalIgnoreCase))
            errors.Add("ApiKey must start with 'BSK-'");

        if (Timeout <= TimeSpan.Zero)
            errors.Add("Timeout must be greater than zero");

        if (MaxRetryAttempts < 0)
            errors.Add("MaxRetryAttempts cannot be negative");

        return errors;
    }
}

/// <summary>
///     Logging configuration settings.
/// </summary>
public class LoggingSettings
{
    public const string SectionName = "Logging";

    public string MinimumLevel { get; set; } = "Information";
    public bool EnableStructuredLogging { get; set; } = true;
    public bool EnablePerformanceLogging { get; set; } = true;
    public bool LogSensitiveData { get; set; } = false;
    public ConsoleLoggingSettings Console { get; set; } = new();
    public FileLoggingSettings File { get; set; } = new();
}

public class ConsoleLoggingSettings
{
    public bool Enabled { get; set; } = true;

    public string OutputTemplate { get; set; } =
        "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}";
}

public class FileLoggingSettings
{
    public bool Enabled { get; set; } = false;
    public string Path { get; set; } = "logs/app-.txt";
    public long FileSizeLimitBytes { get; set; } = 100 * 1024 * 1024; // 100MB
    public int RetainedFileCountLimit { get; set; } = 7;

    public string OutputTemplate { get; set; } =
        "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}";
}