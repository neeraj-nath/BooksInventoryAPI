using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BooksInventory.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "Description", "Genre", "ISBN", "Price", "PublicationYear", "Quantity", "Title" },
                values: new object[,]
                {
                    { 1, "Kanchan Baruah", "A clerk dreaming of a perfect married life with kids, who suddenly gets sucked into a black hole", "Fiction", "978-0-061-99437-7", 550, new DateTime(1945, 7, 15, 14, 29, 24, 945, DateTimeKind.Unspecified), 7, "Ashimat Jar Heraal Seema" },
                    { 2, "Harper Lee", "A classic novel that explores racial injustice and moral growth in the American South during the 1930s.", "Fiction", "978-006112-0084", 750, new DateTime(1960, 7, 11, 5, 30, 0, 0, DateTimeKind.Local), 17, "To Kill a Mockingbird" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
