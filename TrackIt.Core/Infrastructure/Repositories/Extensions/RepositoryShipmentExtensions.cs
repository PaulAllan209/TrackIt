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
    public static class RepositoryShipmentExtensions
    {
        public static IQueryable<Shipment> SearchTitle(this IQueryable<Shipment> shipments, string? searchTitle)
        {
            if (string.IsNullOrWhiteSpace(searchTitle))
                return shipments;

            var lowerCaseTerm = searchTitle.Trim().ToLower();

            return shipments.Where(s => s.Title.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Shipment> Sort(this IQueryable<Shipment> shipments, string? orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return shipments.OrderBy(s => s.CreatedDate);

            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(Shipment).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty == null)
                    continue;

                var direction = param.EndsWith(" desc") ? "descending" : "ascending";

                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction},");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            if (string.IsNullOrWhiteSpace(orderQuery))
                return shipments.OrderBy(s => s.CreatedDate);

            // Use System.Linq.Dynamic.Core to handle dynamic ordering
            return shipments.OrderBy(orderQuery);
        }
    }
}
