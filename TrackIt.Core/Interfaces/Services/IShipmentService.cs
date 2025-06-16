using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Core.Models.TrackIt;

namespace TrackIt.Core.Interfaces.Services
{
    public interface IShipmentService
    {
        Task<Shipment> CreateShipmentAsync(Shipment shipment);
        Task<IEnumerable<Shipment>> GetAllShipmentAsync(string userType, bool trackChanges, string? userId = null);
    }
}
