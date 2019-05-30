namespace Aplikacja.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOpinionsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Opinions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Body = c.String(),
                        AuthId = c.String(),
                        AuthName = c.String(),
                        EnrollmentDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Opinions");
        }
    }
}
