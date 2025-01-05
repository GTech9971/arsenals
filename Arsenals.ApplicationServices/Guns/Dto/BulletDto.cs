using System.Text.Json.Serialization;

namespace Arsenals.ApplicationServices.Guns.Dto;

[Obsolete("Arsenals.Modelsを使用してください")]
public class BulletDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("damage")]
    public int Damage { get; set; }
}