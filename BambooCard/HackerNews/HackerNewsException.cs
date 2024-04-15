using System.Net;

namespace HackerNews;

public class HackerNewsException : Exception
{
    public HttpStatusCode Code { get; set; }
    public string ErrorMessage { get; set; }

    public HackerNewsException(HttpStatusCode code, string errorMessage) : base(
        $"Error response from Hacker News: [{code}] {errorMessage}")
    {
        Code = code;
        ErrorMessage = errorMessage;
    }
}