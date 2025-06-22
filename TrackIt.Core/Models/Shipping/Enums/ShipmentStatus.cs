using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TrackIt.Core.Models.Shipping.Enums
{
    public enum ShipmentStatus
    {
        [EnumMember(Value = "ToShip")]
        ToShip,
        [EnumMember(Value = "ToReceive")]
        ToReceive,
        [EnumMember(Value = "ToDeliver")]
        ToDeliver,
        [EnumMember(Value = "Completed")]
        Completed
    }
}
