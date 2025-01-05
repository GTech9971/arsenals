using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Arsenals.Infrastructure.Ef.Guns;

[Table("gun_images", Schema = DbConst.SchemaName)]
[DisplayName("銃画像")]
public class GunImageData
{
    [Key]
    [Column("id")]
    [Comment("主キー")]
    public int Id { get; set; }

    [Required]
    [Column("extension")]
    [Comment("拡張子")]
    public string Extension { get; set; } = null!;
}
