using E_CommerceSystem.Models;

namespace E_CommerceSystem
{


    internal class Program
    {
        static ApplicationDbContext Db = new ApplicationDbContext();
        static Models.User LoggedInUser;


        public static void checkLogin()
        {
            if (LoggedInUser == null)
            {
                Console.WriteLine("Please login first!");
                
            }
        }


        public static void UserMenu()
        {
            
            Console.WriteLine("choose an option");
            Console.WriteLine("1.Register a new user");
            Console.WriteLine("2.Login ");
            Console.WriteLine("3.Get user");
            int choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:

                    Console.WriteLine("Enter your name:");
                    string name = Console.ReadLine();

                    Console.WriteLine("Enter your Email:");
                    string email = Console.ReadLine();

                    Console.WriteLine("Enter your password:");
                    string password = Console.ReadLine();

                    Console.WriteLine("enter you phone number");
                    string phone = Console.ReadLine();

                    Console.WriteLine("enter your role:");
                    string role = Console.ReadLine();


                    DateTime date = DateTime.Now;
                    Console.WriteLine("the date is :" + date);


                    Db.Users.Add(new Models.User
                    {
                        Name = name,
                        Email = email,
                        Password = password,
                        Phone = phone,
                        Role = role
                    });

                    Db.SaveChanges();

                    Console.WriteLine("User registered successfully!");


                    break;


                case 2:


                    Console.WriteLine("enter your email:");
                    string loginemail = Console.ReadLine();

                    Console.WriteLine("Enter your password:");
                    string loginpassword = Console.ReadLine();

                    var user = Db.Users.FirstOrDefault(u => u.Email == loginemail);

                    if (user == null)
                    {
                        Console.WriteLine("User not found!");
                        break;
                    }

                    if (user.Password != loginpassword)
                    {
                        Console.WriteLine("Wrong password!");
                        break;
                    }

                    LoggedInUser = user;

                    Console.WriteLine("Login successful!");
                    Console.WriteLine("Welcome " + user.Name);


                    break;

                case 3:

                    checkLogin();

                    Console.WriteLine("Enter user ID:");
                    int id = int.Parse(Console.ReadLine());

                    var userById = Db.Users.FirstOrDefault(u => u.UserId == id);

                    if (userById == null)
                    {
                        Console.WriteLine("User not found!");
                        break;
                    }

                    Console.WriteLine("=== USER DETAILS ===");
                    Console.WriteLine("ID: " + userById.UserId);
                    Console.WriteLine("Name: " + userById.Name);
                    Console.WriteLine("Email: " + userById.Email);
                    Console.WriteLine("Phone: " + userById.Phone);
                    Console.WriteLine("Role: " + userById.Role);

                    break;


            }

        }

