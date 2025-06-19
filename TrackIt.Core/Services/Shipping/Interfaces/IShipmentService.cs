using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Core.Models.Shipping;

namespace TrackIt.Core.Services.Shipping.Interfaces
{
    public interface IShipmentService
    {
        Task<Shipment> CreateShipmentAsync(Shipment shipment);
        Task<IEnumerable<Shipment>> GetAllShipmentAsync(string userType, bool trackChanges, string? userId = null);
        Task<Shipment> GetShipmentByIdAsync(string userType, string shipmentId, bool trackChanges, string? userId = null);
        Task UpdateShipmentAsync();
        Task DeleteShipmentAsync(string userType, string shipmentId, bool trackChanges, string? userId = null);
    }
}
