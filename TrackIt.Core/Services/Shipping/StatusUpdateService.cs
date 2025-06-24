using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Core.Interfaces.Repositories;
using TrackIt.Core.Interfaces.Repository;
using TrackIt.Core.Models.Shipping;
using TrackIt.Core.Models.Shipping.Enums;
using TrackIt.Core.RequestFeatures;
using TrackIt.Core.Services.Shipping.Exceptions;
using TrackIt.Core.Services.Shipping.Interfaces;

namespace TrackIt.Core.Services.Shipping
{
    public class StatusUpdateService : IStatusUpdateService
    {
        private readonly IStatusUpdateRepository _statusUpdateRepository;
        private readonly IShipmentRepository _shipmentRepository;

        public StatusUpdateService(IStatusUpdateRepository statusUpdateRepository, IShipmentRepository shipmentRepository)
        {
            _statusUpdateRepository = statusUpdateRepository;
            _shipmentRepository = shipmentRepository;
        }

        public async Task<StatusUpdate> CreateStatusUpdateAsync(StatusUpdate statusUpdate)
        {
            if (statusUpdate == null)
                throw new ArgumentNullException(nameof(statusUpdate));

            var shipmentEntity = await _shipmentRepository.GetShipmentByIdAsync(statusUpdate.ShipmentId.ToString(), trackChanges: true);

            if (shipmentEntity == null)
                throw new ShipmentNotFoundException();

            if (statusUpdate.Status == ShipmentStatus.Delivered)
                shipmentEntity.DeliveredAt = DateTime.UtcNow;

            shipmentEntity.CurrentStatus = statusUpdate.Status;
            await _statusUpdateRepository.CreateStatusUpdateAsync(statusUpdate);

            await _shipmentRepository.SaveAsync();
            await _statusUpdateRepository.SaveAsync();

            return statusUpdate;
        }

        public async Task<(PagedList<StatusUpdate> statusUpdates, MetaData metaData)> GetAllStatusUpdatesAsync(bool isAdmin, StatusUpdateParameters statusUpdateParameters, bool trackChanges, string? userId = null)
        {
            if (!isAdmin && string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId), "UserId is required for non-admin requests.");

            var statusUpdatesWithMetadata = await _statusUpdateRepository.GetAllStatusUpdatesAsync(isAdmin, statusUpdateParameters, trackChanges, userId);

            return (statusUpdates: statusUpdatesWithMetadata, metaData: statusUpdatesWithMetadata.MetaData);
        }

        public async Task<IEnumerable<StatusUpdate>> GetAllStatusUpdatesByShipmentIdAsync(string shipmentId, bool trackChanges)
        {
            if (string.IsNullOrEmpty(shipmentId))
                throw new ArgumentNullException(nameof(shipmentId));

            var statusUpdateEntities = await _statusUpdateRepository.GetAllStatusUpdatesByShipmentIdAsync(shipmentId, trackChanges);

            return statusUpdateEntities;
        }

        public async Task<StatusUpdate> GetStatusUpdateByIdAsync(string statusUpdateId, bool isAdmin, bool trackChanges, string? userId = null)
        {
            if (string.IsNullOrEmpty(statusUpdateId))
                throw new ArgumentNullException(nameof(statusUpdateId));

            if (!isAdmin && string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId), "UserId is required for non-admin requests.");

            var statusUpdateEntity = await _statusUpdateRepository.GetStatusUpdatesByIdAsync(statusUpdateId, isAdmin, trackChanges, userId);

            if (statusUpdateEntity == null)
                throw new StatusUpdateNotFoundException(statusUpdateId);

            return statusUpdateEntity;
        }

        public async Task DeleteStatusUpdateById(string statusUpdateId, bool isAdmin, bool trackChanges, string? userId = null)
        {
            if (string.IsNullOrEmpty(statusUpdateId))
                throw new ArgumentNullException(nameof(statusUpdateId));

            if (!isAdmin && string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId), "UserId is required for non-admin requests.");

            var statusUpdateEntity = await _statusUpdateRepository.GetStatusUpdatesByIdAsync(statusUpdateId, isAdmin, trackChanges, userId);

            if (statusUpdateEntity == null)
                throw new StatusUpdateNotFoundException(statusUpdateId);

            // Repository layer actions
            _statusUpdateRepository.DeleteStatusUpdate(statusUpdateEntity);
            await _statusUpdateRepository.SaveAsync();
        }

        public async Task SaveChangesToDbAsync()
        {
            await _statusUpdateRepository.SaveAsync();
        }
    }
}
