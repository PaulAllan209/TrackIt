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

        public async Task SaveAsync()
        {
            await SaveChangesAsync();
        }
    }
}
