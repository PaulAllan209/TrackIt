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
    }
}
