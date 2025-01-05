using System.Text.Json.Serialization;

namespace Arsenals.ApplicationServices.Guns.Dto;

[Obsolete("Arsenals.Modelsを使用してください")]
public class GunDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("category")]
    public GunCategoryDto Category { get; set; } = null!;

    [JsonPropertyName("capacity")]
    public int Capacity { get; set; }

    [JsonPropertyName("imageUrl")]
    public string? ImageUrl { get; set; }

    [JsonPropertyName("bullets")]
    public IEnumerable<BulletDto> Bullets { get; set; } = [];
}
