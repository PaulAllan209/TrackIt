using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Core.Models.Shipping;
using TrackIt.Core.Models.Shipping.Enums;
using TrackIt.Core.RequestFeatures;

namespace TrackIt.Core.Interfaces.Repositories
{
    public interface IStatusUpdateRepository
    {
        Task CreateStatusUpdateAsync(StatusUpdate statusUpdate);
        Task<PagedList<StatusUpdate>> GetAllStatusUpdatesAsync(bool isAdmin, StatusUpdateParameters statusUpdateParameters, bool trackChanges, string? userId = null);
        Task<IEnumerable<StatusUpdate>> GetAllStatusUpdatesByShipmentIdAsync(string shipmentId, bool trackChanges);
        Task<StatusUpdate?> GetStatusUpdatesByIdAsync(string statusUpdateId, bool isAdmin, bool trackChanges, string? userId = null);
        void DeleteStatusUpdate(StatusUpdate statusUpdate);
        Task SaveAsync();
    }
}
