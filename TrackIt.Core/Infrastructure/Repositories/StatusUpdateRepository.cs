using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Core.Infrastructure.Repositories.Extensions;
using TrackIt.Core.Interfaces.Repositories;
using TrackIt.Core.Models.Shipping;
using TrackIt.Core.RequestFeatures;

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

        public async Task<PagedList<StatusUpdate>> GetAllStatusUpdatesAsync(bool isAdmin, StatusUpdateParameters statusUpdateParameters, bool trackChanges, string? userId = null)
        {
            IQueryable<StatusUpdate> query = FindAll(trackChanges);

            if (!string.IsNullOrEmpty(userId) && !isAdmin)
                query = query.Where(su => (su.CreatedBy == userId) || (su.UpdatedBy == userId));

            var count = await query.CountAsync();
            var items = await query
                .Sort(statusUpdateParameters.OrderBy)
                .Skip((statusUpdateParameters.PageNumber - 1) * statusUpdateParameters.PageSize)
                .Take(statusUpdateParameters.PageSize)
                .ToListAsync();

            return new PagedList<StatusUpdate>(items, count, statusUpdateParameters.PageNumber, statusUpdateParameters.PageSize); 
            
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
