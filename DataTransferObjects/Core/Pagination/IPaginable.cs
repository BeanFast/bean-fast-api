namespace DataTransferObjects.Core.Pagination;

public interface IPaginable<TResult>
{
	int Size { get; }
	int Page { get; }
	int Total { get; }
	int TotalPages { get; }
	
}