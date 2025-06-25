namespace TrackIt.Core.Services.Account
{
    public interface IUserIdAccessor
    {
        string? GetCurrentUserId();
    }
}
