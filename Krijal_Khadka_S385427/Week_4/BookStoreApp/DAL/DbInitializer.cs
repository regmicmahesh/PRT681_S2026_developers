namespace BookStoreApp.DAL;

public static class DbInitializer
{
    public static void Seed(EfBookStoreContext context)
    {
        if (!context.Books.Any(book =>
                book.Name == "The Great Gatsby"))
        {
            context.Books.Add(
                new Book { Name = "The Great Gatsby" });
        }

        if (!context.Books.Any(book =>
                book.Name == "Clean Code"))
        {
            context.Books.Add(
                new Book { Name = "Clean Code" });
        }

        if (!context.Books.Any(book =>
                book.Name == "The Pragmatic Programmer"))
        {
            context.Books.Add(
                new Book { Name = "The Pragmatic Programmer" });
        }

        if (!context.Printers.Any(printer =>
                printer.Name == "Office Printer"))
        {
            context.Printers.Add(new Printer
            {
                Name = "Office Printer",
                Brand = "HP",
                Model = "LaserJet Pro M404"
            });
        }

        if (!context.Printers.Any(printer =>
                printer.Name == "Photo Printer"))
        {
            context.Printers.Add(new Printer
            {
                Name = "Photo Printer",
                Brand = "Canon",
                Model = "PIXMA G660"
            });
        }

        if (!context.Printers.Any(printer =>
                printer.Name == "Home Printer"))
        {
            context.Printers.Add(new Printer
            {
                Name = "Home Printer",
                Brand = "Epson",
                Model = "EcoTank ET-2850"
            });
        }

        context.SaveChanges();
    }
}