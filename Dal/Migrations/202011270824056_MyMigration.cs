namespace Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MyMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Surname = c.String(),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Repass = c.String(nullable: false),
                        Birthday = c.DateTime(nullable: false),
                        ProfileImage = c.String(),
                        IsAdmin = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        ModifiedUser = c.String(nullable: false),
                        CompanyName = c.String(),
                        CompanyId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Remainders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        adminId = c.Int(nullable: false),
                        remainder = c.Single(nullable: false),
                        paymentDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Remainders");
            DropTable("dbo.CustomerInfoes");
        }
    }
}
