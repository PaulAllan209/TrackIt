using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Core.Models.Shipping;
using TrackIt.Core.RequestFeatures;

namespace TrackIt.Core.Services.Shipping.Interfaces
{
    public interface IShipmentService
    {
        Task<Shipment> CreateShipmentAsync(Shipment shipment);
        Task<(PagedList<Shipment> shipments, MetaData metaData)> GetAllShipmentAsync(string userType, ShipmentParameters shipmentParameters, bool trackChanges, string? userId = null);
        Task<Shipment> GetShipmentByIdAsync(string shipmentId, bool trackChanges);
        Task UpdateShipmentAsync();
        Task DeleteShipmentAsync(string shipmentId, bool trackChanges);
    }
}
