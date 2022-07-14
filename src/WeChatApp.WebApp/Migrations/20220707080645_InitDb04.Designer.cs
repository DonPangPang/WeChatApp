﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WeChatApp.WebApp.Data;

#nullable disable

namespace WeChatApp.WebApp.Migrations
{
    [DbContext(typeof(WeComAppDbContext))]
    [Migration("20220707080645_InitDb04")]
    partial class InitDb04
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("WeChatApp.Shared.Entity.BonusPointRecord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("BonusPoints")
                        .HasColumnType("decimal(10,2)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("CreateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreateUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreateUserUid")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("PickUpUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PickUpUserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("WorkTaskId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("BonusPointRecord");
                });

            modelBuilder.Entity("WeChatApp.Shared.Entity.Department", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("CreateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreateUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreateUserUid")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DepartmentName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TreeIds")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("Department");

                    b.HasData(
                        new
                        {
                            Id = new Guid("02516c95-8cc3-4643-ad6e-de08fbf5cd31"),
                            CreateTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DepartmentName = "三国集团",
                            TreeIds = "02516C95-8CC3-4643-AD6E-DE08FBF5CD31"
                        },
                        new
                        {
                            Id = new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f4"),
                            CreateTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DepartmentName = "蜀汉",
                            ParentId = new Guid("02516c95-8cc3-4643-ad6e-de08fbf5cd31"),
                            TreeIds = "02516C95-8CC3-4643-AD6E-DE08FBF5CD31,EAB4C639-14A5-4458-91BA-ED77F6A955F4"
                        },
                        new
                        {
                            Id = new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f5"),
                            CreateTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DepartmentName = "桃园三小伙",
                            ParentId = new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f4"),
                            TreeIds = "02516C95-8CC3-4643-AD6E-DE08FBF5CD31,EAB4C639-14A5-4458-91BA-ED77F6A955F4,EAB4C639-14A5-4458-91BA-ED77F6A955F5"
                        },
                        new
                        {
                            Id = new Guid("170758b4-c800-4895-b373-02488fd9c6c0"),
                            CreateTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DepartmentName = "蜀汉打工仔",
                            ParentId = new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f4"),
                            TreeIds = "02516C95-8CC3-4643-AD6E-DE08FBF5CD31,EAB4C639-14A5-4458-91BA-ED77F6A955F4,170758B4-C800-4895-B373-02488FD9C6C0"
                        },
                        new
                        {
                            Id = new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f6"),
                            CreateTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DepartmentName = "魏国",
                            ParentId = new Guid("02516c95-8cc3-4643-ad6e-de08fbf5cd31"),
                            TreeIds = "02516C95-8CC3-4643-AD6E-DE08FBF5CD31,EAB4C639-14A5-4458-91BA-ED77F6A955F6"
                        },
                        new
                        {
                            Id = new Guid("00663153-ca24-4e78-aee1-8beafcd045c7"),
                            CreateTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DepartmentName = "魏国打工仔",
                            ParentId = new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f6"),
                            TreeIds = "02516C95-8CC3-4643-AD6E-DE08FBF5CD31,EAB4C639-14A5-4458-91BA-ED77F6A955F6,00663153-CA24-4E78-AEE1-8BEAFCD045C7"
                        },
                        new
                        {
                            Id = new Guid("00663153-ca24-4e78-aee1-8beafcd045c8"),
                            CreateTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DepartmentName = "虎豹骑",
                            ParentId = new Guid("00663153-ca24-4e78-aee1-8beafcd045c7"),
                            TreeIds = "02516C95-8CC3-4643-AD6E-DE08FBF5CD31,EAB4C639-14A5-4458-91BA-ED77F6A955F6,00663153-CA24-4E78-AEE1-8BEAFCD045C7,00663153-CA24-4E78-AEE1-8BEAFCD045C8"
                        },
                        new
                        {
                            Id = new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f7"),
                            CreateTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DepartmentName = "吴国",
                            ParentId = new Guid("02516c95-8cc3-4643-ad6e-de08fbf5cd31"),
                            TreeIds = "02516C95-8CC3-4643-AD6E-DE08FBF5CD31,EAB4C639-14A5-4458-91BA-ED77F6A955F7"
                        },
                        new
                        {
                            Id = new Guid("b8d77ddd-a4a3-4ad0-a834-e2e7dfbaf9ef"),
                            CreateTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DepartmentName = "吴国打工仔",
                            ParentId = new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f7"),
                            TreeIds = "02516C95-8CC3-4643-AD6E-DE08FBF5CD31,EAB4C639-14A5-4458-91BA-ED77F6A955F7,B8D77DDD-A4A3-4AD0-A834-E2E7DFBAF9EF"
                        });
                });

