using InventoryManagement.Domain.Interfaces;
using System.Diagnostics;
using System.Net.Http.Json;

namespace InventoryManagement.Domain.Services
{
    public class ExternalUserService : IExternalUserService
    {
        private readonly HttpClient _httpClient;
        public ExternalUserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ExternalUser?> GetUserAsync(int id, CancellationToken token)
        {
            var stopwatch = Stopwatch.StartNew();

            var user = await _httpClient.GetFromJsonAsync<ExternalUser>(
        $"users/{id}", token);
            stopwatch.Stop();
            Console.WriteLine($"GetUserAsync execution time: {stopwatch.ElapsedMilliseconds} ms");
            return user ?? throw new InvalidOperationException("User not found");
        }

        public async Task<ExternalUser?[]> GetThreeUsersAsync(int firstUserId, int secondUserId, int thirdUserId, CancellationToken token)
        {
            var stopwatch = Stopwatch.StartNew();
            var tasks = new[] { firstUserId, secondUserId, thirdUserId}
            .Select(id => _httpClient.GetFromJsonAsync<ExternalUser>($"users/{id}", token));

            ExternalUser?[] users = await Task.WhenAll(tasks);

            stopwatch.Stop();
            Console.WriteLine($"GetThreeUsersAsync execution time: {stopwatch.ElapsedMilliseconds} ms");

            return users ?? throw new InvalidOperationException("Users not found");
        }
    }
}
