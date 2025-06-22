using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Core.Models.Shipping;
using TrackIt.Core.RequestFeatures;

namespace TrackIt.Core.Services.Shipping.Interfaces
{
    public interface IStatusUpdateService
    {
        Task<StatusUpdate> CreateStatusUpdateAsync(StatusUpdate statusUpdate);
        Task<(PagedList<StatusUpdate> statusUpdates, MetaData metaData)> GetAllStatusUpdatesAsync(bool isAdmin, StatusUpdateParameters statusUpdateParameters, bool trackChanges, string? userId = null);
        Task<IEnumerable<StatusUpdate>> GetAllStatusUpdatesByShipmentIdAsync(string shipmentId, bool trackChanges);
        Task<StatusUpdate> GetStatusUpdateByIdAsync(string statusUpdateId, bool isAdmin, bool trackChanges, string? userId = null);
        Task SaveChangesToDbAsync();
        Task DeleteStatusUpdateById(string statusUpdateId, bool isAdmin, bool trackChanges, string? userId = null);
    }
}
