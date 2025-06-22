namespace TrackIt.Server.Dto.TrackIt
{
    public record ShipmentForPatchDto
    {
        public string? title { get; init; }

        public string ?SupplierId { get; init; }

        public string? RecipientName { get; init; }
        public string? RecipientAddress { get; init; }
        public string? RecipientId { get; init; }
    }
}
