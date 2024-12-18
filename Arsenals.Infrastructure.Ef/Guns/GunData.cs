using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Arsenals.Domains.Guns;
using Arsenals.Infrastructure.Ef.Bullets;
using Microsoft.EntityFrameworkCore;

namespace Arsenals.Infrastructure.Ef.Guns;

[Table("guns", Schema = DbConst.SchemaName)]
[DisplayName("銃")]
[Index(nameof(Name), IsUnique = true)]
public class GunData
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column("id")]
    [DisplayName("主キー")]
    [Comment("主キー")]
    public string Id { get; set; } = null!;

    [Required]
    [MinLength(1)]
    [Column("name")]
    [DisplayName("銃の名称")]
    [Comment("銃の名称")]
    public string Name { get; set; } = null!;

    [Required]
    [Column("capacity")]
    [DisplayName("装弾数")]
    [Comment("装弾数")]
    [Range(Domains.Guns.Capacity.MIN, Domains.Guns.Capacity.MAX)]
    public int Capacity { get; set; }


    [ForeignKey(nameof(GunCategoryData))]
    [Column("gun_category_id")]
    [DisplayName("銃のカテゴリー外部キー")]
    [Comment("銃のカテゴリー外部キー")]
    public string GunCategoryDataId { get; set; } = null!;

    public GunCategoryData GunCategoryData { get; set; } = null!;


    public ICollection<BulletData> BulletDataList { get; set; } = [];
}
