using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Core.Models.Shipping;

namespace TrackIt.Core.Infrastructure.Repositories
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
    }
}
