using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Core.Interfaces.Repositories;
using TrackIt.Core.Interfaces.Repository;
using TrackIt.Core.Models.Shipping;
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
    }
}
