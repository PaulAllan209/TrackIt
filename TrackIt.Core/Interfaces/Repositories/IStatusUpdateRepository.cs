using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Core.Models.Shipping;
using TrackIt.Core.Models.Shipping.Enums;

namespace TrackIt.Core.Interfaces.Repositories
{
    public interface IStatusUpdateRepository
    {
        Task CreateStatusUpdateAsync(StatusUpdate statusUpdate);
        Task SaveAsync();
    }
}
