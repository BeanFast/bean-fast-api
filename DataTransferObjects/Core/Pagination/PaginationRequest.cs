using Utilities.ValidationAttributes;

namespace DataTransferObjects.Core.Pagination;
[PaginationRequestValidatorAttribute]
public class PaginationRequest
{
    public int Page { get; set; }

    public int Size { get; set; }
}