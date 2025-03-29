using POS.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class QueryableExtensions
{
    public static async Task<PagedResultDto<T>> ToPagedResultAsync<T>(
        this IQueryable<T> source, int pageNumber, int pageSize) where T : class
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        var count = source.Count();

        var items = source.Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

        return new PagedResultDto<T>(items, count, pageNumber, pageSize);
    }
}
