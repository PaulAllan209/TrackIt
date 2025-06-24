namespace TrackIt.Server.Dto.TrackIt
{
    public record StatusUpdateForPatchDto
    {
        public Guid? ShipmentId { get; init; }
        public string? Status { get; init; }
        public string? Notes { get; init; }
        public string? Location { get; init; }
        public string? ImageUrl { get; init; }
    }
}
