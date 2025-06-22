using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Core.Infrastructure.Repositories.Extensions;
using TrackIt.Core.Interfaces.Repository;
using TrackIt.Core.Models.Shipping;
using TrackIt.Core.Models.Shipping.Enums;
using TrackIt.Core.RequestFeatures;

namespace TrackIt.Core.Infrastructure.Repositories
{
    public class ShipmentRepository : RepositoryBase<Shipment>, IShipmentRepository
    {
        public ShipmentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }

        public async Task CreateShipmentAsync(Shipment shipment)
        {
            await CreateAsync(shipment);
        }

        public async Task<PagedList<Shipment>> GetAllShipmentsAsync(string userType, ShipmentParameters shipmentParameters, bool trackChanges, string? userId = null)
        {
            IQueryable<Shipment> query = FindAll(trackChanges);

            if (userType == UserType.Supplier && !string.IsNullOrEmpty(userId))
            {
                query = query.Where(s => s.SupplierId == userId);
            }
            else if (userType == UserType.Facility && !string.IsNullOrEmpty(userId))
            {
                query = query.Where(s => s.StatusUpdates.Any(su => su.UpdatedBy == userId));
            }
            else if (userType == UserType.Delivery && !string.IsNullOrEmpty(userId))
            {
                query = query.Where(s => s.StatusUpdates.Any(su => su.UpdatedBy == userId));
            }
            else if (userType == UserType.Customer && !string.IsNullOrEmpty(userId))
            {
                query = query.Where(s => s.RecipientId == userId);
            }

            var count = await query.CountAsync();
            var items = await query
                .Sort(shipmentParameters.OrderBy)
                .SearchTitle(shipmentParameters.SearchTitle)
                .Skip((shipmentParameters.PageNumber - 1) * shipmentParameters.PageSize)
                .Take(shipmentParameters.PageSize)
                .ToListAsync();

            return new PagedList<Shipment>(items, count, shipmentParameters.PageNumber, shipmentParameters.PageSize);
        }

        public async Task<Shipment?> GetShipmentByIdAsync(string shipmentId, bool trackChanges)
        {
            if (!string.IsNullOrEmpty(shipmentId))
                return await FindByCondition(s => s.Id.ToString() == shipmentId, trackChanges).FirstOrDefaultAsync();

            return null;
        }

        public void DeleteShipment(Shipment shipment)
        {
            Delete(shipment);
        }

        public async Task SaveAsync()
        {
            await SaveChangesAsync();
        }
    }
}
