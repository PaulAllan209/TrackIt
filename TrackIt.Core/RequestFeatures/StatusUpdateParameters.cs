using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackIt.Core.RequestFeatures
{
    public class StatusUpdateParameters : RequestParameters
    {
        public StatusUpdateParameters() => OrderBy = "CreatedDate";
    }
}
