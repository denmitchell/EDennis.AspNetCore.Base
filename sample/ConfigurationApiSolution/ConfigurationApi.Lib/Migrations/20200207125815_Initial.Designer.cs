﻿// <auto-generated />
using System;
using ConfigurationApi.Lib.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ConfigurationApi.Lib.Migrations
{
    [DbContext(typeof(ConfigurationDbContext))]
    [Migration("20200207125815_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ConfigurationApi.Lib.Models.ProjectSetting", b =>
                {
                    b.Property<string>("ProjectName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SettingKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SettingValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SysEnd")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("(CONVERT(datetime2, '9999-12-31 23:59:59.9999999'))");

                    b.Property<DateTime>("SysStart")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("(getdate())");

                    b.HasKey("ProjectName", "SettingKey");

                    b.ToTable("ProjectSetting");
                });
#pragma warning restore 612, 618
        }
    }
}
