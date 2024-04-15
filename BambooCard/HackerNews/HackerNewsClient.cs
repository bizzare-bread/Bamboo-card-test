using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using HackerNews.Models;

namespace HackerNews;

public partial class HackerNewsClient : IDisposable, IHackerNewsClient
{
    // Should be another abstraction for different environments
    public const string HackerNewsApiUrl = "https://hacker-news.firebaseio.com/v0";

    #region Properties

    public bool ThrowThenErrorResponse { get; set; } = true;

    private HttpClient? _httpClient;
    private DateTime _lastHttpSetupTime;
    private HttpClient? _lastHttpClient;
    private readonly object _gate = new();
    
    #endregion
    
    #region Constructors

    // Space for improvements e.g. access token, dynamic data
    public HackerNewsClient()
    {
    }

    #endregion

    #region Private Methods

    private HttpClient GetHttpClient()
    {
        lock (_gate)
        {
            if (_httpClient == null || (DateTime.UtcNow - _lastHttpSetupTime).TotalSeconds > 120)
            {
                SetupHttpClient();
            }

            return _httpClient;
        }
    }

    private void SetupHttpClient()
    {
        var handler = new HttpClientHandler()
        {
            AllowAutoRedirect = false
        };

        var client = new HttpClient(handler);

        client.BaseAddress = new Uri(HackerNewsApiUrl);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        
        _lastHttpClient?.Dispose();
        _lastHttpClient = _httpClient;
        _httpClient = client;
        _lastHttpSetupTime = DateTime.UtcNow;
    }

    private async Task<CallResult<T>> GetAsync<T>(string url,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        var client = this.GetHttpClient();
        var response = await client.GetAsync($"{url}", cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        return ProcessResponse<T>(response, content);
    }
    
    private CallResult<T> ProcessResponse<T>(HttpResponseMessage response, string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            ThrowErrorExceptionIfEnabled(HttpStatusCode.NotFound, "Empty Response");
            return new CallResult<T>(default, (int)HttpStatusCode.NotFound, "Empty Response");
        }

        try
        {
            var errorMessage = response.StatusCode == HttpStatusCode.OK ? string.Empty : content;
            var obj = response.StatusCode == HttpStatusCode.OK ? JsonSerializer.Deserialize<T>(content) :  default;
            return new CallResult<T>(obj, (int)response.StatusCode, errorMessage);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Cannot ProcessResponse for {nameof(T)}; Content: {content}");
            throw;
        }
    }

    private void ThrowErrorExceptionIfEnabled(HttpStatusCode code, string message)
    {
        if (ThrowThenErrorResponse)
        {
            throw new HackerNewsException(code, message);
        }
    }

    #endregion

    public void Dispose()
    {
        _httpClient?.Dispose();
        _lastHttpClient?.Dispose();
    }
}