namespace TheCollabSys.Backend.Entity.Response;

public record Response<T>
{
    public string Status { get; init; }
    public T Data { get; init; }
    public string Message { get; init; }
}
