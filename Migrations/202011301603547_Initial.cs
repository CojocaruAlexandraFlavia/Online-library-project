namespace Proiect_DAW2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        AuthorId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        IsChecked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AuthorId);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        BookId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Price = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                        Status = c.String(),
                        PublishingHouse = c.String(nullable: false),
                        Discount = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookId)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Exemplaries",
                c => new
                    {
                        ExemplaryId = c.Int(nullable: false, identity: true),
                        BookId = c.Int(nullable: false),
                        OrderId = c.Int(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.ExemplaryId)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.BookId)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        ClientId = c.Int(nullable: false),
                        Adress = c.String(),
                        OrderDate = c.DateTime(nullable: false),
                        OrderStatus = c.String(),
                    })
                .PrimaryKey(t => t.OrderId);
            
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        ReservationId = c.Int(nullable: false, identity: true),
                        PlacementDate = c.DateTime(nullable: false),
                        Deadline = c.DateTime(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.ReservationId);
            
            CreateTable(
                "dbo.BookAuthors",
                c => new
                    {
                        Book_BookId = c.Int(nullable: false),
                        Author_AuthorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Book_BookId, t.Author_AuthorId })
                .ForeignKey("dbo.Books", t => t.Book_BookId, cascadeDelete: true)
                .ForeignKey("dbo.Authors", t => t.Author_AuthorId, cascadeDelete: true)
                .Index(t => t.Book_BookId)
                .Index(t => t.Author_AuthorId);
            
            CreateTable(
                "dbo.ReservationExemplaries",
                c => new
                    {
                        Reservation_ReservationId = c.Int(nullable: false),
                        Exemplary_ExemplaryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Reservation_ReservationId, t.Exemplary_ExemplaryId })
                .ForeignKey("dbo.Reservations", t => t.Reservation_ReservationId, cascadeDelete: true)
                .ForeignKey("dbo.Exemplaries", t => t.Exemplary_ExemplaryId, cascadeDelete: true)
                .Index(t => t.Reservation_ReservationId)
                .Index(t => t.Exemplary_ExemplaryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReservationExemplaries", "Exemplary_ExemplaryId", "dbo.Exemplaries");
            DropForeignKey("dbo.ReservationExemplaries", "Reservation_ReservationId", "dbo.Reservations");
            DropForeignKey("dbo.Exemplaries", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Exemplaries", "BookId", "dbo.Books");
            DropForeignKey("dbo.Books", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.BookAuthors", "Author_AuthorId", "dbo.Authors");
            DropForeignKey("dbo.BookAuthors", "Book_BookId", "dbo.Books");
            DropIndex("dbo.ReservationExemplaries", new[] { "Exemplary_ExemplaryId" });
            DropIndex("dbo.ReservationExemplaries", new[] { "Reservation_ReservationId" });
            DropIndex("dbo.BookAuthors", new[] { "Author_AuthorId" });
            DropIndex("dbo.BookAuthors", new[] { "Book_BookId" });
            DropIndex("dbo.Exemplaries", new[] { "OrderId" });
            DropIndex("dbo.Exemplaries", new[] { "BookId" });
            DropIndex("dbo.Books", new[] { "CategoryId" });
            DropTable("dbo.ReservationExemplaries");
            DropTable("dbo.BookAuthors");
            DropTable("dbo.Reservations");
            DropTable("dbo.Orders");
            DropTable("dbo.Exemplaries");
            DropTable("dbo.Categories");
            DropTable("dbo.Books");
            DropTable("dbo.Authors");
        }
    }
}
