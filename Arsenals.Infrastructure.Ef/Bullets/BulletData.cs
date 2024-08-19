using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Arsenals.Infrastructure.Ef.Guns;
using Microsoft.EntityFrameworkCore;

namespace Arsenals.Infrastructure.Ef.Bullets;

[Table("bullets")]
[DisplayName("弾丸")]
[Index(nameof(Name))]
public class BulletData
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column("id")]
    [DisplayName("主キー")]
    public int Id { get; set; }

    [Required]
    [Column("name")]
    [MaxLength(50)]
    [MinLength(1)]
    [DisplayName("弾丸名")]
    public string Name { get; set; } = null!;

    [Required]
    [Column("damage")]
    [Range(1, 50)]
    [DisplayName("ダメージ")]
    public int Damage { get; set; }

    [ForeignKey(nameof(GunData))]
    [Column("gun_id")]
    [DisplayName("銃の外部キー")]
    public int GunDataId { get; set; }

    public GunData? GunData { get; set; }
}
