using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Core.Models.Shipping;
using TrackIt.Core.Models.Shipping.Enums;

namespace TrackIt.Core.Interfaces.Repository
{
    public interface IShipmentRepository
    {
        Task CreateShipmentAsync(Shipment shipment);
        Task<IEnumerable<Shipment>> GetAllShipmentsAsync(string userType, bool trackChanges, string? userId = null);
        Task<Shipment?> GetShipmentByIdAsync(string userType, string shipmentId, bool trackChanges, string? userId = null);
        void DeleteShipment(Shipment shipment);
        Task SaveAsync();
    }
}
