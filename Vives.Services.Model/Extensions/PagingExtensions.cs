using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vives.Services.Model.Extensions
{
    public static class PagingExtensions
    {
        public static IQueryable<T> AddPaging<T>(this IQueryable<T> query, Paging paging)
        {
            if (paging.StartIndex < 0)
            {
                paging.StartIndex = 0;
            }

            if (paging.PageSize <= 0)
            {
                paging.PageSize = 10;
            }

            if (paging.PageSize > 1000)
            {
                paging.PageSize = 1000;
            }

            query = query
                .Skip(paging.StartIndex)
                .Take(paging.PageSize);

            return query;
        }
    }
}
