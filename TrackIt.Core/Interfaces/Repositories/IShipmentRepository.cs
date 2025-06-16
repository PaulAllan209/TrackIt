using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Core.Models.TrackIt;
using TrackIt.Core.Models.TrackIt.Enums;

namespace TrackIt.Core.Interfaces.Repository
{
    public interface IShipmentRepository
    {
        Task CreateShipmentAsync(Shipment shipment);
        Task<IEnumerable<Shipment>> GetAllShipmentsAsync(string userType, bool trackChanges, string? userId = null);
        Task SaveChanges();
    }
}
