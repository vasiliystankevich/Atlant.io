namespace Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vasiliystan_e6bb5ab2aad541d79446e4b93002540a : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Guid(),
                        IsActivate = c.Boolean(),
                        AccountName = c.String(),
                        Description = c.String(),
                        Email = c.String(),
                        Role = c.String(),
                        Note = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EnumTypes",
                c => new
                    {
                        RowId = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Id = c.Guid(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.RowId)
                .Index(t => t.Id, unique: true);
            
            CreateTable(
                "dbo.EnumValues",
                c => new
                    {
                        RowId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.Int(nullable: false),
                        Id = c.Guid(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Type_RowId = c.Int(),
                    })
                .PrimaryKey(t => t.RowId)
                .ForeignKey("dbo.EnumTypes", t => t.Type_RowId)
                .Index(t => t.Id, unique: true)
                .Index(t => t.Type_RowId);
            
            CreateTable(
                "dbo.HotWallets",
                c => new
                    {
                        RowId = c.Int(nullable: false, identity: true),
                        Account = c.String(),
                        Address = c.String(),
                        Amount = c.Single(nullable: false),
                        Id = c.Guid(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.RowId)
                .Index(t => t.Id, unique: true);
            
            CreateTable(
                "dbo.IncomingTransactions",
                c => new
                    {
                        RowId = c.Int(nullable: false, identity: true),
                        IsInterrogation = c.Boolean(nullable: false),
                        TxId = c.String(),
                        Confirmations = c.Int(nullable: false),
                        TimeTransaction = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Account = c.String(),
                        Address = c.String(),
                        Amount = c.Single(nullable: false),
                        Id = c.Guid(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        HotWalletModel_RowId = c.Int(),
                    })
                .PrimaryKey(t => t.RowId)
                .ForeignKey("dbo.HotWallets", t => t.HotWalletModel_RowId)
                .Index(t => t.Id, unique: true)
                .Index(t => t.HotWalletModel_RowId);
            
            CreateTable(
                "dbo.OutgoingTransactions",
                c => new
                    {
                        RowId = c.Int(nullable: false, identity: true),
                        TxId = c.String(),
                        Confirmations = c.Int(nullable: false),
                        TimeTransaction = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Account = c.String(),
                        Address = c.String(),
                        Amount = c.Single(nullable: false),
                        Id = c.Guid(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        HotWalletModel_RowId = c.Int(),
                    })
                .PrimaryKey(t => t.RowId)
                .ForeignKey("dbo.HotWallets", t => t.HotWalletModel_RowId)
                .Index(t => t.Id, unique: true)
                .Index(t => t.HotWalletModel_RowId);
            
            CreateTable(
                "dbo.Seeding",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsSeed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SendBtcRequests",
                c => new
                    {
                        RowId = c.Int(nullable: false, identity: true),
                        Address = c.String(),
                        Amount = c.Single(nullable: false),
                        Time = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IsExecute = c.Boolean(nullable: false),
                        Id = c.Guid(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.RowId)
                .Index(t => t.Id, unique: true);
            
            CreateTable(
                "dbo.webpages_Membership",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        CreateDate = c.DateTime(),
                        ConfirmationToken = c.String(maxLength: 128),
                        IsConfirmed = c.Boolean(),
                        LastPasswordFailureDate = c.DateTime(),
                        PasswordFailuresSinceLastSuccess = c.Int(nullable: false),
                        Password = c.String(nullable: false, maxLength: 128),
                        PasswordChangedDate = c.DateTime(),
                        PasswordSalt = c.String(nullable: false, maxLength: 128),
                        PasswordVerificationToken = c.String(maxLength: 128),
                        PasswordVerificationTokenExpirationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.webpages_OAuthMembership",
                c => new
                    {
                        Provider = c.String(nullable: false, maxLength: 30),
                        ProviderUserId = c.String(nullable: false, maxLength: 100),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Provider, t.ProviderUserId });
            
            CreateTable(
                "dbo.webpages_Roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        RoleName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.webpages_UsersInRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.webpages_Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.webpages_UsersInRoles", "RoleId", "dbo.webpages_Roles");
            DropForeignKey("dbo.OutgoingTransactions", "HotWalletModel_RowId", "dbo.HotWallets");
            DropForeignKey("dbo.IncomingTransactions", "HotWalletModel_RowId", "dbo.HotWallets");
            DropForeignKey("dbo.EnumValues", "Type_RowId", "dbo.EnumTypes");
            DropIndex("dbo.webpages_UsersInRoles", new[] { "RoleId" });
            DropIndex("dbo.SendBtcRequests", new[] { "Id" });
            DropIndex("dbo.OutgoingTransactions", new[] { "HotWalletModel_RowId" });
            DropIndex("dbo.OutgoingTransactions", new[] { "Id" });
            DropIndex("dbo.IncomingTransactions", new[] { "HotWalletModel_RowId" });
            DropIndex("dbo.IncomingTransactions", new[] { "Id" });
            DropIndex("dbo.HotWallets", new[] { "Id" });
            DropIndex("dbo.EnumValues", new[] { "Type_RowId" });
            DropIndex("dbo.EnumValues", new[] { "Id" });
            DropIndex("dbo.EnumTypes", new[] { "Id" });
            DropTable("dbo.webpages_UsersInRoles");
            DropTable("dbo.webpages_Roles");
            DropTable("dbo.webpages_OAuthMembership");
            DropTable("dbo.webpages_Membership");
            DropTable("dbo.SendBtcRequests");
            DropTable("dbo.Seeding");
            DropTable("dbo.OutgoingTransactions");
            DropTable("dbo.IncomingTransactions");
            DropTable("dbo.HotWallets");
            DropTable("dbo.EnumValues");
            DropTable("dbo.EnumTypes");
            DropTable("dbo.Accounts");
        }
    }
}
