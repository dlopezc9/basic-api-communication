using APIConnection.Models;
using APIConnection.Services;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;

public class PhoneServiceTests
{
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
    private readonly PhoneService _phoneService;
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly HttpClient _httpClient;

    public PhoneServiceTests()
    {
        // Arrange: Set up the mock HttpMessageHandler and HttpClient
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("https://api.restful-api.dev/")
        };

        // Arrange: Set up the mock IHttpClientFactory
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _httpClientFactoryMock.Setup(f => f.CreateClient("PhoneApi")).Returns(_httpClient);

        // Arrange: Instantiate the service with the mock dependencies
        _phoneService = new PhoneService(_httpClientFactoryMock.Object);
    }

    [Fact]
    public async Task GetPhones_ShouldReturnPhones_WhenApiCallIsSuccessful()
    {
        // Arrange: Set up a successful response from the mock handler
        var expectedPhones = new List<Phone>
        {
            new Phone { Id = 1, Name = "iPhone X"},
            new Phone { Id = 2, Name = "Galaxy S10"}
        };
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonConvert.SerializeObject(expectedPhones))
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        // Act: Call the GetPhones method
        var phones = await _phoneService.GetPhones();

        // Assert: Verify the returned phones match the expected result
        phones.Should().BeEquivalentTo(expectedPhones);
    }

    [Fact]
    public async Task GetPhones_ShouldReturnNull_WhenResponseBodyIsEmpty()
    {
        // Arrange: Set up a successful response with an empty body
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("")
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        // Act: Call the GetPhones method
        var phones = await _phoneService.GetPhones();

        // Assert: Verify that the result is null
        phones.Should().BeNull();
    }

    [Fact]
    public async Task GetPhones_ShouldThrowException_WhenApiCallIsUnsuccessful()
    {
        // Arrange: Set up an unsuccessful response (e.g., InternalServerError)
        var responseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        // Act: Call the GetPhones method and capture the exception
        Func<Task> act = async () => await _phoneService.GetPhones();

        // Assert: Verify that an exception is thrown with the expected message
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("The reached endpoint is not available at the moment, please try again later.");
    }

    [Fact]
    public async Task GetPhones_ShouldThrowException_WhenHttpRequestExceptionIsThrown()
    {
        // Arrange: Simulate an HttpRequestException being thrown
        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException());

        // Act: Call the GetPhones method and capture the exception
        Func<Task> act = async () => await _phoneService.GetPhones();

        // Assert: Verify that an exception is thrown with the expected message
        await act.Should().ThrowAsync<HttpRequestException>();
    }

    [Fact]
    public async Task GetPhones_ShouldThrowException_WhenResponseStatusCodeIsBadRequest()
    {
        // Arrange: Set up a response with a BadRequest status code
        var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        // Act: Call the GetPhones method and capture the exception
        Func<Task> act = async () => await _phoneService.GetPhones();

        // Assert: Verify that an exception is thrown with the expected message
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("The reached endpoint is not available at the moment, please try again later.");
    }
}
