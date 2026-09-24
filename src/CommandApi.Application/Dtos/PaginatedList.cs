namespace CommandApi.Application.Dtos;

public record PaginatedList<T>
(IList<T> Items, int Count, int Index, int PageSize)
{
    public int TotalPages => (Count + PageSize - 1) / PageSize;

    public bool HasPreviousPage => PageSize > 1;

    public bool HasNextPage => Index < TotalPages;
};

public record PaginationParams
{
    public int PageIndex { get; init; }
    public int PageSize { get; init; }
}