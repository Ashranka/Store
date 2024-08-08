using Microsoft.EntityFrameworkCore;

namespace Store.RequestHelpers
{
    public class PageList<T> : List<T>
    {
        public PageList(List<T> items, int count, int pageNumber, int pageSize)
        {
            Metadata = new MetaData
            {
                TotalCount = count,
                CurrentPage = pageSize,
                PageSize = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)

            };

            AddRange(items);
        }
        public MetaData Metadata { get; set; }

        public static async Task<PageList<T>> TopageList(IQueryable<T> query, int pageSize, int pageNumber)
        {
            var count = await query.CountAsync();
            var items = await query.Skip((pageNumber-1) * pageSize).Take(pageSize).ToListAsync();
            return new PageList<T>(items, count , pageNumber,pageSize);
        }
    }
}
