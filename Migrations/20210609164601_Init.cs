using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace UnchainedBackend.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    PublicAddress = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Signature = table.Column<string>(type: "text", nullable: true),
                    Bio = table.Column<string>(type: "text", nullable: true),
                    ProfilePic = table.Column<string>(type: "text", nullable: true),
                    IsArtist = table.Column<bool>(type: "boolean", nullable: false),
                    Verified = table.Column<bool>(type: "boolean", nullable: false),
                    Twitter = table.Column<string>(type: "text", nullable: true),
                    Facebook = table.Column<string>(type: "text", nullable: true),
                    Instagram = table.Column<string>(type: "text", nullable: true),
                    LinkedIn = table.Column<string>(type: "text", nullable: true),
                    SoundCloud = table.Column<string>(type: "text", nullable: true),
                    BandCamp = table.Column<string>(type: "text", nullable: true),
                    Beatport = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.PublicAddress);
                });

            migrationBuilder.CreateTable(
                name: "PendingArtists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ArtistPublicAddress = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingArtists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PendingArtists_Users_ArtistPublicAddress",
                        column: x => x.ArtistPublicAddress,
                        principalTable: "Users",
                        principalColumn: "PublicAddress",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tracks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ImageLocation = table.Column<string>(type: "text", nullable: true),
                    FileLocation = table.Column<string>(type: "text", nullable: true),
                    IsMinted = table.Column<bool>(type: "boolean", nullable: false),
                    IsAuctioned = table.Column<bool>(type: "boolean", nullable: false),
                    IsListed = table.Column<bool>(type: "boolean", nullable: false),
                    IsSold = table.Column<bool>(type: "boolean", nullable: false),
                    TokenId = table.Column<int>(type: "integer", nullable: false),
                    OwnerOfPublicAddress = table.Column<string>(type: "text", nullable: true),
                    AuctionId = table.Column<string>(type: "text", nullable: true),
                    ListingId = table.Column<string>(type: "text", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tracks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tracks_Users_OwnerOfPublicAddress",
                        column: x => x.OwnerOfPublicAddress,
                        principalTable: "Users",
                        principalColumn: "PublicAddress",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Auctions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Started = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Ending = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsEnded = table.Column<bool>(type: "boolean", nullable: false),
                    TrackId = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    ContractAddress = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auctions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auctions_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Listings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    TrackId = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Listings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Listings_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bids",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OwnerOfPublicAddress = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<double>(type: "double precision", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AuctionId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bids", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bids_Auctions_AuctionId",
                        column: x => x.AuctionId,
                        principalTable: "Auctions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bids_Users_OwnerOfPublicAddress",
                        column: x => x.OwnerOfPublicAddress,
                        principalTable: "Users",
                        principalColumn: "PublicAddress",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_TrackId",
                table: "Auctions",
                column: "TrackId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bids_AuctionId",
                table: "Bids",
                column: "AuctionId");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_OwnerOfPublicAddress",
                table: "Bids",
                column: "OwnerOfPublicAddress");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_TrackId",
                table: "Listings",
                column: "TrackId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PendingArtists_ArtistPublicAddress",
                table: "PendingArtists",
                column: "ArtistPublicAddress");

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_OwnerOfPublicAddress",
                table: "Tracks",
                column: "OwnerOfPublicAddress");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bids");

            migrationBuilder.DropTable(
                name: "Listings");

            migrationBuilder.DropTable(
                name: "PendingArtists");

            migrationBuilder.DropTable(
                name: "Auctions");

            migrationBuilder.DropTable(
                name: "Tracks");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