        public static void ProductMenu()
        {
            Console.WriteLine("choose an option");
            Console.WriteLine("1.add a new product ");
            Console.WriteLine("2.update a product details ");
            Console.WriteLine("3.get list of products by name/price");
            Console.WriteLine("4.get product details by ID ");
            int choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:

                    checkLogin();

                    Console.WriteLine("Enter product name:");
                    string name = Console.ReadLine();

                    Console.WriteLine("Enter price:");
                    decimal price = decimal.Parse(Console.ReadLine());

                    Console.WriteLine("Enter stock:");
                    int stock = int.Parse(Console.ReadLine());

                    var product = new Models.Product
                    {
                        Name = name,
                        Price = price,
                        Stock = stock
                    };


                    Db.Products.Add(product);
                    Db.SaveChanges();

                    Console.WriteLine("Product added successfully!");

                    break;

                case 2:

                    checkLogin();

                    Console.WriteLine("Enter product ID to update:");
                    int id = int.Parse(Console.ReadLine());

                    var Product = Db.Products.FirstOrDefault(p => p.ProductId == id);

                    if (Product == null)
                    {
                        Console.WriteLine("Product not found!");
                        return;
                    }

                    Console.WriteLine("Enter new name (leave empty to keep old):");
                    string UpdatedName = Console.ReadLine();

                    Console.WriteLine("Enter new price (or press Enter to skip):");
                    string Updatedprice = Console.ReadLine();

                    Console.WriteLine("Enter new stock (or press Enter to skip):");
                    string Updatedstock = Console.ReadLine();

                    if (!string.IsNullOrEmpty(UpdatedName))
                        Product.Name = UpdatedName;


                    if (!string.IsNullOrEmpty(Updatedprice))
                        Product.Price = decimal.Parse(Updatedprice);


                    if (!string.IsNullOrEmpty(Updatedstock))
                        Product.Stock = int.Parse(Updatedstock);

                    Db.SaveChanges();

                    Console.WriteLine("Product updated successfully!");


                    break;



                case 3:

                    checkLogin();

                    Console.WriteLine("=== SEARCH PRODUCTS ===");
                    Console.WriteLine("1. Search by Name");
                    Console.WriteLine("2. Search by Price Range");

                    int Choice = int.Parse(Console.ReadLine());


                    if (Choice == 1)
                    {
                        Console.WriteLine("Enter product name:");
                        string NAME = Console.ReadLine();

                        var products1 = Db.Products.Where(p => p.Name.Contains(NAME)).ToList();

                        if (products1.Count == 0)
                        {
                            Console.WriteLine("No products found!");
                            return;
                        }

                        foreach (var product1 in products1)
                        {
                            Console.WriteLine($"ID: {product1.ProductId}");
                            Console.WriteLine($"Name: {product1.Name}");
                            Console.WriteLine($"Price: {product1.Price}");
                            Console.WriteLine($"Stock: {product1.Stock}");
                            Console.WriteLine("-------------------");
                        }
                    }


                    else if (choice == 2)
                    {
                        Console.WriteLine("Enter minimum price:");
                        decimal minPrice = decimal.Parse(Console.ReadLine());

                        Console.WriteLine("Enter maximum price:");
                        decimal maxPrice = decimal.Parse(Console.ReadLine());

                        var products2 = Db.Products.Where(p => p.Price >= minPrice && p.Price <= maxPrice).ToList();

                        if (products2.Count == 0)
                        {
                            Console.WriteLine("No products found!");
                            return;
                        }

                        foreach (var PRODUCT in products2)
                        {
                            Console.WriteLine($"ID: {PRODUCT.ProductId}");
                            Console.WriteLine($"Name: {PRODUCT.Name}");
                            Console.WriteLine($"Price: {PRODUCT.Price}");
                            Console.WriteLine($"Stock: {PRODUCT.Stock}");
                            Console.WriteLine("-------------------");
                        }
                    }

                    else
                    {
                        Console.WriteLine("Invalid choice!");
                    }


                    break;



                case 4:

                    checkLogin();

                    Console.WriteLine("Enter Product ID:");
                    int productid = int.Parse(Console.ReadLine());

                    var products = Db.Products.FirstOrDefault(p => p.ProductId == productid);

                    if (products == null)
                    {
                        Console.WriteLine("Product not found!");
                        return;
                    }

                    Console.WriteLine("\n=== PRODUCT DETAILS ===");
                    Console.WriteLine($"ID: {products.ProductId}");
                    Console.WriteLine($"Name: {products.Name}");
                    Console.WriteLine($"Price: {products.Price}");
                    Console.WriteLine($"Stock: {products.Stock}");


                    break;


             
            }

        }

        public static void OrderMenu()
        {
            Console.WriteLine("choose an option");
            Console.WriteLine("1.Place a new order");
            Console.WriteLine("2.Get all orders for a user");
            Console.WriteLine("3.Get order details by ID");
            int choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:

                    checkLogin();

                    Console.WriteLine("Enter User ID:");
                    int userId = int.Parse(Console.ReadLine());

                    Console.WriteLine("Enter Product ID:");
                    int productId = int.Parse(Console.ReadLine());

                    Console.WriteLine("Enter Quantity:");
                    int quantity = int.Parse(Console.ReadLine());


                    var product = Db.Products.FirstOrDefault(p => p.ProductId == productId);

                    if (product == null)
                    {
                        Console.WriteLine("Product not found!");
                        return;
                    }


                    if (product.Stock < quantity)
                    {
                        Console.WriteLine("Not enough stock!");
                        return;
                    }


                    decimal totalAmount = product.Price * quantity;


                    var order = new Models.Order
                    {
                        UserId = userId,
                        OrderDate = DateTime.Now
                    };

                    Console.WriteLine(order.TotalAmount);

                    Db.Orders.Add(order);
                    Db.SaveChanges();


                    var orderProduct = new Models.OrderProducts
                    {
                        OrderId = order.OrderId,
                        ProductId = product.ProductId,
                        Quantity = quantity
                    };

                    Db.OrderProducts.Add(orderProduct);


                    product.Stock -= quantity;

                    Db.SaveChanges();

                    Console.WriteLine("Order placed successfully!");
                    Console.WriteLine($"Total Amount: {totalAmount}");




                    break;


                case 2:

                    checkLogin();

                    Console.WriteLine("Enter User ID:");
                    int USERId = int.Parse(Console.ReadLine());

                    var orders = Db.Orders.Where(o => o.UserId == USERId).ToList();

                    if (orders.Count == 0)
                    {
                        Console.WriteLine("No orders found!");
                        return;
                    }

                    Console.WriteLine("=== USER ORDERS ===");

                    foreach (var ORDER in orders)
                    {
                        Console.WriteLine($"Order ID: {ORDER.OrderId}");
                        Console.WriteLine($"Order Date: {ORDER.OrderDate}");
                        Console.WriteLine($"Total Amount: {ORDER.TotalAmount}");
                        Console.WriteLine("-------------------");
                    }

                    break;



                case 3:

                    checkLogin();

   
                    Console.WriteLine("Enter Order ID:");
                    int orderId = int.Parse(Console.ReadLine());

                    // Find order
                    var orderDetails = Db.Orders.FirstOrDefault(o => o.OrderId == orderId & o.UserId == LoggedInUser.UserId);

                    if (orderDetails == null)
                    {
                        Console.WriteLine("Order not found!");
                        break;
                    }

                    Console.WriteLine("=== ORDER DETAILS ===");
                    Console.WriteLine($"Order ID: {orderDetails.OrderId}");
                    Console.WriteLine($"Order Date: {orderDetails.OrderDate}");
                    Console.WriteLine($"Total Amount: {orderDetails.TotalAmount}");

                    // Get products in order
                    var orderProducts = Db.OrderProducts.Where(op => op.OrderId == orderDetails.OrderId).ToList();

                    Console.WriteLine("=== PRODUCTS ===");

                    foreach (var op in orderProducts)
                    {
                        var productDetails = Db.Products.FirstOrDefault(p => p.ProductId == op.ProductId);

                        if (productDetails != null)
                        {
                            Console.WriteLine($"Product Name: {productDetails.Name}");
                            Console.WriteLine($"Price: {productDetails.Price}");
                            Console.WriteLine($"Quantity: {op.Quantity}");
                            Console.WriteLine("-------------------");
                        }
                    }

                    break;

            }

        }


        public static void ReviewMenu()
        {
            Console.WriteLine("choose an option");
            Console.WriteLine("1.Add a review for a product");
            Console.WriteLine("2.Get all reviews for a product with pagination");
            Console.WriteLine("3.Update or delete a review ");
            int choice = int.Parse(Console.ReadLine());
            switch (choice)
            {

                case 1:

                    checkLogin();

                    Console.WriteLine("Enter Product ID:");
                    int productId = int.Parse(Console.ReadLine());

                    
                    var product = Db.Products
                        .FirstOrDefault(p => p.ProductId == productId);

                    if (product == null)
                    {
                        Console.WriteLine("Product not found!");
                        break;
                    }

                   
                    bool hasPurchased = Db.OrderProducts.Any(op =>op.ProductId == productId &&Db.Orders.Any(o =>o.OrderId == op.OrderId &&o.UserId == LoggedInUser.UserId));

                    if (!hasPurchased)
                    {
                        Console.WriteLine("You cannot review a product you didn't buy!");
                        break;
                    }

                    Console.WriteLine("Enter rating (1-5):");
                    int rating = int.Parse(Console.ReadLine());

                    Console.WriteLine("Enter comment:");
                    string comment = Console.ReadLine();

                   
                    var review = new Models.Review
                    {
                        UserId = LoggedInUser.UserId,
                        ProductId = productId,
                        Rating = rating,
                        Comment = comment
                    };

                    Db.Reviews.Add(review);
                    Db.SaveChanges();

                    Console.WriteLine("Review added successfully");


                    break;





                case 2:

                    checkLogin();

                    Console.WriteLine("Enter Product ID:");
                    int PRODUCTID = int.Parse(Console.ReadLine());

                    
                    var products = Db.Products
                        .FirstOrDefault(p => p.ProductId == PRODUCTID);

                    if (products == null)
                    {
                        Console.WriteLine("Product not found!");
                        break;
                    }

                    Console.WriteLine("Enter page number:");
                    int page = int.Parse(Console.ReadLine());

                    Console.WriteLine("Enter page size:");
                    int pageSize = int.Parse(Console.ReadLine());

                    // Pagination
                    var reviews = Db.Reviews
                        .Where(r => r.ProductId == PRODUCTID)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    if (reviews.Count == 0)
                    {
                        Console.WriteLine("No reviews found!");
                        break;
                    }

                    Console.WriteLine("\n=== PRODUCT REVIEWS ===");

                    foreach (var reviews1 in reviews)
                    {
                        var user = Db.Users.FirstOrDefault(u => u.UserId == reviews1.UserId);

                        Console.WriteLine($"Review ID: {reviews1.ReviewId}");
                        Console.WriteLine($"User: {user.Name}");
                        Console.WriteLine($"Rating: {reviews1.Rating}");
                        Console.WriteLine($"Comment: {reviews1.Comment}");
                        Console.WriteLine("-------------------");
                    }


                    break;




                case 3:

                    checkLogin();
                    Console.WriteLine("1. Update Review");
                    Console.WriteLine("2. Delete Review");

                    int reviewChoice = int.Parse(Console.ReadLine());

                    Console.WriteLine("Enter Review ID:");
                    int reviewId = int.Parse(Console.ReadLine());

                    // 🔍 Find review created by logged-in user
                    var review2 = Db.Reviews
                        .FirstOrDefault(r => r.ReviewId == reviewId &&
                                             r.UserId == LoggedInUser.UserId);

                    if (review2 == null)
                    {
                        Console.WriteLine("Review not found or you are not allowed!");
                        break;
                    }

                   
                    if (reviewChoice == 1)
                    {
                        Console.WriteLine("Enter new rating:");
                        review2.Rating = int.Parse(Console.ReadLine());

                        Console.WriteLine("Enter new comment:");
                        review2.Comment = Console.ReadLine();

                        Db.SaveChanges();

                        Console.WriteLine("Review updated successfully!");
                    }

                    
                    else if (reviewChoice == 2)
                    {
                        Db.Reviews.Remove(review2);
                        Db.SaveChanges();

                        Console.WriteLine("Review deleted successfully!");
                    }

                    else
                    {
                        Console.WriteLine("Invalid choice!");
                    }
                    break;

            }


            }


        static void Main(string[] args)
            {

            Db.Database.EnsureCreated();
            bool exit = false;

                while (exit==false)

                {

                    Console.WriteLine("Welcome to the E-Commerce System!");
                    Console.WriteLine("Please select an option:");
                    Console.WriteLine("1. User APIs");
                    Console.WriteLine("2. Product APIs");
                    Console.WriteLine("3. ORDER APIs");
                    Console.WriteLine("4. REVIEW APIs");
                    Console.WriteLine("5. Exit");

                    int choice = int.Parse(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:

                            UserMenu();

                            break;


                        case 2:

                            ProductMenu();

                            break;



                        case 3:

                            OrderMenu();

                            break;

                        case 4:

                           ReviewMenu();

                            break;

                        case 5:

                        Console.WriteLine("Exiting the system ");
                        Console.WriteLine("Thank you for using the system....");
                        exit = true;

                            break;



                        default:
                         
                        Console.WriteLine("Invalide option ");
                            
                        break;
                    }


                }











            }



        }
    }

