﻿// <auto-generated />
using System;
using Commitments.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Commitments.Infrastructure.Migrations
{
    [DbContext(typeof(CommitmentsDbContext))]
    [Migration("20230318104959_Commitments_Initial")]
    partial class Commitments_Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Commitments")
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Commitments.Core.AggregateModel.ActivityAggregate.Activity", b =>
                {
                    b.Property<Guid>("ActivityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BehaviourId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("PerformedOn")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ProfileId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ActivityId");

                    b.HasIndex("BehaviourId");

                    b.HasIndex("ProfileId");

                    b.ToTable("Activities", "Commitments");
                });

            modelBuilder.Entity("Commitments.Core.AggregateModel.BehaviourAggregate.Behaviour", b =>
                {
                    b.Property<Guid>("BehaviourId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BehaviourTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Slug")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BehaviourId");

                    b.HasIndex("BehaviourTypeId");

                    b.ToTable("Behaviours", "Commitments");
                });

            modelBuilder.Entity("Commitments.Core.AggregateModel.BehaviourTypeAggregate.BehaviourType", b =>
                {
                    b.Property<Guid>("BehaviourTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BehaviourTypeId");

                    b.ToTable("BehaviourTypes", "Commitments");
                });

            modelBuilder.Entity("Commitments.Core.AggregateModel.CardAggregate.Card", b =>
                {
                    b.Property<Guid>("CardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CardId");

                    b.ToTable("Cards", "Commitments");
                });

            modelBuilder.Entity("Commitments.Core.AggregateModel.CardLayoutAggregate.CardLayout", b =>
                {
                    b.Property<Guid>("CardLayoutId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CardLayoutId");

                    b.ToTable("CardLayouts", "Commitments");
                });

            modelBuilder.Entity("Commitments.Core.AggregateModel.CommitmentAggregate.Commitment", b =>
                {
                    b.Property<Guid>("CommitmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BehaviourId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ProfileId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("CommitmentId");

                    b.HasIndex("BehaviourId");

                    b.ToTable("Commitment", "Commitments");
                });

            modelBuilder.Entity("Commitments.Core.AggregateModel.CommitmentAggregate.CommitmentFrequency", b =>
                {
                    b.Property<Guid>("CommitmentFrequencyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CommitmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("FrequencyId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("CommitmentFrequencyId");

                    b.HasIndex("CommitmentId");

                    b.HasIndex("FrequencyId");

                    b.ToTable("CommitmentFrequency", "Commitments");
                });

            modelBuilder.Entity("Commitments.Core.AggregateModel.CommitmentAggregate.CommitmentPreCondition", b =>
                {
                    b.Property<Guid>("CommitmentPreConditionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CommitmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CommitmentPreConditionId");

                    b.HasIndex("CommitmentId");

                    b.ToTable("CommitmentPreCondition", "Commitments");
                });

            modelBuilder.Entity("Commitments.Core.AggregateModel.DashboardAggregate.Dashboard", b =>
                {
                    b.Property<Guid>("DashboardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ProfileId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("DashboardId");

                    b.ToTable("Dashboards", "Commitments");
                });

            modelBuilder.Entity("Commitments.Core.AggregateModel.DashboardCardAggregate.DashboardCard", b =>
                {
                    b.Property<Guid>("DashboardCardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CardId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CardLayoutId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("DashboardId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Options")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DashboardCardId");

                    b.HasIndex("CardId");

                    b.HasIndex("CardLayoutId");

                    b.HasIndex("DashboardId");

                    b.ToTable("DashboardCards", "Commitments");
                });

            modelBuilder.Entity("Commitments.Core.AggregateModel.DigitalAssetAggregate.DigitalAsset", b =>
                {
                    b.Property<Guid>("DigitalAssetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("newsequentialid()");

                    b.Property<byte[]>("Bytes")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("ContentType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DigitalAssetId");

                    b.ToTable("DigitalAssets", "Commitments");
                });

            modelBuilder.Entity("Commitments.Core.AggregateModel.FrequencyAggregate.Frequency", b =>
                {
                    b.Property<Guid>("FrequencyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("Frequency")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FrequencyTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDesirable")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("FrequencyId");

                    b.HasIndex("FrequencyTypeId");

                    b.ToTable("Frequencies", "Commitments");
                });

            modelBuilder.Entity("Commitments.Core.AggregateModel.FrequencyTypeAggregate.FrequencyType", b =>
                {
                    b.Property<Guid>("FrequencyTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FrequencyTypeId");

                    b.ToTable("FrequencyTypes", "Commitments");
                });

            modelBuilder.Entity("Commitments.Core.AggregateModel.NoteAggregate.Note", b =>
                {
                    b.Property<Guid>("NoteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Body")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Slug")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("NoteId");

                    b.ToTable("Notes", "Commitments");
                });

            modelBuilder.Entity("Commitments.Core.AggregateModel.NoteAggregate.NoteTag", b =>
                {
                    b.Property<Guid>("NoteTagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("NoteId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TagId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("NoteTagId");

                    b.HasIndex("NoteId");

                    b.HasIndex("TagId");

                    b.ToTable("NoteTag", "Commitments");
                });

            modelBuilder.Entity("Commitments.Core.AggregateModel.ProfileAggregate.Profile", b =>
                {
                    b.Property<Guid>("ProfileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("ProfileId");

                    b.ToTable("Profiles", "Commitments");
                });

            modelBuilder.Entity("Commitments.Core.AggregateModel.TagAggregate.Tag", b =>
                {
                    b.Property<Guid>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Slug")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TagId");

                    b.ToTable("Tags", "Commitments");
                });

            modelBuilder.Entity("Commitments.Core.AggregateModel.ToDoAggregate.ToDo", b =>
                {
                    b.Property<Guid>("ToDoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CompletedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DueOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ProfileId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ToDoId");

                    b.HasIndex("ProfileId");

                    b.ToTable("ToDos", "Commitments");
                });

            modelBuilder.Entity("Commitments.Core.AggregateModel.ActivityAggregate.Activity", b =>
                {
                    b.HasOne("Commitments.Core.AggregateModel.BehaviourAggregate.Behaviour", "Behaviour")
                        .WithMany()
                        .HasForeignKey("BehaviourId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Commitments.Core.AggregateModel.ProfileAggregate.Profile", "Profile")
                        .WithMany()
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Behaviour");

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("Commitments.Core.AggregateModel.BehaviourAggregate.Behaviour", b =>
                {
                    b.HasOne("Commitments.Core.AggregateModel.BehaviourTypeAggregate.BehaviourType", "BehaviourType")
                        .WithMany("Behaviours")
                        .HasForeignKey("BehaviourTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BehaviourType");
                });

            modelBuilder.Entity("Commitments.Core.AggregateModel.CommitmentAggregate.Commitment", b =>
                {
                    b.HasOne("Commitments.Core.AggregateModel.BehaviourAggregate.Behaviour", "Behaviour")
                        .WithMany("Commitments")
                        .HasForeignKey("BehaviourId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Behaviour");
                });

            modelBuilder.Entity("Commitments.Core.AggregateModel.CommitmentAggregate.CommitmentFrequency", b =>
                {
                    b.HasOne("Commitments.Core.AggregateModel.CommitmentAggregate.Commitment", "Commitment")
                        .WithMany("CommitmentFrequencies")
                        .HasForeignKey("CommitmentId");

                    b.HasOne("Commitments.Core.AggregateModel.FrequencyAggregate.Frequency", "Frequency")
                        .WithMany("CommitmentFrequencies")
                        .HasForeignKey("FrequencyId");

                    b.Navigation("Commitment");

                    b.Navigation("Frequency");
                });

            modelBuilder.Entity("Commitments.Core.AggregateModel.CommitmentAggregate.CommitmentPreCondition", b =>
                {
                    b.HasOne("Commitments.Core.AggregateModel.CommitmentAggregate.Commitment", "Commitment")
                        .WithMany("CommitmentPreConditions")
                        .HasForeignKey("CommitmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Commitment");
                });

            modelBuilder.Entity("Commitments.Core.AggregateModel.DashboardCardAggregate.DashboardCard", b =>
                {
                    b.HasOne("Commitments.Core.AggregateModel.CardAggregate.Card", "Card")
                        .WithMany()
                        .HasForeignKey("CardId");

                    b.HasOne("Commitments.Core.AggregateModel.CardLayoutAggregate.CardLayout", "CardLayout")
                        .WithMany()
                        .HasForeignKey("CardLayoutId");

                    b.HasOne("Commitments.Core.AggregateModel.DashboardAggregate.Dashboard", "Dashboard")
                        .WithMany("DashboardCards")
                        .HasForeignKey("DashboardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");

                    b.Navigation("CardLayout");

                    b.Navigation("Dashboard");
                });

            modelBuilder.Entity("Commitments.Core.AggregateModel.FrequencyAggregate.Frequency", b =>
                {
                    b.HasOne("Commitments.Core.AggregateModel.FrequencyTypeAggregate.FrequencyType", "FrequencyType")
                        .WithMany()
                        .HasForeignKey("FrequencyTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FrequencyType");
                });

            modelBuilder.Entity("Commitments.Core.AggregateModel.NoteAggregate.NoteTag", b =>
                {
                    b.HasOne("Commitments.Core.AggregateModel.NoteAggregate.Note", "Note")
                        .WithMany("NoteTags")
                        .HasForeignKey("NoteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Commitments.Core.AggregateModel.TagAggregate.Tag", "Tag")
                        .WithMany("NoteTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Note");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("Commitments.Core.AggregateModel.ToDoAggregate.ToDo", b =>
                {
                    b.HasOne("Commitments.Core.AggregateModel.ProfileAggregate.Profile", "Profile")
                        .WithMany()
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("Commitments.Core.AggregateModel.BehaviourAggregate.Behaviour", b =>
                {
                    b.Navigation("Commitments");
                });

            modelBuilder.Entity("Commitments.Core.AggregateModel.BehaviourTypeAggregate.BehaviourType", b =>
                {
                    b.Navigation("Behaviours");
                });

            modelBuilder.Entity("Commitments.Core.AggregateModel.CommitmentAggregate.Commitment", b =>
                {
                    b.Navigation("CommitmentFrequencies");

                    b.Navigation("CommitmentPreConditions");
                });

            modelBuilder.Entity("Commitments.Core.AggregateModel.DashboardAggregate.Dashboard", b =>
                {
                    b.Navigation("DashboardCards");
                });

            modelBuilder.Entity("Commitments.Core.AggregateModel.FrequencyAggregate.Frequency", b =>
                {
                    b.Navigation("CommitmentFrequencies");
                });

            modelBuilder.Entity("Commitments.Core.AggregateModel.NoteAggregate.Note", b =>
                {
                    b.Navigation("NoteTags");
                });

            modelBuilder.Entity("Commitments.Core.AggregateModel.TagAggregate.Tag", b =>
                {
                    b.Navigation("NoteTags");
                });
#pragma warning restore 612, 618
        }
    }
}