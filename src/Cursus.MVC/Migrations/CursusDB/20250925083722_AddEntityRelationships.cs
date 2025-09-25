using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cursus.MVC.Migrations.CursusDB
{
    /// <inheritdoc />
    public partial class AddEntityRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Subscribes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InstructorId",
                table: "Subscribes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccId",
                table: "Progresses",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Accounts_AccountId",
                table: "Accounts",
                column: "AccountId");

            migrationBuilder.CreateTable(
                name: "CourseDiscounts",
                columns: table => new
                {
                    CoursesCourseId = table.Column<int>(type: "int", nullable: false),
                    DiscountsDiscountId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseDiscounts", x => new { x.CoursesCourseId, x.DiscountsDiscountId });
                    table.ForeignKey(
                        name: "FK_CourseDiscounts_Courses_CoursesCourseId",
                        column: x => x.CoursesCourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseDiscounts_Discounts_DiscountsDiscountId",
                        column: x => x.DiscountsDiscountId,
                        principalTable: "Discounts",
                        principalColumn: "DiscountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscribes_InstructorId",
                table: "Subscribes",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscribes_UserId",
                table: "Subscribes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_AccountId",
                table: "Reports",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CmtId",
                table: "Reports",
                column: "CmtId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CourseId",
                table: "Reports",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Progresses_AccId",
                table: "Progresses",
                column: "AccId");

            migrationBuilder.CreateIndex(
                name: "IX_Progresses_LessonId",
                table: "Progresses",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseDiscounts_DiscountsDiscountId",
                table: "CourseDiscounts",
                column: "DiscountsDiscountId");

            migrationBuilder.AddForeignKey(
                name: "FK_InstructorSubscriptions_Accounts_InstructorId",
                table: "InstructorSubscriptions",
                column: "InstructorId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Progresses_Accounts_AccId",
                table: "Progresses",
                column: "AccId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Progresses_Lessons_LessonId",
                table: "Progresses",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "LessionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Accounts_AccountId",
                table: "Reports",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Comments_CmtId",
                table: "Reports",
                column: "CmtId",
                principalTable: "Comments",
                principalColumn: "CmtId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Courses_CourseId",
                table: "Reports",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscribes_Accounts_InstructorId",
                table: "Subscribes",
                column: "InstructorId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscribes_Accounts_UserId",
                table: "Subscribes",
                column: "UserId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstructorSubscriptions_Accounts_InstructorId",
                table: "InstructorSubscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Progresses_Accounts_AccId",
                table: "Progresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Progresses_Lessons_LessonId",
                table: "Progresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Accounts_AccountId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Comments_CmtId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Courses_CourseId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscribes_Accounts_InstructorId",
                table: "Subscribes");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscribes_Accounts_UserId",
                table: "Subscribes");

            migrationBuilder.DropTable(
                name: "CourseDiscounts");

            migrationBuilder.DropIndex(
                name: "IX_Subscribes_InstructorId",
                table: "Subscribes");

            migrationBuilder.DropIndex(
                name: "IX_Subscribes_UserId",
                table: "Subscribes");

            migrationBuilder.DropIndex(
                name: "IX_Reports_AccountId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_CmtId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_CourseId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Progresses_AccId",
                table: "Progresses");

            migrationBuilder.DropIndex(
                name: "IX_Progresses_LessonId",
                table: "Progresses");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Accounts_AccountId",
                table: "Accounts");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Subscribes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InstructorId",
                table: "Subscribes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccId",
                table: "Progresses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
