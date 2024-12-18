using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Arsenals.Infrastructure.Ef.Guns;
using Microsoft.EntityFrameworkCore;

namespace Arsenals.Infrastructure.Ef.Bullets;

[Table("bullets", Schema = DbConst.SchemaName)]
[DisplayName("弾丸")]
[Index(nameof(Name), IsUnique = true)]
public class BulletData
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column("id")]
    [DisplayName("主キー")]
    [Comment("主キー")]
    public string Id { get; set; } = null!;

    [Required]
    [Column("name")]
    [MaxLength(50)]
    [MinLength(1)]
    [DisplayName("弾丸名")]
    [Comment("弾丸名")]
    public string Name { get; set; } = null!;

    [Required]
    [Column("damage")]
    [Range(1, 50)]
    [DisplayName("ダメージ")]
    [Comment("ダメージ")]
    public int Damage { get; set; }


    public ICollection<GunData> GunDataList { get; set; } = [];
}
