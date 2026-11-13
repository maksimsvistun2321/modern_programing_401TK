namespace InventoryManagement.Domain.Interfaces
{
    public interface IExternalUserService
    {
        Task<ExternalUser?> GetUserAsync(int id, CancellationToken token);
        Task<ExternalUser?[]> GetThreeUsersAsync(int firstUserId, int secondUserId, int thirdUserId, CancellationToken token);
    }
}
