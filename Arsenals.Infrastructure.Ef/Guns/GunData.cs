using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Arsenals.Infrastructure.Ef.Bullets;
using Microsoft.EntityFrameworkCore;

namespace Arsenals.Infrastructure.Ef.Guns;

[Table("guns")]
[DisplayName("銃")]
[Index(nameof(Name), IsUnique = true)]
public class GunData
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column("id")]
    [DisplayName("主キー")]
    public int Id { get; set; }

    [Required]
    [MinLength(1)]
    [Column("name")]
    [DisplayName("銃の名称")]
    public string Name { get; set; } = null!;

    [Required]
    [Column("capacity")]
    [DisplayName("装弾数")]
    [Range(1, 5000)]
    public int Capacity { get; set; }


    [ForeignKey(nameof(GunCategoryData))]
    [Column("gun_category_id")]
    [DisplayName("銃のカテゴリー外部キー")]
    public int GunCategoryDataId { get; set; }

    public GunCategoryData GunCategoryData { get; set; } = null!;


    public ICollection<BulletData>? BulletDataList { get; set; }
}
