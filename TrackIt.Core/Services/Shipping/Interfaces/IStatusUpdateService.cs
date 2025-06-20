using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Core.Models.Shipping;

namespace TrackIt.Core.Services.Shipping.Interfaces
{
    public interface IStatusUpdateService
    {
        Task<StatusUpdate> CreateStatusUpdateAsync(StatusUpdate statusUpdate);
        Task<IEnumerable<StatusUpdate>> GetAllStatusUpdatesAsync(bool isAdmin, bool trackChanges, string? userId = null);
        Task<IEnumerable<StatusUpdate>> GetAllStatusUpdatesByShipmentIdAsync(string shipmentId, bool trackChanges);
        Task<StatusUpdate> GetStatusUpdateByIdAsync(string statusUpdateId, bool isAdmin, bool trackChanges, string? userId = null);
        Task SaveChangesToDbAsync();
        Task DeleteStatusUpdateById(string statusUpdateId, bool isAdmin, bool trackChanges, string? userId = null);
    }
}
