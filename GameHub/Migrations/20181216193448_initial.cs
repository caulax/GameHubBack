using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GameHub.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chooser",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SceneId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chooser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PlayersCount = table.Column<int>(nullable: false),
                    StartSceneId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Choice",
                columns: table => new
                {
                    ChoiceId = table.Column<Guid>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    ChooserId = table.Column<Guid>(nullable: false),
                    Caption = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Choice", x => x.ChoiceId);
                    table.ForeignKey(
                        name: "FK_Choice_Chooser_ChooserId",
                        column: x => x.ChooserId,
                        principalTable: "Chooser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatrixVariant",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ChooserId = table.Column<Guid>(nullable: false),
                    MatrixPosition = table.Column<string>(nullable: true),
                    Value = table.Column<int>(nullable: false),
                    NextSceneId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatrixVariant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatrixVariant_Chooser_ChooserId",
                        column: x => x.ChooserId,
                        principalTable: "Chooser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImageResources",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GameId = table.Column<Guid>(nullable: false),
                    Path = table.Column<string>(nullable: true),
                    Image = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageResources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageResources_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Scenes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GameId = table.Column<Guid>(nullable: false),
                    ChooserId = table.Column<Guid>(nullable: false),
                    ChooserId1 = table.Column<Guid>(nullable: true),
                    BackgroundId = table.Column<Guid>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    WaitForOthers = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scenes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scenes_Chooser_ChooserId1",
                        column: x => x.ChooserId1,
                        principalTable: "Chooser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Scenes_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sprite",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SceneId = table.Column<Guid>(nullable: false),
                    PositionX = table.Column<int>(nullable: false),
                    PositionY = table.Column<int>(nullable: false),
                    ResourceId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sprite", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sprite_Scenes_SceneId",
                        column: x => x.SceneId,
                        principalTable: "Scenes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Choice_ChooserId",
                table: "Choice",
                column: "ChooserId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageResources_GameId",
                table: "ImageResources",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_MatrixVariant_ChooserId",
                table: "MatrixVariant",
                column: "ChooserId");

            migrationBuilder.CreateIndex(
                name: "IX_Scenes_ChooserId1",
                table: "Scenes",
                column: "ChooserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Scenes_GameId",
                table: "Scenes",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Sprite_SceneId",
                table: "Sprite",
                column: "SceneId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Choice");

            migrationBuilder.DropTable(
                name: "ImageResources");

            migrationBuilder.DropTable(
                name: "MatrixVariant");

            migrationBuilder.DropTable(
                name: "Sprite");

            migrationBuilder.DropTable(
                name: "Scenes");

            migrationBuilder.DropTable(
                name: "Chooser");

            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
