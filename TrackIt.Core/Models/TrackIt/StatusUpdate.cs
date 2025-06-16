using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Core.Models.TrackIt.Enums;

namespace TrackIt.Core.Models.TrackIt
{
    public class StatusUpdate : IAuditableEntity
    {
        public Guid Id { get; set; }

        public Guid ShipmentId { get; set; }
        public Shipment Shipment { get; set; }

        public ShipmentStatus Status { get; set; }
        public string? Notes { get; set; }
        public string? Location { get; set; }
        public string? ImageUrl { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
