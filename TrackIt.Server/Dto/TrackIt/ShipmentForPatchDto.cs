using System.ComponentModel.DataAnnotations;

namespace TrackIt.Server.Dto.TrackIt
{
    public record ShipmentForPatchDto
    {
        [Required(AllowEmptyStrings = false)]
        public string? title { get; init; }

        [Required(AllowEmptyStrings = false)]
        public string? SupplierId { get; init; }

        [Required(AllowEmptyStrings = false)]
        public string? RecipientName { get; init; }

        [Required(AllowEmptyStrings = false)]
        public string? RecipientAddress { get; init; }

        [Required(AllowEmptyStrings = false)]
        public string? RecipientId { get; init; }
    }
}
