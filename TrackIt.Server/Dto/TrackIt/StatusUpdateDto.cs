namespace TrackIt.Server.Dto.TrackIt
{
    public record StatusUpdateDto
    {
        public Guid Id { get; init; }
        public Guid ShipmentId { get; init; }
        public string Status { get; init; }
        public string? Notes { get; init; }
        public string? Location { get; init; }
        public string? ImageUrl { get; init; }

        public string? CreatedBy { get; init; }
        public string? UpdatedBy { get; init; }
        public DateTime CreatedDate { get; init; }
        public DateTime UpdatedDate { get; init; }
    }
}
