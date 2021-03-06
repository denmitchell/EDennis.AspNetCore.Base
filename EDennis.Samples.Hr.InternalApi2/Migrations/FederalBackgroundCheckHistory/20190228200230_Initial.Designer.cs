﻿// <auto-generated />
using System;
using EDennis.Samples.Hr.InternalApi2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EDennis.Samples.Hr.InternalApi2.Migrations.FederalBackgroundCheckHistory
{
    [DbContext(typeof(FederalBackgroundCheckHistoryContext))]
    [Migration("20190228200230_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EDennis.Samples.Hr.InternalApi2.Models.FederalBackgroundCheck", b =>
                {
                    b.Property<int>("Id");

                    b.Property<DateTime>("SysStart");

                    b.Property<DateTime>("DateCompleted");

                    b.Property<int>("EmployeeId");

                    b.Property<string>("Status");

                    b.Property<DateTime>("SysEnd");

                    b.Property<string>("SysUser");

                    b.Property<string>("SysUserNext");

                    b.HasKey("Id", "SysStart");

                    b.ToTable("FederalBackgroundCheck","dbo_history");
                });
#pragma warning restore 612, 618
        }
    }
}
