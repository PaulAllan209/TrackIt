using TrackIt.Core.Models.Account;
using TrackIt.Core.Models.TrackIt;
using TrackIt.Core.Models.TrackIt.Enums;

namespace TrackIt.Server.Dto.TrackIt
{
    public class ShipmentDto
    {
        public Guid Id { get; set; }

        public string SupplierId { get; set; }

        public string RecipientName { get; set; }
        public string RecipientAddress { get; set; }
        public string RecipientId { get; set; }

        public string CurrentStatus { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public ICollection<StatusUpdate> StatusUpdates { get; set; } = new List<StatusUpdate>();
    }
}
