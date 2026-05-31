using E_CommerceSystem.Models;
using System.Security.Cryptography;
using System.Text;

namespace E_CommerceSystem
{


    internal class Program
    {
        static ApplicationDbContext Db = new ApplicationDbContext();

        static User LoggedInUser;

        //static int currentuserId=0;

        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(
                    Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();

                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }




      
        public static void Getuser()
        {
           

            Console.WriteLine("Enter user ID:");

            string input = Console.ReadLine();

            if (!int.TryParse(input, out int id))
            {
                Console.WriteLine("Invalid ID. Please enter a numeric value.");
                return;
            }

            var userById = Db.Users.FirstOrDefault(u => u.UserId == id);

            if (userById == null)
            {
                Console.WriteLine("User not found!");
                return;
            }

            Console.WriteLine("=== USER DETAILS ===");
            Console.WriteLine("ID: " + userById.UserId);
            Console.WriteLine("Name: " + userById.Name);
            Console.WriteLine("Email: " + userById.Email);
            Console.WriteLine("Phone: " + userById.Phone);
            Console.WriteLine("Role: " + userById.Role);
        }


        //product
        public static void AddProduct()
        {
            

            Console.WriteLine("Enter product name:");
            string name = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Product name cannot be empty!");
                return;
            }

            Console.WriteLine("Enter price:");
            string priceInput = Console.ReadLine();

            if (!decimal.TryParse(priceInput, out decimal price))
            {
                Console.WriteLine("Invalid price format!");
                return;
            }

            if (price <= 0)
            {
                Console.WriteLine("Product price must be greater than zero!");
                return;
            }

            Console.WriteLine("enter a product description:");
            String ProductDes= Console.ReadLine();

            Console.WriteLine("Enter stock:");
            string stockInput = Console.ReadLine();

            if (!int.TryParse(stockInput, out int stock))
            {
                Console.WriteLine("Invalid stock format!");
                return;
            }

            if (stock < 0)
            {
                Console.WriteLine("Stock cannot be negative!");
                return;
            }

            var product = new Product
            {
                Name = name,
                Price = price,
                Description= ProductDes,
                Stock = stock
            };

            Db.Products.Add(product);
            Db.SaveChanges();

            Console.WriteLine("Product added successfully!");
        }

        public static void UpdateProduct()

        {

           

            Console.WriteLine("Enter product ID to update:");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid product ID!");
                return;
            }

