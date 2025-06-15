using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Core.Interfaces.Repository;
using TrackIt.Core.Interfaces.Services;
using TrackIt.Core.Models.TrackIt;

namespace TrackIt.Core.Services
{
    public class ShipmentService : IShipmentService
    {
        private readonly IShipmentRepository _shipmentRepository;
        public ShipmentService(IShipmentRepository shipmentRepository)
        {
            _shipmentRepository = shipmentRepository;
        }

        public async Task<Shipment> CreateShipmentAsync(Shipment shipment)
        {
            if (shipment == null)
                throw new ArgumentNullException(nameof(shipment));

            await _shipmentRepository.CreateShipmentAsync(shipment);
            await _shipmentRepository.SaveChanges();

            return shipment;
        }
    }
}
