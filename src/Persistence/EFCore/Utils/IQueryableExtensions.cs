using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Persistence
{
    /// <summary>
    /// Extension methods for IQueryable to facilitate query construction and pagination.
    /// </summary>
    internal static class IQueryableExtensions
    {
        /// <summary>
        /// Constructs a query based on specified parameters such as filtering, ordering, and inclusion.
        /// </summary>
        /// <typeparam name="TSource">The type of the entity in the query.</typeparam>
        /// <param name="query">The IQueryable to be modified.</param>
        /// <param name="where">The filter expression for the query.</param>
        /// <param name="orderBy">The ordering expression for ascending order.</param>
        /// <param name="orderByDescending">The ordering expression for descending order.</param>
        /// <param name="include">The include expression for related entities.</param>
        /// <param name="trackingMode">Indicates whether tracking is enabled or disabled.</param>
        /// <param name="evenArchivedData">Indicates whether archived data should be included.</param>
        /// <returns>An IQueryable representing the modified query based on the specified parameters.</returns>
        internal static IQueryable<TSource> MakeQuery<TSource>(
            this IQueryable<TSource> query,
            Expression<Func<TSource, bool>>? where = null,
            Expression<Func<TSource, object>>? orderBy = null,
            Expression<Func<TSource, object>>? orderByDescending = null,
            Expression<Func<TSource, object>>? include = null,
            bool? trackingMode = false,
            bool? evenArchivedData = false) 
            where TSource : class
        {
            if ((bool)!trackingMode!)
                query = query.AsNoTracking();

            if ((bool)evenArchivedData!)
                query = query.IgnoreQueryFilters();

            if (where != null)
                query = query.Where(where);

            if (orderBy != null)
                query = query.OrderBy(orderBy);

            if (orderByDescending != null)
                query = query.OrderByDescending(orderByDescending);

            if (include != null)
                query = query.Include(include);

            return query;
        }

        /// <summary>
        /// Skips a specified number of elements in a query and limits the result set.
        /// </summary>
        /// <typeparam name="TSource">The type of the entity in the query.</typeparam>
        /// <param name="query">The IQueryable to be modified.</param>
        /// <param name="pageNumber">The number of items to skip (pageNumber for pagination).</param>
        /// <param name="pageSize">The number of items to be included in the result set (pageSize for pagination).</param>
        /// <returns>An IQueryable representing the modified query with skip and pageSize applied.</returns>
        internal static IQueryable<TSource> SkipQuery<TSource>(
            this IQueryable<TSource> query,
            int? pageNumber = null,
            int? pageSize = null)
            where TSource : class
        {
            if (pageNumber == null)
                return query;

            if (pageNumber < 1)
                throw new ArgumentException("The page number must be 1 or higher.");

            if (pageSize != null && pageSize < 1)
                throw new ArgumentException("The page size must be 1 or higher.");

            pageNumber = pageNumber - 1;

            query = query.Skip(((int)pageNumber!) * (pageSize ?? 0));
            if (pageSize != null)
                query = query.Take((int)pageSize);

            return query;
        }
    }
}
