using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Core.Interfaces.Repository;
using TrackIt.Core.Models.TrackIt;

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

        public async Task SaveChanges()
        {
            await SaveChangesAsync();
        }
    }
}
