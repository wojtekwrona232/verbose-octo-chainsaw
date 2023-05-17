using System.Text.Json.Serialization;

namespace GusAPI.Models;

public class Area
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("nazwa")] public string Name { get; set; } = null!;

    [JsonPropertyName("id-nadrzedny-element")]
    public int IdParent { get; set; }

    [JsonPropertyName("id-poziom")] public int LevelId { get; set; }
    [JsonPropertyName("nazwa-poziom")] public string LevelName { get; set; } = null!;
    [JsonPropertyName("czy-zmienne")] public bool IsChangeable { get; set; }

    public Area()
    {
    }

    public Area(int id, string name, int idParent, int levelId, string levelName, bool isChangeable)
    {
        Id = id;
        Name = name;
        IdParent = idParent;
        LevelId = levelId;
        LevelName = levelName;
        IsChangeable = isChangeable;
    }
}