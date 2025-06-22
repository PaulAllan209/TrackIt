using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Core.Models.Account;
using TrackIt.Core.Models.Shipping.Enums;

namespace TrackIt.Core.Models.Shipping
{
    public class Shipment : IAuditableEntity
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string SupplierId { get; set; }
        public ApplicationUser Supplier { get; set; }

        public string RecipientName { get; set; }
        public string RecipientAddress { get; set; }
        public string RecipientId { get; set; }
        public ApplicationUser Recipient { get; set; }

        public ShipmentStatus CurrentStatus { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public ICollection<StatusUpdate> StatusUpdates { get; set; } = new List<StatusUpdate>();
    }
}
