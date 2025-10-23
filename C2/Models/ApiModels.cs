using System.Text.Json.Serialization;

namespace C2.Models;

public class Player
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
}

public class Item
{
    public int Index { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Tier { get; set; } = string.Empty;
    public string TierName { get; set; } = string.Empty;
    public bool IsSelected { get; set; }
    public string IconUrl { get; set; } = string.Empty;
    public int Quantity { get; set; }
    
    // Properties to maintain compatibility with existing UI code
    public string Rarity => TierName;
}

public class ItemsResponse
{
    public List<Item> Items { get; set; } = new();
    public int Count { get; set; }
    public int SelectedIndex { get; set; }
    public string CurrentTier { get; set; } = string.Empty;
    public bool Enabled { get; set; }
}

public class ModStatus
{
    public string Status { get; set; } = string.Empty;
    public bool Enabled { get; set; }
    public string Timestamp { get; set; } = string.Empty;
}

public class ApiResponse
{
    public string Message { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool? Enabled { get; set; }
    public string Timestamp { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
    public List<Player>? Players { get; set; }
    public int? Count { get; set; }
    public List<string>? SelectedPlayers { get; set; }
}

public class PlayersResponse
{
    [JsonPropertyName("players")]
    public List<Player> Players { get; set; } = new();
    
    [JsonPropertyName("count")]
    public int Count { get; set; }
    
    [JsonPropertyName("timestamp")]
    public string Timestamp { get; set; } = string.Empty;
}

public enum ShrineMode
{
    Off,
    Guaranteed,
    Impossible
}
