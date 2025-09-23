using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Embedly.SDK.Configuration;
using Embedly.SDK.Http;
using Embedly.SDK.Models.Responses.Common;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace Embedly.SDK.Tests.Testing;

/// <summary>
///     Base class for all unit tests providing common setup and utilities.
///     Follows the SDK pattern with proper service instantiation and dependency injection.
/// </summary>
[TestFixture]
public abstract class TestBase
{
    [SetUp]
    public virtual void SetUp()
    {
        // Setup mock HTTP client
        MockHttpClient = new Mock<IEmbedlyHttpClient>();

        // Setup test options
        TestOptions = new EmbedlyOptions
        {
            ApiKey = TestApiKey,
            OrganizationId = TestOrganizationId,
            Environment = EmbedlyEnvironment.Staging,
            EnableLogging = false,
            Timeout = TimeSpan.FromSeconds(30),
            RsaPublicKey = TestRsaPublicKey
        };

        // Setup mock options
        MockOptions = new Mock<IOptions<EmbedlyOptions>>();
        MockOptions.Setup(o => o.Value).Returns(TestOptions);

        OnSetUp();
    }

    [TearDown]
    public virtual void TearDown()
    {
        OnTearDown();
        MockHttpClient?.Reset();
        MockOptions?.Reset();
    }

    /// <summary>
    ///     Mock HTTP client for API calls.
    /// </summary>
    protected Mock<IEmbedlyHttpClient> MockHttpClient { get; private set; } = null!;

    /// <summary>
    ///     Mock options for configuration.
    /// </summary>
    protected Mock<IOptions<EmbedlyOptions>> MockOptions { get; private set; } = null!;

    /// <summary>
    ///     Standard test configuration options.
    /// </summary>
    protected EmbedlyOptions TestOptions { get; private set; } = null!;

    /// <summary>
    ///     Test organization ID for consistent testing.
    /// </summary>
    protected string TestOrganizationId => "test-org-12345";

    /// <summary>
    ///     Test API key for consistent testing.
    /// </summary>
    protected string TestApiKey => "test-api-key-12345";

    /// <summary>
    ///     Test RSA public key for PIN encryption testing.
    ///     This is a valid 2048-bit RSA public key in PEM format following Embedly's specification.
    ///     Generated for testing purposes only - in production, use the key provided by Embedly.
    /// </summary>
    protected string TestRsaPublicKey => @"-----BEGIN PUBLIC KEY-----
MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwmiv7kE9A5qR9B/qlAt2
1IiPD8Jiqrvhe4el0qpmzWVnAha2ciKfEjdWIzxFaQ74WOUcbdy/f175Zu88i2aB
EyvEncw+seLnYZzluruQ/B+QnF0hqnYTa9mkchlUMdsrp//9wCtQUnKyXmqjRStZ
YDKcn9IYSMCymmpO1QNWdRUlNA++InKjd35Y9tIUkIpI5WfFPGJTkqXAWPR0N+HD
Kb9TM+TB7U4/YueYAPCd2L+AkI1iKIJ9+K5/wDEblr0t4GLHCHam3OKwj/slRbg3
SKJKb6csl3HkxiU7I5il3yfpYdBJ22clBKJp6KAqSO2x3gaIxILI/Y50BuqNr9yy
nQIDAQAB
-----END PUBLIC KEY-----";

    /// <summary>
    ///     Override this method for additional setup in derived classes.
    /// </summary>
    protected virtual void OnSetUp()
    {
    }

    /// <summary>
    ///     Override this method for additional teardown in derived classes.
    /// </summary>
    protected virtual void OnTearDown()
    {
    }

    /// <summary>
    ///     Creates a test GUID for consistent testing.
    /// </summary>
    /// <param name="seed">Optional seed for deterministic GUIDs.</param>
    /// <returns>A test GUID.</returns>
    protected static Guid CreateTestGuid(int seed = 1)
    {
        var bytes = new byte[16];
        BitConverter.GetBytes(seed).CopyTo(bytes, 0);
        return new Guid(bytes);
    }

    /// <summary>
    ///     Creates a test timestamp for consistent testing.
    /// </summary>
    /// <param name="daysOffset">Days to offset from base date.</param>
    /// <returns>A test timestamp.</returns>
    protected static DateTimeOffset CreateTestTimestamp(int daysOffset = 0)
    {
        var baseDate = new DateTimeOffset(2024, 1, 1, 12, 0, 0, TimeSpan.Zero);
        return baseDate.AddDays(daysOffset);
    }

    /// <summary>
    ///     Generates test phone number with optional suffix.
    /// </summary>
    /// <param name="suffix">Optional suffix for uniqueness.</param>
    /// <returns>A test phone number.</returns>
    protected static string CreateTestPhoneNumber(string suffix = "0001")
    {
        return $"+234801234{suffix}";
    }

    /// <summary>
    ///     Generates test email address with optional prefix.
    /// </summary>
    /// <param name="prefix">Optional prefix for uniqueness.</param>
    /// <returns>A test email address.</returns>
    protected static string CreateTestEmail(string prefix = "test")
    {
        return $"{prefix}@embedly-test.com";
    }

    /// <summary>
    ///     Creates a test string ID for consistent testing.
    /// </summary>
    /// <param name="prefix">Optional prefix for the ID.</param>
    /// <param name="seed">Optional seed for deterministic IDs.</param>
    /// <returns>A test string ID.</returns>
    protected static string CreateTestStringId(string prefix = "test", int seed = 1)
    {
        return $"{prefix}-{seed:D6}";
    }

    /// <summary>
    ///     Creates a cryptographically-random long in [minInclusive, maxInclusive].
    /// </summary>
    /// <param name="minInclusive">Minimum inclusive range.</param>
    /// <param name="maxInclusive">Maximum inclusive range.</param>
    /// <returns>A cryptographically-random 8-digit long ID.</returns>
    protected static long CreateTestLongId(long minInclusive = 10_000_000, long maxInclusive = 99_000_000)
    {
        var rng = RandomNumberGenerator.Create();
        long range = maxInclusive - minInclusive + 1;

        var buffer = new byte[8];
        while (true)
        {
            rng.GetBytes(buffer);
            ulong sample = BitConverter.ToUInt64(buffer, 0);
            ulong max = ulong.MaxValue - ((ulong.MaxValue % (ulong)range) + 1);
            if (sample <= max)
            {
                return (long)(minInclusive + (long)(sample % (ulong)range));
            }
        }
    }

    /// <summary>
    ///     Creates a test paginated response.
    /// </summary>
    /// <typeparam name="T">The type of items in the response.</typeparam>
    /// <param name="items">The items for the response.</param>
    /// <param name="totalCount">Total count of items.</param>
    /// <param name="pageSize">Page size.</param>
    /// <param name="currentPage">Current page number.</param>
    /// <returns>A test paginated response.</returns>
    protected static PaginatedResponse<T> CreateTestPaginatedResponse<T>(
        List<T> items,
        int totalCount = 0,
        int pageSize = 10,
        int currentPage = 1)
    {
        var totalItems = totalCount > 0 ? totalCount : items.Count;
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        return new PaginatedResponse<T>
        {
            Items = items,
            Pagination = new PaginationInfo
            {
                Page = currentPage,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                HasNext = currentPage < totalPages,
                HasPrevious = currentPage > 1
            }
        };
    }
}