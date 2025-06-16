using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Core.Interfaces.Repository;
using TrackIt.Core.Models.TrackIt;
using TrackIt.Core.Models.TrackIt.Enums;

namespace TrackIt.Core.Infrastructure.Repositories
{
    public class ShipmentRepository : RepositoryBase<Shipment>, IShipmentRepository
    {
        public ShipmentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }

        public async Task CreateShipmentAsync(Shipment shipment)
        {
            Create(shipment);
        }

        public async Task<IEnumerable<Shipment>> GetAllShipmentsAsync(string userType, bool trackChanges, string? userId = null)
        {
            if(userType == UserType.Admin && !string.IsNullOrEmpty(userId))
            {
                return await FindAll(trackChanges).ToListAsync();
            }
            else if(userType == UserType.Supplier && !string.IsNullOrEmpty(userId))
            {
                return await FindByCondition(s => s.SupplierId == userId, trackChanges).ToListAsync();
            }
            else if (userType == UserType.Facility && !string.IsNullOrEmpty(userId))
            {
                return await FindByCondition(s => s.StatusUpdates.Any(su => su.UpdatedBy == userId), trackChanges).ToListAsync();
            }
            else if (userType == UserType.Delivery && !string.IsNullOrEmpty(userId))
            {
                return await FindByCondition(s => s.StatusUpdates.Any(su => su.UpdatedBy == userId), trackChanges).ToListAsync();
            }
            else if (userType == UserType.Customer && !string.IsNullOrEmpty(userId))
            {
                return await FindByCondition(s => s.RecipientId == userId, trackChanges).ToListAsync();
            }

            return new List<Shipment>();
        }

        public async Task SaveChanges()
        {
            await SaveChangesAsync();
        }
    }
}