            modelBuilder.Entity("WeChatApp.Shared.Entity.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("DepartmentId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsSuper")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Tel")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Uid")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            Id = new Guid("45d16a4c-4a1c-4cd9-a0ba-6af0b86bbaf6"),
                            DepartmentId = new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f5"),
                            Email = "liubei@qq.com",
                            IsSuper = false,
                            Name = "刘备",
                            Password = "123456",
                            Role = 2,
                            Tel = "13900000000",
                            Uid = "liubei"
                        },
                        new
                        {
                            Id = new Guid("45d16a4c-4a1c-4cd9-a0ba-6af0b86bbaf7"),
                            DepartmentId = new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f5"),
                            Email = "guanyu@qq.com",
                            IsSuper = false,
                            Name = "关羽",
                            Password = "123456",
                            Role = 1,
                            Tel = "13900000000",
                            Uid = "guanyu"
                        },
                        new
                        {
                            Id = new Guid("45d16a4c-4a1c-4cd9-a0ba-6af0b86bbaf8"),
                            DepartmentId = new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f5"),
                            Email = "zhangfei@qq.com",
                            IsSuper = false,
                            Name = "张飞",
                            Password = "123456",
                            Role = 1,
                            Tel = "13900000000",
                            Uid = "zhangfei"
                        },
                        new
                        {
                            Id = new Guid("45d16a4c-4a1c-4cd9-a0ba-6af0b86bbaf9"),
                            DepartmentId = new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f5"),
                            Email = "zhugeliang@qq.com",
                            IsSuper = false,
                            Name = "诸葛亮",
                            Password = "123456",
                            Role = 0,
                            Tel = "13900000000",
                            Uid = "zhugeliang"
                        },
                        new
                        {
                            Id = new Guid("45d16a4c-4a1c-4cd9-a0ba-7af0b86bbaf8"),
                            DepartmentId = new Guid("170758b4-c800-4895-b373-02488fd9c6c0"),
                            Email = "zhaozilong@qq.com",
                            IsSuper = false,
                            Name = "赵子龙",
                            Password = "123456",
                            Role = 1,
                            Tel = "13900000000",
                            Uid = "zhaozilong"
                        },
                        new
                        {
                            Id = new Guid("8898ae8f-78c0-48f8-8e6a-f5894e9a4ec4"),
                            DepartmentId = new Guid("170758b4-c800-4895-b373-02488fd9c6c0"),
                            Email = "zhangsan@qq.com",
                            IsSuper = false,
                            Name = "张三",
                            Password = "123456",
                            Role = 0,
                            Tel = "13900000000",
                            Uid = "zhangsan"
                        },
                        new
                        {
                            Id = new Guid("8898ae8f-78c0-48f8-8e6a-f5894e9a4ec5"),
                            DepartmentId = new Guid("170758b4-c800-4895-b373-02488fd9c6c0"),
                            Email = "zhaozilong@qq.com",
                            IsSuper = false,
                            Name = "lisi",
                            Password = "123456",
                            Role = 0,
                            Tel = "13900000000",
                            Uid = "lisi"
                        });
                });

            modelBuilder.Entity("WeChatApp.Shared.Entity.WorkTask", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("CreateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreateUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreateUserUid")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("DepartmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsPublicNodes")
                        .HasColumnType("bit");

                    b.Property<int>("MaxPickUpCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("ModifyTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("ModifyUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ModifyUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModifyUserUid")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PickUpUserIds")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PickUpUserNames")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("PointsRewards")
                        .HasColumnType("decimal(10,2)");

                    b.Property<decimal>("PointsSettlement")
                        .HasColumnType("decimal(10,2)");

                    b.Property<DateTime>("PublicEndTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("PublicStartTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<byte[]>("Timespan")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("WorkPublishType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("WorkTask");
                });

            modelBuilder.Entity("WeChatApp.Shared.Entity.WorkTaskNode", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("CreateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreateUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreateUserUid")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageSources")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("NodeTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<Guid>("WorkTaskId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("WorkTaskId");

                    b.ToTable("WorkTaskNode");
                });

            modelBuilder.Entity("WeChatApp.Shared.Entity.WorkTaskNodeItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("CreateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreateUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreateUserUid")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageSources")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("WorkTaskId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("WorkTaskNodeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("WorkTaskNodeId");

                    b.ToTable("WorkTaskNodeItem");
                });

            modelBuilder.Entity("WeChatApp.Shared.Entity.Department", b =>
                {
                    b.HasOne("WeChatApp.Shared.Entity.Department", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("WeChatApp.Shared.Entity.WorkTaskNode", b =>
                {
                    b.HasOne("WeChatApp.Shared.Entity.WorkTask", null)
                        .WithMany("Nodes")
                        .HasForeignKey("WorkTaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WeChatApp.Shared.Entity.WorkTaskNodeItem", b =>
                {
                    b.HasOne("WeChatApp.Shared.Entity.WorkTaskNode", null)
                        .WithMany("Items")
                        .HasForeignKey("WorkTaskNodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WeChatApp.Shared.Entity.Department", b =>
                {
                    b.Navigation("Children");
                });

            modelBuilder.Entity("WeChatApp.Shared.Entity.WorkTask", b =>
                {
                    b.Navigation("Nodes");
                });

            modelBuilder.Entity("WeChatApp.Shared.Entity.WorkTaskNode", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
