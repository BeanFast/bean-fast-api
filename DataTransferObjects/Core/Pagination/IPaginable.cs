namespace Pos_System.Domain.Paginate;

public interface IPaginable<TResult>
{
	int Size { get; }
	int Page { get; }
	int Total { get; }
	int TotalPages { get; }
	IList<TResult> Items { get; }
}