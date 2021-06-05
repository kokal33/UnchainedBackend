﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using UnchainedBackend.Data;

namespace UnchainedBackend.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("UnchainedBackend.Models.Auction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ContractAddress")
                        .HasColumnType("text");

                    b.Property<DateTime>("Ending")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsEnded")
                        .HasColumnType("boolean");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("Started")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("TrackId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TrackId")
                        .IsUnique();

                    b.ToTable("Auctions");
                });

            modelBuilder.Entity("UnchainedBackend.Models.Bid", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<double>("Amount")
                        .HasColumnType("double precision");

                    b.Property<int?>("AuctionId")
                        .HasColumnType("integer");

                    b.Property<string>("OwnerOfPublicAddress")
                        .HasColumnType("text");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("AuctionId");

                    b.HasIndex("OwnerOfPublicAddress");

                    b.ToTable("Bids");
                });

            modelBuilder.Entity("UnchainedBackend.Models.Listing", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<int>("TrackId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TrackId")
                        .IsUnique();

                    b.ToTable("Listings");
                });

            modelBuilder.Entity("UnchainedBackend.Models.PendingArtist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ArtistPublicAddress")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ArtistPublicAddress");

                    b.ToTable("PendingArtists");
                });

            modelBuilder.Entity("UnchainedBackend.Models.Track", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AuctionId")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("FileLocation")
                        .HasColumnType("text");

                    b.Property<string>("ImageLocation")
                        .HasColumnType("text");

                    b.Property<bool>("IsAuctioned")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsListed")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsMinted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsSold")
                        .HasColumnType("boolean");

                    b.Property<string>("ListingId")
                        .HasColumnType("text");

                    b.Property<string>("OwnerOfPublicAddress")
                        .HasColumnType("text");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<int>("TokenId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OwnerOfPublicAddress");

                    b.ToTable("Tracks");
                });

            modelBuilder.Entity("UnchainedBackend.Models.User", b =>
                {
                    b.Property<string>("PublicAddress")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<string>("BandCamp")
                        .HasColumnType("text");

                    b.Property<string>("Beatport")
                        .HasColumnType("text");

                    b.Property<string>("Bio")
                        .HasColumnType("text");

                    b.Property<string>("Facebook")
                        .HasColumnType("text");

                    b.Property<string>("Instagram")
                        .HasColumnType("text");

                    b.Property<bool>("IsArtist")
                        .HasColumnType("boolean");

                    b.Property<string>("LinkedIn")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("ProfilePic")
                        .HasColumnType("text");

                    b.Property<string>("Signature")
                        .HasColumnType("text");

                    b.Property<string>("SoundCloud")
                        .HasColumnType("text");

                    b.Property<string>("Twitter")
                        .HasColumnType("text");

                    b.Property<bool>("Verified")
                        .HasColumnType("boolean");

                    b.HasKey("PublicAddress");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("UnchainedBackend.Models.Auction", b =>
                {
                    b.HasOne("UnchainedBackend.Models.Track", "Track")
                        .WithOne("Auction")
                        .HasForeignKey("UnchainedBackend.Models.Auction", "TrackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Track");
                });

            modelBuilder.Entity("UnchainedBackend.Models.Bid", b =>
                {
                    b.HasOne("UnchainedBackend.Models.Auction", null)
                        .WithMany("Bids")
                        .HasForeignKey("AuctionId");

                    b.HasOne("UnchainedBackend.Models.User", "OwnerOf")
                        .WithMany()
                        .HasForeignKey("OwnerOfPublicAddress");

                    b.Navigation("OwnerOf");
                });

            modelBuilder.Entity("UnchainedBackend.Models.Listing", b =>
                {
                    b.HasOne("UnchainedBackend.Models.Track", "Track")
                        .WithOne("Listing")
                        .HasForeignKey("UnchainedBackend.Models.Listing", "TrackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Track");
                });

            modelBuilder.Entity("UnchainedBackend.Models.PendingArtist", b =>
                {
                    b.HasOne("UnchainedBackend.Models.User", "Artist")
                        .WithMany()
                        .HasForeignKey("ArtistPublicAddress");

                    b.Navigation("Artist");
                });

            modelBuilder.Entity("UnchainedBackend.Models.Track", b =>
                {
                    b.HasOne("UnchainedBackend.Models.User", "OwnerOf")
                        .WithMany()
                        .HasForeignKey("OwnerOfPublicAddress");

                    b.Navigation("OwnerOf");
                });

            modelBuilder.Entity("UnchainedBackend.Models.Auction", b =>
                {
                    b.Navigation("Bids");
                });

            modelBuilder.Entity("UnchainedBackend.Models.Track", b =>
                {
                    b.Navigation("Auction");

                    b.Navigation("Listing");
                });
#pragma warning restore 612, 618
        }
    }
}
