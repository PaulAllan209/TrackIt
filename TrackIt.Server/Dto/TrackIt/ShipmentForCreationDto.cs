using System.ComponentModel.DataAnnotations;

namespace TrackIt.Server.Dto.TrackIt
{
    public record ShipmentForCreationDto
    {
        [Required(ErrorMessage = "RecipientName is required")]
        public string RecipientName { get; init; }

        [Required(ErrorMessage = "RecipientAddress is required")]
        public string RecipientAddress { get; init; }

        [Required(ErrorMessage = "RecipientId is required")]
        public string RecipientId { get; init; }
    }
}
