using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortalBack.Application.Common.Contracts.Responses;

public sealed class PagedResponse<T> : ApiResponse<IReadOnlyCollection<T>>
{
    public int PageNumber { get; init; }

    public int PageSize { get; init; }

    public int TotalRecords { get; init; }

    public int TotalPages { get; init; }
}
