using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Embedly.SDK.Models.Requests.Utilities;
using Embedly.SDK.Models.Responses.Utilities;
using Embedly.SDK.Models.Responses.Common;
using Embedly.SDK.Services.Utilities;
using Embedly.SDK.Tests.Testing;

namespace Embedly.SDK.Tests.Services;

/// <summary>
/// Unit tests for UtilityService following SDK patterns.
/// Tests currency and country utility operations.
/// </summary>
[TestFixture]
public class UtilityServiceTests : ServiceTestBase
{
    private UtilityService _utilityService = null!;

    protected override void OnSetUp()
    {
        _utilityService = new UtilityService(MockHttpClient.Object, MockOptions.Object);
    }

    #region Currency Tests

    [Test]
    public async Task GetCurrenciesAsync_ReturnsListOfCurrencies()
    {
        // Arrange
        var expectedCurrencies = new List<Currency>
        {
            CreateTestCurrency("NGN", "Nigerian Naira"),
            CreateTestCurrency("USD", "US Dollar")
        };
        var apiResponse = CreateSuccessfulApiResponse(expectedCurrencies);

        MockHttpClient
            .Setup(x => x.GetAsync<List<Currency>>(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _utilityService.GetCurrenciesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Should().HaveCount(2);
    }

    [Test]
    public async Task CreateCurrencyAsync_WithValidRequest_ReturnsCreatedCurrency()
    {
        // Arrange
        var request = CreateValidCurrencyRequest();
        var expectedCurrency = CreateTestCurrency("EUR", "Euro");
        var apiResponse = CreateSuccessfulApiResponse(expectedCurrency);

        MockHttpClient
            .Setup(x => x.PostAsync<CreateCurrencyRequest, Currency>(
                It.IsAny<string>(),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _utilityService.CreateCurrencyAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Code.Should().Be("EUR");
    }

    [Test]
    public async Task GetCurrenciesAlternateAsync_ReturnsListOfCurrencies()
    {
        // Arrange
        var expectedCurrencies = new List<Currency>
        {
            CreateTestCurrency("NGN", "Nigerian Naira"),
            CreateTestCurrency("USD", "US Dollar")
        };
        var apiResponse = CreateSuccessfulApiResponse(expectedCurrencies);

        MockHttpClient
            .Setup(x => x.GetAsync<List<Currency>>(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _utilityService.GetCurrenciesAlternateAsync();

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Should().HaveCount(2);
    }

    #endregion

    #region Country Tests

    [Test]
    public async Task GetCountriesAsync_ReturnsListOfCountries()
    {
        // Arrange
        var expectedCountries = new List<Country>
        {
            CreateTestCountry("NG", "Nigeria"),
            CreateTestCountry("US", "United States")
        };
        var apiResponse = CreateSuccessfulApiResponse(expectedCountries);

        MockHttpClient
            .Setup(x => x.GetAsync<List<Country>>(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _utilityService.GetCountriesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Should().HaveCount(2);
    }

    [Test]
    public async Task GetCountriesAlternateAsync_ReturnsListOfCountries()
    {
        // Arrange
        var expectedCountries = new List<Country>
        {
            CreateTestCountry("NG", "Nigeria"),
            CreateTestCountry("US", "United States")
        };
        var apiResponse = CreateSuccessfulApiResponse(expectedCountries);

        MockHttpClient
            .Setup(x => x.GetAsync<List<Country>>(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _utilityService.GetCountriesAlternateAsync();

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Should().HaveCount(2);
    }

    [Test]
    public async Task CreateCountryAsync_WithValidRequest_ReturnsCreatedCountry()
    {
        // Arrange
        var request = CreateValidCountryRequest();
        var expectedCountry = CreateTestCountry("CA", "Canada");
        var apiResponse = CreateSuccessfulApiResponse(expectedCountry);

        MockHttpClient
            .Setup(x => x.PostAsync<CreateCountryRequest, Country>(
                It.IsAny<string>(),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _utilityService.CreateCountryAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Code.Should().Be("CA");
    }

    #endregion

    #region File Upload Tests

    [Test]
    public async Task UploadFileAsync_WithValidFile_ReturnsFileUploadResponse()
    {
        // Arrange
        var fileStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("test file content"));
        var fileName = "test.txt";
        var contentType = "text/plain";
        var expectedResponse = CreateTestFileUploadResponse();
        var apiResponse = CreateSuccessfulApiResponse(expectedResponse);

        MockHttpClient
            .Setup(x => x.PostAsync<FileUploadRequest, FileUploadResponse>(
                It.IsAny<string>(),
                It.IsAny<FileUploadRequest>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _utilityService.UploadFileAsync(fileStream, fileName, contentType);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Url.Should().NotBeNullOrEmpty();
    }

    #endregion

    #region Helper Methods

    private CreateCurrencyRequest CreateValidCurrencyRequest()
    {
        return new CreateCurrencyRequest
        {
            Code = "EUR",
            Name = "Euro",
            Symbol = "€",
            DecimalPlaces = 2
        };
    }

    private Currency CreateTestCurrency(string code, string name)
    {
        return new Currency
        {
            Code = code,
            Name = name,
            Symbol = code == "NGN" ? "₦" : "$",
            IsActive = true,
            DecimalPlaces = 2
        };
    }

    private CreateCountryRequest CreateValidCountryRequest()
    {
        return new CreateCountryRequest
        {
            Code = "CA",
            Name = "Canada",
            Code3 = "CAN",
            DialCode = "+1",
            IsActive = true
        };
    }

    private Country CreateTestCountry(string code, string name)
    {
        return new Country
        {
            Id = CreateTestGuid().ToString(),
            Code = code,
            Name = name,
            Code3 = code == "NG" ? "NGA" : "USA",
            DialCode = code == "NG" ? "+234" : "+1",
            IsActive = true
        };
    }

    private FileUploadResponse CreateTestFileUploadResponse()
    {
        return new FileUploadResponse
        {
            Url = "https://example.com/files/test-file-123.txt",
            FileId = "file-123",
            FileName = "test.txt",
            ContentType = "text/plain",
            FileSize = 1024
        };
    }

    #endregion
}