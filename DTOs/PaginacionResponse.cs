namespace EventHub.Api.DTOs;
public class PaginacionResponse<T>
{
    public object Meta { get; set; } = default!;
    public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();
}
