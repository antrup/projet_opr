using BugTrackerAPI.Data.DTO;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace BugTrackerAPI.Data
{
    public class ApiResultGetter<T>
    {
        public ApiResult<T> apiresult;

        // Private constructor called by CreateAsync
        private ApiResultGetter(
            List<T> data,
            int count,
            int pageIndex,
            int pageSize,
            string? sortColumn,
            string? sortOrder)
        {
            apiresult = new ApiResult<T>(data, pageIndex, pageSize, count, (int)Math.Ceiling(count / (double)pageSize));
            apiresult.SortColumn = sortColumn;
            apiresult.SortOrder = sortOrder;
        }

        // allow 'async construction'
        public static async Task<ApiResult<T>> CreateAsync(
            IQueryable<T> source,
            int pageIndex,
            int pageSize,
            string? sortColumn = null,
            string? sortOrder = null)
        {
            var count = await source.CountAsync();

            if (!string.IsNullOrEmpty(sortColumn) && IsValidProperty(sortColumn))
            {
                // protection against SQL injection
                sortOrder = !string.IsNullOrEmpty(sortOrder) && sortOrder.ToUpper() == "ASC"
                    ? "ASC"
                    : "DESC";
                // sort data source
                source = source.OrderBy(string.Format("{0} {1}", sortColumn, sortOrder));
            }
            // paging feature
            source = source
                .Skip(pageIndex * pageSize)
                .Take(pageSize);

            var data = await source.ToListAsync();

            var ApiGetter = new ApiResultGetter<T>(
                data,
                count,
                pageIndex,
                pageSize,
                sortColumn,
                sortOrder);

            return ApiGetter.apiresult;
        }


        // Checks if the column name exists to protect against SQL injection attacks
        public static bool IsValidProperty(
            string propertyName,
            bool throwExceptionIfNotFound = true)
        {
            var prop = typeof(T).GetProperty(
                propertyName,
                BindingFlags.IgnoreCase |
                BindingFlags.Public |
                BindingFlags.Instance);
            if (prop == null && throwExceptionIfNotFound)
                throw new NotSupportedException(
                    string.Format(
                        $"ERROR: Property '{propertyName}' does not exist.")
                    );
            return prop != null;
        }


        // TRUE if the current page has a previous page, FALSE otherwise.
        public bool HasPreviousPage
        {
            get
            {
                return (apiresult.PageIndex > 0);
            }
        }

        // TRUE if the current page has a next page, FALSE otherwise.
        public bool HasNextPage
        {
            get
            {
                return ((apiresult.PageIndex + 1) < apiresult.TotalPages);
            }
        }
    }
}
