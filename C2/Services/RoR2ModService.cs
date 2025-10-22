using C2.Models;
using System.Net.Http.Json;

namespace C2.Services;

public interface IRoR2ModService
{
    Task<List<Player>> GetAllPlayersAsync();
    Task<List<Item>> GetAllItemsAsync();
    Task<ModStatus> GetStatusAsync();
    Task<ApiResponse> CycleItemAsync();
    Task<ApiResponse> CycleTierAsync();
    Task<ApiResponse> ToggleAsync();
    Task<ApiResponse> EnableAsync();
    Task<ApiResponse> DisableAsync();
    Task<ApiResponse> SetItemByNameAsync(string itemName);
    Task<ApiResponse> SetAffectedPlayersAsync(List<string> playerNames);
}

public class RoR2ModService : IRoR2ModService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public RoR2ModService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        // TODO: Configure the base URL for the mod's web API
        _baseUrl = "http://localhost:8080";
    }

    public async Task<List<Player>> GetAllPlayersAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/players");
            var json = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Players API Response: {json}");
            var playersResponse = System.Text.Json.JsonSerializer.Deserialize<PlayersResponse>(json) ?? new PlayersResponse();
            return playersResponse.Players ?? new List<Player>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting players: {ex.Message}");
            return new List<Player>();
        }
    }

    public async Task<List<Item>> GetAllItemsAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ItemsResponse>($"{_baseUrl}/api/items");
            return response?.Items ?? new List<Item>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting items: {ex.Message}");
            return new List<Item>();
        }
    }

    public async Task<ModStatus> GetStatusAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ModStatus>($"{_baseUrl}/api/status");
            return response ?? new ModStatus();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting status: {ex.Message}");
            return new ModStatus();
        }
    }

    public async Task<ApiResponse> CycleItemAsync()
    {
        try
        {
            var response = await _httpClient.PostAsync($"{_baseUrl}/api/cycle", null);
            var json = await response.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<ApiResponse>(json) ?? new ApiResponse();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error cycling item: {ex.Message}");
            return new ApiResponse { Message = $"Error: {ex.Message}" };
        }
    }

    public async Task<ApiResponse> CycleTierAsync()
    {
        try
        {
            var response = await _httpClient.PostAsync($"{_baseUrl}/api/cycle-tier", null);
            var json = await response.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<ApiResponse>(json) ?? new ApiResponse();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error cycling tier: {ex.Message}");
            return new ApiResponse { Message = $"Error: {ex.Message}" };
        }
    }

    public async Task<ApiResponse> ToggleAsync()
    {
        try
        {
            var response = await _httpClient.PostAsync($"{_baseUrl}/api/toggle", null);
            var json = await response.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<ApiResponse>(json) ?? new ApiResponse();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error toggling: {ex.Message}");
            return new ApiResponse { Message = $"Error: {ex.Message}" };
        }
    }

    public async Task<ApiResponse> EnableAsync()
    {
        try
        {
            var response = await _httpClient.PostAsync($"{_baseUrl}/api/enable", null);
            var json = await response.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<ApiResponse>(json) ?? new ApiResponse();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error enabling: {ex.Message}");
            return new ApiResponse { Message = $"Error: {ex.Message}" };
        }
    }

    public async Task<ApiResponse> DisableAsync()
    {
        try
        {
            var response = await _httpClient.PostAsync($"{_baseUrl}/api/disable", null);
            var json = await response.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<ApiResponse>(json) ?? new ApiResponse();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error disabling: {ex.Message}");
            return new ApiResponse { Message = $"Error: {ex.Message}" };
        }
    }

    public async Task<ApiResponse> SetItemByNameAsync(string itemName)
    {
        try
        {
            var requestBody = System.Text.Json.JsonSerializer.Serialize(new { itemName });
            var content = new StringContent(requestBody, System.Text.Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync($"{_baseUrl}/api/set-item", content);
            var json = await response.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<ApiResponse>(json) ?? new ApiResponse();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error setting item: {ex.Message}");
            return new ApiResponse { Message = $"Error: {ex.Message}" };
        }
    }

    public async Task<ApiResponse> SetAffectedPlayersAsync(List<string> playerNames)
    {
        try
        {
            var requestBody = System.Text.Json.JsonSerializer.Serialize(new { players = playerNames });
            var content = new StringContent(requestBody, System.Text.Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync($"{_baseUrl}/api/set-players", content);
            var json = await response.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<ApiResponse>(json) ?? new ApiResponse();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error setting affected players: {ex.Message}");
            return new ApiResponse { Message = $"Error: {ex.Message}" };
        }
    }
}
