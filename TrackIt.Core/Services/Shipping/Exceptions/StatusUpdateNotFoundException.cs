using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackIt.Core.Services.Shipping.Exceptions
{
    public sealed class StatusUpdateNotFoundException : NotFoundException
    {
        public StatusUpdateNotFoundException(string statusUpdateId) : base($"The StatusUpdate with Id {statusUpdateId} could not be found")
        {
            
        }
    }
}
