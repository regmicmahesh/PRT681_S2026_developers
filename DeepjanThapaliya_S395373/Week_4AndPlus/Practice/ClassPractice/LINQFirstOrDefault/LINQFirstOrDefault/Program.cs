
List<Product> products = new List<Product>
{
    new Product(1, "Laptop", "Electronics", 1500m, 5),
    new Product(2, "Mouse", "Electronics", 25m, 50),
    new Product(3, "Keyboard", "Electronics", 80m, 10),
    new Product(4, "Office Chair", "Furniture", 250m, 30),
    new Product(5, "Desk", "Furniture", 400m, 7),
    new Product(6, "Notebook", "Stationery", 5m, 100),
    new Product(7, "Pen", "Stationery", 2m, 200),
    new Product(8, "Monitor", "Electronics", 300m, 0)
};

Product? selectProduct = products.SingleOrDefault(product => product.Id == 5);
Console.WriteLine($"Selected product: {selectProduct}");
Console.WriteLine();

Product? firstElectronic = products.FirstOrDefault(product => product.Category == "Electronics");
Console.WriteLine($"First electronics: {firstElectronic}");
Console.WriteLine();

Product? lastElectronic = products.LastOrDefault(product => product.Category == "Electronics");
Console.WriteLine($"Last electronics: {lastElectronic}");
Console.WriteLine();

bool anyOutOfStock = products.Any(product => product.StockQuantity < 1);
if (anyOutOfStock)
{
    Console.WriteLine("There are out of stock product in the inventory.");
}
else
{
    Console.WriteLine("There are no out of stock product in the inventory.");
}
Console.WriteLine();

bool DoAllProductHasPrice = products.All(product => product.Price > 0);
if (DoAllProductHasPrice)
{
    Console.WriteLine("There are no free products in the inventory.");
}
else
{
    Console.WriteLine("There are free products in the inventory.");
}
Console.WriteLine();

bool AreAllProductInStock = products.All(product => product.StockQuantity > 0);
if (AreAllProductInStock)
{
    Console.WriteLine("There are no out of stock product.");

}
{
    Console.WriteLine("There are  out of stock product");
}
Console.WriteLine();

decimal lowestProductPrice = products.Min(product => product.Price);
Console.WriteLine($"Lowest Product Price: ${lowestProductPrice}");
Console.WriteLine();

decimal lowestPrice2ndWay = products.Select(product => product.Price).Min();
Console.WriteLine($"Lowest Product Price: ${lowestPrice2ndWay}");
Console.WriteLine();

Product? cheapestProduct = products.MinBy(product => product.Price);
Console.WriteLine($"Cheapest Product : {cheapestProduct}");
Console.WriteLine();

decimal highestProductPrice = products.Max(product => product.Price);
Console.WriteLine($"Highest product price: ${highestProductPrice}");
Console.WriteLine();

Product? mostExpensiveProduct = products.MaxBy(product => product.Price);
Console.WriteLine($"Most expensive product detail: {mostExpensiveProduct}");
Console.WriteLine();

decimal avgProductPrice = products.Average(product => product.Price);
Console.WriteLine($"Average product price: ${avgProductPrice}");
Console.WriteLine();

bool HaveFurniture = products.Any(product => product.Category == "Furniture");
if (HaveFurniture)
{
    Console.WriteLine("There are Furniture in product inventory.");
}
else
{
    Console.WriteLine("There are no Furniture in product inventory.");
}
Console.WriteLine();

List<string> outOfStockProduct = products.Where(product => product.StockQuantity == 0).Select(product => product.ProductName).ToList();
foreach(string product in outOfStockProduct)
{
    Console.WriteLine($"Out of Stock Product: {product}");
}
Console.WriteLine();
//Anther way with contain:

List<string> productCategory = products.Select(product => product.Category)
    .Distinct().ToList();
