namespace Treps.PaymentOrchestration.Sdk.Exceptions;

/// <summary>
/// Thrown for any non-2xx HTTP response, or any 2xx response whose body has <c>status: false</c>.
/// </summary>
public sealed class TrepsApiException : Exception
{
    public int HttpStatus { get; }

    public IReadOnlyList<string>? Errors { get; }

    public object? ResponseBody { get; }

    public TrepsApiException(string message, int httpStatus, IReadOnlyList<string>? errors, object? responseBody)
        : base(message)
    {
        HttpStatus = httpStatus;
        Errors = errors;
        ResponseBody = responseBody;
    }
}
