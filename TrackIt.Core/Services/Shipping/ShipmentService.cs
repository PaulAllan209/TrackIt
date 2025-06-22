using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Core.Interfaces.Repository;
using TrackIt.Core.Models.Shipping;
using TrackIt.Core.RequestFeatures;
using TrackIt.Core.Services.Shipping.Exceptions;
using TrackIt.Core.Services.Shipping.Interfaces;

namespace TrackIt.Core.Services.Shipping
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
            await _shipmentRepository.SaveAsync();

            return shipment;
        }

        public async Task<(PagedList<Shipment> shipments, MetaData metaData)> GetAllShipmentAsync(string userType, ShipmentParameters shipmentParameters, bool trackChanges, string? userId = null)
        {
            if(string.IsNullOrWhiteSpace(userType))
            {
                throw new ArgumentNullException("User type cannot be empty.", nameof(userType));
            }

            var shipmentEntitiesWithMetaData = await _shipmentRepository.GetAllShipmentsAsync(userType, shipmentParameters, trackChanges, userId);

            return (shipments: shipmentEntitiesWithMetaData, metaData: shipmentEntitiesWithMetaData.MetaData);
        }

        public async Task<Shipment> GetShipmentByIdAsync(string userType, string shipmentId, bool trackChanges, string? userId = null)
        {
            if (string.IsNullOrWhiteSpace(userType))
            {
                throw new ArgumentNullException("User type cannot be empty.", nameof(userType));
            }
            if (string.IsNullOrWhiteSpace(shipmentId))
            {
                throw new ArgumentNullException("Shipment ID cannot be empty.", nameof(shipmentId));
            }

            var shipmentEntity = await _shipmentRepository.GetShipmentByIdAsync(userType, shipmentId, trackChanges, userId);

            if (shipmentEntity == null)
            {
                throw new ShipmentNotFoundException($"Shipment with ID '{shipmentId}' could not be found.");
            }

            return shipmentEntity;
        }

        public async Task UpdateShipmentAsync()
        {
            // This is just a save changes async call so that after modifying the entity in the controller layer 
            await _shipmentRepository.SaveAsync();
        }

        public async Task DeleteShipmentAsync(string userType, string shipmentId, bool trackChanges, string? userId = null)
        {
            if (string.IsNullOrWhiteSpace(userType))
            {
                throw new ArgumentNullException("User type cannot be empty.", nameof(userType));
            }
            if (string.IsNullOrWhiteSpace(shipmentId))
            {
                throw new ArgumentNullException("Shipment ID cannot be empty.", nameof(shipmentId));
            }
            
            var shipmentEntityToDelete = await _shipmentRepository.GetShipmentByIdAsync(userType, shipmentId, trackChanges, userId);

            if (shipmentEntityToDelete == null)
            {
                throw new ShipmentNotFoundException($"Shipment with ID '{shipmentId}' could not be found.");
            }

            // Repository layer actions
            _shipmentRepository.DeleteShipment(shipmentEntityToDelete);
            await _shipmentRepository.SaveAsync();
        }
    }
}
