using System.ComponentModel.DataAnnotations;

namespace TrackIt.Server.Dto.TrackIt
{
    public record StatusUpdateForCreationDto
    {
        [Required(ErrorMessage = "Shipment Id is required")]
        public string ShipmentId { get; init; }
        [Required(ErrorMessage = "Status is required")]
        public string Status { get; init; }
        public string? Notes { get; init; }
        public string? Location { get; init; }
        public string? ImageUrl { get; init; }
    }
}
