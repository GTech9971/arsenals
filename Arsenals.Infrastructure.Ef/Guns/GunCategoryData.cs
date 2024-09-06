using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Arsenals.Infrastructure.Ef.Guns;

[Table("gun_categories")]
[DisplayName("銃のカテゴリー")]
[Index(nameof(Name), IsUnique = true)]
public class GunCategoryData
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [DisplayName("主キー")]
    public int Id { get; set; }

    [Required]
    [Column("name")]
    [MaxLength(50)]
    [DisplayName("カテゴリー名")]
    public string Name { get; set; } = null!;
}
