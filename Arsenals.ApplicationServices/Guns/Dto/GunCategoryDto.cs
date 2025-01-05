using System.Text.Json.Serialization;

namespace Arsenals.ApplicationServices.Guns.Dto;

[Obsolete("Arsenals.Modelsを使用してください")]
public class GunCategoryDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
}

