using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Core.Models.TrackIt;

namespace TrackIt.Core.Interfaces.Repository
{
    public interface IShipmentRepository
    {
        Task CreateShipmentAsync(Shipment shipment);
        Task SaveChanges();
    }
}
