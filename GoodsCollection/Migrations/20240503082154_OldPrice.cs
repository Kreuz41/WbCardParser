using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoodsCollection.Migrations
{
    /// <inheritdoc />
    public partial class OldPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OldPrice",
                table: "Goods",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OldPrice",
                table: "Goods");
        }
    }
}
