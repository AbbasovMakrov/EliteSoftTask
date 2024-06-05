namespace EliteSoftTask.Services.Utils;

public struct HttpResponse
{
    public uint StatusCode { get; init; }
    public string? Message { get; init; }
    public bool IsSuccess => StatusCode >= 200 && StatusCode <= 299;
}
public struct HttpResponse<TValue>
{
    public uint StatusCode { get; init; }
    public string? Message { get; init; }
    public TValue Value { get; init; }
    public bool IsSuccess => StatusCode >= 200 && StatusCode <= 299;
}