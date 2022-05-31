using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Salky.Domain.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupConfig",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    MaxUser = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", nullable: false),
                    NormalizedUserName = table.Column<string>(type: "TEXT", nullable: false),
                    PictureSource = table.Column<string>(type: "TEXT", nullable: false),
                    Visibility = table.Column<int>(type: "INTEGER", nullable: false),
                    PassWordHash = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Friend",
                columns: table => new
                {
                    RequestedById = table.Column<Guid>(type: "TEXT", nullable: false),
                    RequestedToId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    RequestTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    BecameFriendsTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    FriendRequestFlag = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friend", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Friend_Users_RequestedById",
                        column: x => x.RequestedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Friend_Users_RequestedToId",
                        column: x => x.RequestedToId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ConfigId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PictureSource = table.Column<string>(type: "TEXT", nullable: false),
                    OwnerId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_GroupConfig_ConfigId",
                        column: x => x.ConfigId,
                        principalTable: "GroupConfig",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Groups_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MessagesFriend",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FriendId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SenderId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessagesFriend", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessagesFriend_Friend_FriendId",
                        column: x => x.FriendId,
                        principalTable: "Friend",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupsRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoleName = table.Column<string>(type: "TEXT", nullable: false),
                    Hierarchy = table.Column<int>(type: "INTEGER", nullable: false),
                    GroupId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupsRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupsRoles_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MessagesGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    GroupId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SenderId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessagesGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessagesGroup_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transference",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    GroupId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transference", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transference_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CallPermisions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    GroupRoleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CanMuteMicrofoneOfOtherUser = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanUnMuteMicrofoneOfOtherUser = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanMuteHeadPhoneOfOtherUser = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanUnMuteHeadPhoneOfOtherUser = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanEntryInCall = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanSeeCall = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallPermisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CallPermisions_GroupsRoles_GroupRoleId",
                        column: x => x.GroupRoleId,
                        principalTable: "GroupsRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatPermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    GroupRoleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CanDeleteOtherUserMessages = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanSendMessage = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanReadMessage = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatPermissions_GroupsRoles_GroupRoleId",
                        column: x => x.GroupRoleId,
                        principalTable: "GroupsRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupPermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    GroupRoleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CanInviteOtherUsers = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanRemoveOtherUsers = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanEditGroupName = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanEditGroupPicture = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanChangeOtherUserRoles = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupPermissions_GroupsRoles_GroupRoleId",
                        column: x => x.GroupRoleId,
                        principalTable: "GroupsRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupsUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    GroupId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoleId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupsUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupsUsers_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupsUsers_GroupsRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "GroupsRoles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GroupsUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CallPermisions_GroupRoleId",
                table: "CallPermisions",
                column: "GroupRoleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatPermissions_GroupRoleId",
                table: "ChatPermissions",
                column: "GroupRoleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Friend_RequestedById",
                table: "Friend",
                column: "RequestedById");

            migrationBuilder.CreateIndex(
                name: "IX_Friend_RequestedToId",
                table: "Friend",
                column: "RequestedToId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupPermissions_GroupRoleId",
                table: "GroupPermissions",
                column: "GroupRoleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_ConfigId",
                table: "Groups",
                column: "ConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_OwnerId",
                table: "Groups",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupsRoles_GroupId",
                table: "GroupsRoles",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupsRoles_RoleName_GroupId",
                table: "GroupsRoles",
                columns: new[] { "RoleName", "GroupId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupsUsers_GroupId",
                table: "GroupsUsers",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupsUsers_RoleId",
                table: "GroupsUsers",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupsUsers_UserId",
                table: "GroupsUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MessagesFriend_FriendId",
                table: "MessagesFriend",
                column: "FriendId");

            migrationBuilder.CreateIndex(
                name: "IX_MessagesGroup_GroupId",
                table: "MessagesGroup",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Transference_GroupId",
                table: "Transference",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CallPermisions");

            migrationBuilder.DropTable(
                name: "ChatPermissions");

            migrationBuilder.DropTable(
                name: "GroupPermissions");

            migrationBuilder.DropTable(
                name: "GroupsUsers");

            migrationBuilder.DropTable(
                name: "MessagesFriend");

            migrationBuilder.DropTable(
                name: "MessagesGroup");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Transference");

            migrationBuilder.DropTable(
                name: "GroupsRoles");

            migrationBuilder.DropTable(
                name: "Friend");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "GroupConfig");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
