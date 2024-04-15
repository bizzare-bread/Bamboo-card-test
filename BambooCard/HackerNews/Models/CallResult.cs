using System.Net;

namespace HackerNews.Models;

public class CallResult<T>
{
    /// <summary>
    /// The data returned by the call
    /// </summary>
    public T Data { get; internal set; }

    /// <summary>
    /// An error code if the call didn't succeed
    /// </summary>
    public int Code { get; internal set; }

    /// <summary>
    /// An error message if the call didn't succeed
    /// </summary>
    public string Message { get; internal set; }

    /// <summary>
    /// Whether the call was successful
    /// </summary>
    public bool Success =>
        Code is (int)HttpStatusCode.OK or (int)HttpStatusCode.Created or (int)HttpStatusCode.Accepted;

    public CallResult(T data, int code, string message)
    {
        Data = data;
        Code = code;
        Message = message;
    }
}