            var product = Db.Products.FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                Console.WriteLine("Product not found!");
                return;
            }

            Console.WriteLine("Enter new name (leave empty to keep old):");
            string updatedName = Console.ReadLine()?.Trim();

            Console.WriteLine("Enter new price (or press Enter to skip):");
            string updatedPrice = Console.ReadLine();

            Console.WriteLine("Enter new description (or press Enter to skip):");
            string updatedDes= Console.ReadLine();

            Console.WriteLine("Enter new stock (or press Enter to skip):");
            string updatedStock = Console.ReadLine();

            

            // Update name
            if (!string.IsNullOrWhiteSpace(updatedName))
            {
                product.Name = updatedName;
            }

            // Update price safely
            if (!string.IsNullOrWhiteSpace(updatedPrice))
            {
                if (decimal.TryParse(updatedPrice, out decimal price))
                {
                    if (price > 0)
                        product.Price = price;
                    else
                        Console.WriteLine("Price must be greater than 0. Skipped.");
                }
                else
                {
                    Console.WriteLine("Invalid price format. Skipped.");
                }
            }

            // Update description safely

            if (!string.IsNullOrWhiteSpace(updatedDes))
            {
                product.Description = updatedDes.Trim();
            }


            // Update stock safely
            if (!string.IsNullOrWhiteSpace(updatedStock))
            {
                if (int.TryParse(updatedStock, out int stock))
                {
                    if (stock >= 0)
                        product.Stock = stock;
                    else
                        Console.WriteLine("Stock cannot be negative. Skipped.");
                }
                else
                {
                    Console.WriteLine("Invalid stock format. Skipped.");
                }
            }



            Db.SaveChanges();

            Console.WriteLine("Product updated successfully!");


        }


        public static void getListOfProduct()
        {
            Console.WriteLine("=== SEARCH PRODUCTS ===");
            Console.WriteLine("1. Search by Name");
            Console.WriteLine("2. Search by Price Range");

            int choice = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter page number:");
            int page = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter page size:");
            int pageSize = int.Parse(Console.ReadLine());

            if (choice == 1)
            {
                Console.WriteLine("Enter product name:");
                string name = Console.ReadLine();

                var products = Db.Products
                    .Where(p => p.Name.Contains(name))
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                if (products.Count == 0)
                {
                    Console.WriteLine("No products found!");
                    return;
                }

                foreach (var product in products)
                {
                    Console.WriteLine($"ID: {product.ProductId}");
                    Console.WriteLine($"Name: {product.Name}");
                    Console.WriteLine($"Price: {product.Price}");
                    Console.WriteLine($"Stock: {product.Stock}");
                    Console.WriteLine("-------------------");
                }
            }
            else if (choice == 2)
            {
                Console.WriteLine("Enter minimum price:");
                decimal minPrice = decimal.Parse(Console.ReadLine());

                Console.WriteLine("Enter maximum price:");
                decimal maxPrice = decimal.Parse(Console.ReadLine());

                var products = Db.Products
                    .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                if (products.Count == 0)
                {
                    Console.WriteLine("No products found!");
                    return;
                }

                foreach (var product in products)
                {
                    Console.WriteLine($"ID: {product.ProductId}");
                    Console.WriteLine($"Name: {product.Name}");
                    Console.WriteLine($"Price: {product.Price}");
                    Console.WriteLine($"Stock: {product.Stock}");
                    Console.WriteLine("-------------------");
                }
            }

            else
            {
                Console.WriteLine("Invalid choice!");
            }
        }


        public static void GetProductDetailsByID()
        {
            

            Console.WriteLine("Enter Product ID:");

            if (!int.TryParse(Console.ReadLine(), out int productId))
            {
                Console.WriteLine("Invalid Product ID!");
                return;
            }

            var product = Db.Products.FirstOrDefault(p => p.ProductId == productId);

            if (product == null)
            {
                Console.WriteLine("Product not found!");
                return;
            }

            Console.WriteLine("\n=== PRODUCT DETAILS ===");
            Console.WriteLine($"ID: {product.ProductId}");
            Console.WriteLine($"Name: {product.Name}");
            Console.WriteLine($"Price: {product.Price}");
            Console.WriteLine($"Stock: {product.Stock}");
        }




        //Order

        public static void PlaceAnewOrder()
        {
           

            if (LoggedInUser == null)
            {
                Console.WriteLine("User session expired!");
                return;
            }

            Console.WriteLine("Enter Product ID:");

            if (!int.TryParse(Console.ReadLine(), out int productId))
            {
                Console.WriteLine("Invalid Product ID!");
                return;
            }

            Console.WriteLine("Enter Quantity:");

            if (!int.TryParse(Console.ReadLine(), out int quantity))
            {
                Console.WriteLine("Invalid quantity!");
                return;
            }

            if (quantity <= 0)
            {
                Console.WriteLine("Quantity must be greater than zero!");
                return;
            }

            var product = Db.Products.FirstOrDefault(p => p.ProductId == productId);

            if (product == null)
            {
                Console.WriteLine("Product not found!");
                return;
            }

            if (quantity > product.Stock)
            {
                Console.WriteLine("Order cannot be placed");
                Console.WriteLine("Insufficient stock available");
                return;
            }

            decimal totalAmount = product.Price * quantity;


            product.Stock -= quantity;

            var order = new Order
            {
                UserId = LoggedInUser.UserId,
                OrderDate = DateTime.Now,
                TotalAmount = totalAmount
            };

            Db.Orders.Add(order);

            Db.SaveChanges(); // generates OrderId first

            var orderProduct = new OrderProducts
            {
                OrderId = order.OrderId,
                ProductId = product.ProductId,
                Quantity = quantity
            };

            Db.OrderProducts.Add(orderProduct);

            Db.SaveChanges();

            Console.WriteLine($"Old Stock: {product.Stock + quantity}");
            Console.WriteLine($"New Stock: {product.Stock}");

            Console.WriteLine("Order placed successfully!");
            Console.WriteLine($"Total Amount: {totalAmount}");

        }

        public static void GetAllOrdersForAUser()
        {

            
            Console.WriteLine("Enter User ID:");

            if (!int.TryParse(Console.ReadLine(), out int userId))
            {
                Console.WriteLine("Invalid User ID!");
                return;
            }

            var orders = Db.Orders.Where(o => o.UserId == userId).OrderByDescending(o => o.OrderDate).ToList();

            if (!orders.Any())
            {
                Console.WriteLine("No orders found!");
                return;
            }

            Console.WriteLine("=== USER ORDERS ===");

            foreach (var order in orders)
            {
                Console.WriteLine($"Order ID: {order.OrderId}");
                Console.WriteLine($"Order Date: {order.OrderDate}");
                Console.WriteLine($"Total Amount: {order.TotalAmount}");
                Console.WriteLine("-------------------");
            }



        }


        public static void GetOrderDetailsByID()
        {
            

            if (LoggedInUser == null)
            {
                Console.WriteLine("User session expired!");
                return;
            }

            Console.WriteLine("Enter Order ID:");

            if (!int.TryParse(Console.ReadLine(), out int orderId))
            {
                Console.WriteLine("Invalid Order ID!");
                return;
            }

            var orderDetails = Db.Orders
                .FirstOrDefault(o =>
                    o.OrderId == orderId &&
                    o.UserId == LoggedInUser.UserId);

            if (orderDetails == null)
            {
                Console.WriteLine("Order not found!");
                return;
            }

            Console.WriteLine("=== ORDER DETAILS ===");
            Console.WriteLine($"Order ID: {orderDetails.OrderId}");
            Console.WriteLine($"Order Date: {orderDetails.OrderDate}");
            Console.WriteLine($"Total Amount: {orderDetails.TotalAmount}");

            
            var orderProducts = Db.OrderProducts
                .Where(op => op.OrderId == orderDetails.OrderId)
                .ToList();

            Console.WriteLine("=== PRODUCT DETAILS ===");

            foreach (var op in orderProducts)
            {
                var productDetails = Db.Products
                    .FirstOrDefault(p => p.ProductId == op.ProductId);

                if (productDetails != null)
                {
                    Console.WriteLine($"Product Name: {productDetails.Name}");
                    Console.WriteLine($"Price: {productDetails.Price}");
                    Console.WriteLine($"Quantity: {op.Quantity}");
                    Console.WriteLine("-------------------");
                }
            }


        }


        //Review

        public static void AddAReviewForAProduct()
        {
            
            if (LoggedInUser == null)
            {
                Console.WriteLine("User session expired!");
                return;
            }

            Console.WriteLine("Enter Product ID:");

            if (!int.TryParse(Console.ReadLine(), out int productId))
            {
                Console.WriteLine("Invalid Product ID!");
                return;
            }

            var product = Db.Products.FirstOrDefault(p => p.ProductId == productId);

            if (product == null)
            {
                Console.WriteLine("Product not found!");
                return;
            }

            // Safer purchase check (step-by-step instead of nested query)
            var userOrders = Db.Orders
                .Where(o => o.UserId == LoggedInUser.UserId)
                .Select(o => o.OrderId)
                .ToList();

            bool hasPurchased = Db.OrderProducts
                .Any(op => op.ProductId == productId && userOrders.Contains(op.OrderId));

            if (!hasPurchased)
            {
                Console.WriteLine("You cannot review a product you didn't buy!");
                return;
            }

            bool alreadyReviewed = Db.Reviews
                .Any(r => r.UserId == LoggedInUser.UserId && r.ProductId == productId);

            if (alreadyReviewed)
            {
                Console.WriteLine("You already reviewed this product!");
                return;
            }

            Console.WriteLine("Enter rating (1-5):");

            if (!int.TryParse(Console.ReadLine(), out int rating))
            {
                Console.WriteLine("Invalid rating!");
                return;
            }

            if (rating < 1 || rating > 5)
            {
                Console.WriteLine("Rating must be between 1 and 5");
                return;
            }

            Console.WriteLine("Enter comment:");
            string comment = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(comment))
            {
                Console.WriteLine("Comment cannot be empty!");
                return;
            }

            var review = new Review
            {
                UserId = LoggedInUser.UserId,
                ProductId = productId,
                Rating = rating,
                Comment = comment
            };

            Db.Reviews.Add(review);
            Db.SaveChanges();

            Console.WriteLine("Review added successfully");

            // Optional: refresh product rating properly
            var updatedRating = Db.Reviews
                .Where(r => r.ProductId == productId)
                .Average(r => r.Rating);

            Console.WriteLine($"New Product Rating: {updatedRating:F1}");


        }


        public static void GetAllReviewsForAProductWithPagination()
        {

            Console.WriteLine("Enter Product ID:");

            if (!int.TryParse(Console.ReadLine(), out int productId))
            {
                Console.WriteLine("Invalid Product ID!");
                return;
            }

            var product = Db.Products.FirstOrDefault(p => p.ProductId == productId);

            if (product == null)
            {
                Console.WriteLine("Product not found!");
                return;
            }

            Console.WriteLine("Enter page number:");

            if (!int.TryParse(Console.ReadLine(), out int page) || page <= 0)
            {
                Console.WriteLine("Invalid page number!");
                return;
            }

            Console.WriteLine("Enter page size:");

            if (!int.TryParse(Console.ReadLine(), out int pageSize) || pageSize <= 0)
            {
                Console.WriteLine("Invalid page size!");
                return;
            }

            // Pagination
            var reviews = Db.Reviews
                .Where(r => r.ProductId == productId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            if (!reviews.Any())
            {
                Console.WriteLine("No reviews found!");
                return;
            }

            Console.WriteLine("\n=== PRODUCT REVIEWS ===");

            foreach (var review in reviews)
            {
                var user = Db.Users.FirstOrDefault(u => u.UserId == review.UserId);

                Console.WriteLine($"Review ID: {review.ReviewId}");
                Console.WriteLine($"User: {user?.Name ?? "Unknown"}");
                Console.WriteLine($"Rating: {review.Rating}");
                Console.WriteLine($"Comment: {review.Comment}");
                Console.WriteLine("-------------------");
            }


        }

        public static void UpdateOrDeleteAReview()
        {

           
            if (LoggedInUser == null)
            {
                Console.WriteLine("User session expired!");
                return;
            }

            Console.WriteLine("1. Update Review");
            Console.WriteLine("2. Delete Review");

            if (!int.TryParse(Console.ReadLine(), out int reviewChoice))
            {
                Console.WriteLine("Invalid choice!");
                return;
            }

            Console.WriteLine("Enter Review ID:");

            if (!int.TryParse(Console.ReadLine(), out int reviewId))
            {
                Console.WriteLine("Invalid Review ID!");
                return;
            }

            var review = Db.Reviews.FirstOrDefault(r =>
                r.ReviewId == reviewId &&
                r.UserId == LoggedInUser.UserId);

            if (review == null)
            {
                Console.WriteLine("Review not found or you are not allowed!");
                return;
            }

            if (reviewChoice == 1)
            {
                Console.WriteLine("Enter new rating:");

                if (!int.TryParse(Console.ReadLine(), out int newRating))
                {
                    Console.WriteLine("Invalid rating!");
                    return;
                }

                if (newRating < 1 || newRating > 5)
                {
                    Console.WriteLine("Rating must be between 1 and 5!");
                    return;
                }

                Console.WriteLine("Enter new comment:");
                string newComment = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(newComment))
                {
                    Console.WriteLine("Comment cannot be empty!");
                    return;
                }

                review.Rating = newRating;
                review.Comment = newComment;

                Db.SaveChanges();

                Console.WriteLine("Review updated successfully!");
            }
            else if (reviewChoice == 2)
            {
                Db.Reviews.Remove(review);
                Db.SaveChanges();

                Console.WriteLine("Review deleted successfully!");
            }
            else
            {
                Console.WriteLine("Invalid choice!");
            }



        }



        //Logout

        public static void Logout()
        {
            if (LoggedInUser == null)
            {
                Console.WriteLine("You are already logged out.");
                return;
            }

            LoggedInUser = null;
            Console.WriteLine("Logged out successfully!");
        }


        static void Main(string[] args)
           
        {

            Db.Database.EnsureCreated();

            bool exit = false;

            while (!exit)
            {
                
                while (LoggedInUser == null)
                {
                    Console.WriteLine("Welcome to E-commerce System......");
                    Console.WriteLine("Choose an Option");
                    Console.WriteLine("1. Register");
                    Console.WriteLine("2. Login");
                    Console.WriteLine("3.Exit ");
                    Console.WriteLine("----------------------------------------------------");


                    var authChoice = int.Parse(Console.ReadLine());

                    switch (authChoice)
                    {
                       
                        case 1:

                            Console.WriteLine("Enter your name:");
                            string name = Console.ReadLine();

                            Console.WriteLine("Enter your email:");
                            string email = Console.ReadLine();

                            // Check email format
                            if (!email.Contains("@") || !email.Contains("."))
                            {
                                Console.WriteLine("Invalid email format!");
                                break;
                            }

                            var existingUser = Db.Users.FirstOrDefault(u => u.Email == email);

                            if (existingUser != null)
                            {
                                Console.WriteLine("This email is already registered!");
                                break;
                            }

                            Console.WriteLine("Enter your password:");
                            string password = Console.ReadLine();

                            Console.WriteLine("Enter phone number:");
                            string phone = Console.ReadLine();

                            Console.WriteLine("Created At: " + DateTime.Now);


                            var newUser = new User
                            {
                                Name = name,
                                Email = email,
                                Password = HashPassword(password),
                                Phone = phone,
                                Role = "User"

                            };

                            Db.Users.Add(newUser);
                            Db.SaveChanges();

                            Console.WriteLine("Registration successful!");
                            Console.WriteLine("Please login.");

                            break;

                      
                        case 2:

                            Console.WriteLine("Enter email:");
                            string loginEmail = Console.ReadLine();

                            Console.WriteLine("Enter password:");
                            string loginPassword = Console.ReadLine();

                            // Check email format
                            if (!loginEmail.Contains("@") || !loginEmail.Contains("."))
                            {
                                Console.WriteLine("Invalid email format!");
                                break;
                            }

                            var user = Db.Users.FirstOrDefault(u =>
                                u.Email == loginEmail &&
                                u.Password == HashPassword(loginPassword));

                            if (user == null)
                            {
                                Console.WriteLine("Invalid email or password!");
                                break;
                            }

                            LoggedInUser = user;

                            Console.WriteLine("Login successful!");
                            Console.WriteLine("Welcome " + LoggedInUser.Name);

                            break;

                        case 3:

                            Console.WriteLine("Exiting system...");
                            LoggedInUser = null;
                            exit = true;

                            return;

                           

                        default:

                            Console.WriteLine("Invalid option!");
                            break;
                    }


                    Console.WriteLine("Press any key...");
                    Console.ReadLine();
                    Console.Clear();

                }


                //main system
                while (LoggedInUser != null)
                {
                    Console.WriteLine("=== MAIN MENU ===");
                    Console.WriteLine("Choose an Option");
                    Console.WriteLine("1. Get user details by ID");
                    Console.WriteLine("2. Add a new product");
                    Console.WriteLine("3. Update product details");
                    Console.WriteLine("4. Get products with pagination and filtering");
                    Console.WriteLine("5. Get product details by ID");
                    Console.WriteLine("6. Place a new order");
                    Console.WriteLine("7. Get all orders for a user");
                    Console.WriteLine("8. Get order details by ID");
                    Console.WriteLine("9. Add a review for a product");
                    Console.WriteLine("10. Get all reviews for a product");
                    Console.WriteLine("11. Update or delete a review");
                    Console.WriteLine("12. Logout");
                    Console.WriteLine("13. Exit");
                    Console.WriteLine("----------------------------------------------------");

                    int choice = int.Parse(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:

                            Getuser();

                            break;

                        case 2:

                            AddProduct();

                            break;

                        case 3:

                            UpdateProduct();

                            break;

                        case 4:

                            getListOfProduct();

                            break;

                        case 5:

                            GetProductDetailsByID();
                            
                           
                            break;



                        case 6:

                            PlaceAnewOrder();

                            break;



                        case 7:

                            GetAllOrdersForAUser();
                            break;



                        case 8:

                            GetOrderDetailsByID();

                            break;



                        case 9:

                            AddAReviewForAProduct();

                            break;


                        case 10:

                            GetAllReviewsForAProductWithPagination();


                            break;


                        case 11:

                            UpdateOrDeleteAReview();


                            break;


                        case 12:

                            Logout();

                            break;

                        case 13:

                            Console.WriteLine("Exiting system...");
                            LoggedInUser = null;
                            exit = true;

                            return;




                        default:

                            Console.WriteLine("Invalid option!");
                            break;
                    }

                    Console.WriteLine("Press any key...");
                    Console.ReadLine();
                    Console.Clear();

                }
            }

            Console.WriteLine("Thank you for using the system!");


        }







        }



    }
   

