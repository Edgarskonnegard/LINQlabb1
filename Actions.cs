public class Actions
{
    Dictionary<string, Action> actionMap;
    public Actions()
    {
        actionMap = new Dictionary<string, Action>{
        {"Get electronics", () => GetElectronics() },
        {"Get suppliers", () => GetSuppliers() },
        {"Calculate order value of last three months", () => CalculateOrderValue()},
        {"Get most sold products", () => GetMostSold()},
        {"List all categories", () => ListCategories()},
        {"Get orders with details", () => GetOrderByCustomer()}
        };
    }
    public void Menu()
    {
        int currentSelection = 0;
        var actionKeys = actionMap.Keys.ToList();
        ConsoleKey key;
        do{
            Console.Clear();
            foreach(var s in actionMap.Keys)
            {
                Console.ResetColor();
                if(currentSelection == actionKeys.IndexOf(s))
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                System.Console.WriteLine(s);
                Console.ResetColor();
            }

            var keyPressed = Console.ReadKey(intercept: true);
            key = keyPressed.Key;

            if(key == ConsoleKey.DownArrow)
            {
                currentSelection = currentSelection == actionKeys.Count() - 1 ? 0 : currentSelection + 1;
            }
            else if(key == ConsoleKey.UpArrow)
            {
                currentSelection = currentSelection == 0 ? actionKeys.Count() - 1 : currentSelection - 1;
            }
        }while(key != ConsoleKey.Enter);
        Console.Clear();
        System.Console.WriteLine();
        actionMap[actionKeys[currentSelection]]();
        Console.ReadKey(intercept: true);

    }

    public void GetElectronics()
    {
        using(var context = new ECommerceContext())
        {
            var electronics = context.Products
                .Where(p => p.Category.Name == "Electronics")
                .OrderByDescending(p => p.Price)
                .ToList();
            
            foreach(var product in electronics)
            {
                System.Console.WriteLine($"{product.Name}: {product.Price}");
            }
        }
    }

    public void GetSuppliers()
    {
        using(var context = new ECommerceContext())
        {
            var suppliers = context.Products
                .Where(p => p.StockQuantity < 10)
                .Select(p => p.Supplier)
                .Distinct();

            foreach(var supplier in suppliers)
            {
                System.Console.WriteLine($"{supplier.Name}");
            }
        }
    }

    public void CalculateOrderValue()
    {
        using(var context = new ECommerceContext())
        {
            var orderValue = context.Orders
                .Where(o => o.OrderDate.Month >= DateTime.Now.Month-3)
                .Sum(o => o.TotalAmount);
            
            Console.WriteLine($"Sum of orders from the last three months :{orderValue} SEK");
        }
    }

    public void GetMostSold()
    {
        using(var context = new ECommerceContext())
        {
            var mostSold = context.OrderDetails
                .GroupBy(od => od.Product)
                .Select(g => new
                {
                    Product = g.Key,
                    Quantity = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(x => x.Quantity)
                .Take(3)
                .ToList();
            
            foreach(var x in mostSold)
            {
                Console.WriteLine($"Product :{x.Product.Name}, Amount sold : {x.Quantity}");
            }
        }

    }
    public void ListCategories()
    {
        using(var context = new ECommerceContext())
        {
            var categories = context.Categories
                .Select(c => new{
                    c.Name,
                    Amount = c.Products.Count()

                })
                .ToList();
            
            foreach(var category in categories)
            {
                System.Console.WriteLine($"Name: {category.Name}, Amount: {category.Amount}");
            }
        }
    }

    public void GetOrderByCustomer()
    {
        using(var context = new ECommerceContext())
        {
            var ordersWithDetail = context.Orders
                .Where(o => o.TotalAmount > 1000)
                .SelectMany(o => o.OrderDetails, (order, detail) => new 
                {
                    OrderId = order.Id,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    CustomerName = order.Customer.Name,
                    ProductName = detail.Product.Name,
                    Quantity = detail.Quantity,
                    UnitPrice = detail.UnitPrice
                })
                .OrderBy(o => o.OrderId)
                .ToList();
            
            foreach(var o in ordersWithDetail)
            {
                System.Console.WriteLine($"OrderId: {o.OrderId} Customer: {o.CustomerName}, Product: {o.ProductName} {o.UnitPrice}, Quantity: {o.Quantity}, Cost: {o.TotalAmount}, {o.OrderDate}");
            }
        }
    }
}