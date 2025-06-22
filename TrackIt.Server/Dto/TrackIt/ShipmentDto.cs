using TrackIt.Core.Models.Account;
using TrackIt.Core.Models.Shipping;
using TrackIt.Core.Models.Shipping.Enums;

namespace TrackIt.Server.Dto.TrackIt
{
    public record ShipmentDto
    {
        public Guid Id { get; init; }

        public string title { get; init; }

        public string SupplierId { get; init; }

        public string RecipientName { get; init; }
        public string RecipientAddress { get; init; }
        public string RecipientId { get; init; }

        public string CurrentStatus { get; init; }
        public DateTime? DeliveredAt { get; init; }
        public string? CreatedBy { get; init; }
        public string? UpdatedBy { get; init; }
        public DateTime CreatedDate { get; init; }
        public DateTime UpdatedDate { get; init; }
        public ICollection<StatusUpdate> StatusUpdates { get; set; } = new List<StatusUpdate>();
    }
}
