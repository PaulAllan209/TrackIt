using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Core.Interfaces.Repositories;
using TrackIt.Core.Interfaces.Repository;
using TrackIt.Core.Models.Shipping;
using TrackIt.Core.Services.Shipping.Exceptions;
using TrackIt.Core.Services.Shipping.Interfaces;

namespace TrackIt.Core.Services.Shipping
{
    public class StatusUpdateService : IStatusUpdateService
    {
        private readonly IStatusUpdateRepository _statusUpdateRepository;

        public StatusUpdateService(IStatusUpdateRepository statusUpdateRepository)
        {
            _statusUpdateRepository = statusUpdateRepository;
        }

        public async Task<StatusUpdate> CreateStatusUpdateAsync(StatusUpdate statusUpdate)
        {
            if (statusUpdate == null)
                throw new ArgumentNullException(nameof(statusUpdate));

            await _statusUpdateRepository.CreateStatusUpdateAsync(statusUpdate);
            await _statusUpdateRepository.SaveAsync();

            return statusUpdate;
        }

        public async Task<IEnumerable<StatusUpdate>> GetAllStatusUpdatesAsync(bool isAdmin, bool trackChanges, string? userId = null)
        {
            if (!isAdmin && string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId), "UserId is required for non-admin requests.");

            var statusUpdateEntities = await _statusUpdateRepository.GetAllStatusUpdatesAsync(isAdmin, trackChanges, userId);

            return statusUpdateEntities;
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
