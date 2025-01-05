﻿// <auto-generated />
using Arsenals.Infrastructure.Ef;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Arsenals.Infrastructure.Ef.Migrations
{
    [DbContext(typeof(ArsenalDbContext))]
    [Migration("20250104234711_AddGunImageTable")]
    partial class AddGunImageTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Arsenals.Infrastructure.Ef.Bullets.BulletData", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id")
                        .HasComment("主キー");

                    b.Property<int>("Damage")
                        .HasColumnType("integer")
                        .HasColumnName("damage")
                        .HasComment("ダメージ");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("name")
                        .HasComment("弾丸名");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("bullets", "arsenals");
                });

            modelBuilder.Entity("Arsenals.Infrastructure.Ef.Guns.GunCategoryData", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id")
                        .HasComment("主キー");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("name")
                        .HasComment("カテゴリー名");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("gun_categories", "arsenals");
                });

            modelBuilder.Entity("Arsenals.Infrastructure.Ef.Guns.GunData", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id")
                        .HasComment("主キー");

                    b.Property<int>("Capacity")
                        .HasColumnType("integer")
                        .HasColumnName("capacity")
                        .HasComment("装弾数");

                    b.Property<string>("GunCategoryDataId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("gun_category_id")
                        .HasComment("銃のカテゴリー外部キー");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name")
                        .HasComment("銃の名称");

                    b.HasKey("Id");

                    b.HasIndex("GunCategoryDataId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("guns", "arsenals");
                });

            modelBuilder.Entity("Arsenals.Infrastructure.Ef.Guns.GunImageData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasComment("主キー");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("GunId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("gun_id")
                        .HasComment("銃の外部キー");

                    b.HasKey("Id");

                    b.HasIndex("GunId")
                        .IsUnique();

                    b.ToTable("gun_images", "arsenals");
                });

            modelBuilder.Entity("BulletDataGunData", b =>
                {
                    b.Property<string>("BulletDataListId")
                        .HasColumnType("text");

                    b.Property<string>("GunDataListId")
                        .HasColumnType("text");

                    b.HasKey("BulletDataListId", "GunDataListId");

                    b.HasIndex("GunDataListId");

                    b.ToTable("BulletDataGunData", "arsenals");
                });

            modelBuilder.Entity("Arsenals.Infrastructure.Ef.Guns.GunData", b =>
                {
                    b.HasOne("Arsenals.Infrastructure.Ef.Guns.GunCategoryData", "GunCategoryData")
                        .WithMany()
                        .HasForeignKey("GunCategoryDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GunCategoryData");
                });

            modelBuilder.Entity("Arsenals.Infrastructure.Ef.Guns.GunImageData", b =>
                {
                    b.HasOne("Arsenals.Infrastructure.Ef.Guns.GunData", "GunData")
                        .WithOne("GunImageData")
                        .HasForeignKey("Arsenals.Infrastructure.Ef.Guns.GunImageData", "GunId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GunData");
                });

            modelBuilder.Entity("BulletDataGunData", b =>
                {
                    b.HasOne("Arsenals.Infrastructure.Ef.Bullets.BulletData", null)
                        .WithMany()
                        .HasForeignKey("BulletDataListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Arsenals.Infrastructure.Ef.Guns.GunData", null)
                        .WithMany()
                        .HasForeignKey("GunDataListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Arsenals.Infrastructure.Ef.Guns.GunData", b =>
                {
                    b.Navigation("GunImageData");
                });
#pragma warning restore 612, 618
        }
    }
}
