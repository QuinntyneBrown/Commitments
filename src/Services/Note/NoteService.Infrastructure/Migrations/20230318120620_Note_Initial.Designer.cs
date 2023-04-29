﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NoteService.Infrastructure.Data;

#nullable disable

namespace NoteService.Infrastructure.Migrations
{
    [DbContext(typeof(NoteServiceDbContext))]
    [Migration("20230318120620_Note_Initial")]
    partial class NoteInitial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Note")
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("NoteService.Core.AggregateModel.NoteAggregate.Note", b =>
                {
                    b.Property<Guid>("NoteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("Title")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("NoteId");

                    b.ToTable("Notes", "Note");
                });

            modelBuilder.Entity("NoteService.Core.AggregateModel.TagAggregate.Tag", b =>
                {
                    b.Property<Guid>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Name")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TagId");

                    b.ToTable("Tags", "Note");
                });

            modelBuilder.Entity("NoteTag", b =>
                {
                    b.Property<Guid>("NotesNoteId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TagsTagId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("NotesNoteId", "TagsTagId");

                    b.HasIndex("TagsTagId");

                    b.ToTable("NoteTag", "Note");
                });

            modelBuilder.Entity("NoteTag", b =>
                {
                    b.HasOne("NoteService.Core.AggregateModel.NoteAggregate.Note", null)
                        .WithMany()
                        .HasForeignKey("NotesNoteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NoteService.Core.AggregateModel.TagAggregate.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsTagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}