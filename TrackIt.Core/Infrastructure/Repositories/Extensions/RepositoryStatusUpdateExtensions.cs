using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Core.Models.Shipping;

namespace TrackIt.Core.Infrastructure.Repositories.Extensions
{
    public static class RepositoryStatusUpdateExtensions
    {
        public static IQueryable<StatusUpdate> Sort(this IQueryable<StatusUpdate> statusUpdates, string? orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return statusUpdates.OrderBy(su => su.CreatedDate);

            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(StatusUpdate).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty == null)
                    continue;

                var direction = param.EndsWith(" asc") ? "ascending" : "descending";

                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction},");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            if (string.IsNullOrWhiteSpace(orderQuery))
                return statusUpdates.OrderBy(s => s.CreatedDate);

            // Use System.Linq.Dynamic.Core to handle dynamic ordering
            return statusUpdates.OrderBy(orderQuery);
        }
    }
}
