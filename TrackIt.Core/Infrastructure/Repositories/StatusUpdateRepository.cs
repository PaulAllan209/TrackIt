using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Core.Interfaces.Repositories;
using TrackIt.Core.Models.Shipping;

namespace TrackIt.Core.Infrastructure.Repositories
{
    public class StatusUpdateRepository : RepositoryBase<StatusUpdate>, IStatusUpdateRepository
    {
        public StatusUpdateRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }
        public async Task CreateStatusUpdateAsync(StatusUpdate statusUpdate)
        {
            await CreateAsync(statusUpdate);
        }

        public async Task<IEnumerable<StatusUpdate>> GetAllStatusUpdatesAsync(bool isAdmin, bool trackChanges, string? userId = null)
        {
            if (isAdmin)
                return await FindAll(trackChanges).ToListAsync();

            else if (!string.IsNullOrEmpty(userId))
                return await FindByCondition(su => (su.CreatedBy == userId) || (su.UpdatedBy == userId), trackChanges).ToListAsync();

            return new List<StatusUpdate>(); // Return empty list if no conditionals passed
            
        }

        public async Task<IEnumerable<StatusUpdate>> GetAllStatusUpdatesByShipmentIdAsync(string shipmentId, bool trackChanges)
        {
            if (!string.IsNullOrEmpty(shipmentId))
                return await FindByCondition(su => (su.ShipmentId.ToString() == shipmentId), trackChanges).ToListAsync();

            return new List<StatusUpdate>();
        }

        public async Task<StatusUpdate?> GetStatusUpdatesByIdAsync(string statusUpdateId, bool isAdmin, bool trackChanges, string? userId = null)
        {
            if (isAdmin)
                return await FindByCondition(su => su.Id.ToString() == statusUpdateId, trackChanges).FirstOrDefaultAsync();

            else if (!string.IsNullOrEmpty(userId))
                return await FindByCondition(su => (su.Id.ToString() == statusUpdateId) && ((su.CreatedBy == userId) || (su.UpdatedBy == userId)), trackChanges).FirstOrDefaultAsync();

            return null;
        }

        public void DeleteStatusUpdate(StatusUpdate statusUpdate)
        {
            Delete(statusUpdate);
        }

        public async Task SaveAsync()
        {
            await SaveChangesAsync();
        }
    }
}
