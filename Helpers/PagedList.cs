using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            // total nr of pages / pages f.x for 13 users and page size 5 13/5 = 2. something = 3 pages
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.AddRange(items);
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            // for 13 users and page size of 5 so for page 2 we want users from 6 - 10
            
            // so for page 1 we skip (1 - 1) * 5 so take just first 5
            // so for page 2 we (2 - 1) * 5 so skip first 5 and take next five
            // so for page 3 we (3 - 1) * 5 so skip first 10 and take next five
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            // we return filtered list of users
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