bool hasFurtinure = productCategory.Contains("Furniture");
if (hasFurtinure)
{
    Console.WriteLine("There are Furniture in product inventory.");
}
else
{
    Console.WriteLine("There are no Furniture in product inventory.");
}


decimal totalSumOfAllProductPrice = products.Where(product => product.StockQuantity > 0)
    .Sum(product => product.Price * product.StockQuantity);
Console.WriteLine($"Total  Stock Product price: ${totalSumOfAllProductPrice}");


//List<Product> products = new List<Product>
//{
//    new Product(1, "Laptop", "Electronics", 1500m, 5),
//    new Product(2, "Mouse", "Electronics", 25m, 50),
//    new Product(3, "Keyboard", "Electronics", 80m,10),
//    new Product(4, "Office Chair", "Furniture", 250m, 30),
//    new Product(5, "Desk", "Furniture", 400m, 7),
//    new Product(6, "Notebook", "Stationery", 5m, 100),
//    new Product(7, "Pen", "Stationery", 2m, 200)
//};

//Product? foundProduct = products.FirstOrDefault(product => product.Id == 3);
//{
//    if(foundProduct == null)
//    {
//        throw new ArgumentNullException("Mains Error: No Product found.");
//    }
//    else
//    {
//        Console.WriteLine(foundProduct);
//    }
//}
//Console.WriteLine();

//List<Product> expensiveProduct = products.Where(product => product.Price >= 100m).ToList();
//Console.WriteLine($"Expensive product:");
//foreach (Product product in expensiveProduct)
//{
//    Console.WriteLine($"{product}");
//}
//Console.WriteLine();

//List<Product> electronicProduct = products.Where(product => product.Category == "Electronics").ToList();
//Console.WriteLine($"Electronic product:");
//foreach(Product product in electronicProduct)
//{
//    Console.WriteLine($"{product}");
//}
//Console.WriteLine();

//List<Product> sortProduct = products.OrderBy(product => product.Price).ToList();
//Console.WriteLine($"Ordered list:");
//foreach (Product product in sortProduct)
//{
//    Console.WriteLine($"{product}");
//}
//Console.WriteLine();

//List<Product> sortedProductByStockQuantityInDesc = products.OrderByDescending(products => products.StockQuantity).ToList();
//Console.WriteLine($"Sorted product by quantity in descending order:");
//foreach (Product product in sortedProductByStockQuantityInDesc)
//{
//    Console.WriteLine($"{product}");
//}
//Console.WriteLine();

//List<string> getProductName = products.Select(product => product.ProductName).ToList();
//Console.WriteLine("Product Name");
//foreach(string name in getProductName)
//{
//    Console.WriteLine(name);
//}

//Console.WriteLine($"Total electronic product: {products.Count(product => product.Category == "Electronics")}");
//Console.WriteLine();
//bool isProductExpensive = products.Any(product => product.Price > 1000m);
//if (isProductExpensive == true)
//{
//    Console.WriteLine("There is at least one product" +
//        " priced more than 1000");
//}
//else Console.WriteLine("No product priced more than 1000.");
//Console.WriteLine();

//Console.WriteLine($"Total Stock value: {products.Sum(product => product.StockQuantity * product.Price)}");

////Where() = filter many items
////FirstOrDefault() = find one item
////OrderBy() = sort items
////Select() = transform items
////Count() = count items
////Sum() = total values
////Any() = check if at least one exists
////foreach = do an action for each item
////FirstOrDefault()   -> one object or null
////First()->one object or exception
////SingleOrDefault()  -> one object, null, or exception if duplicate
////LastOrDefault()    -> one object or null
////Find()             -> one object or null, List<T> only
////Contains()         -> bool
////Any()              -> bool
////All()              -> bool
////Count()            -> int
////Sum()              -> number
////Min()              -> number/value
////Max()              -> number/value
////Average()          -> number
////Where()            -> many items
////Select()           -> transformed values
////OrderBy()          -> sorted many items