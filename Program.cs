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




        public static bool checkLogin()
        {
            if (LoggedInUser == null)
            {
                Console.WriteLine("Please login first!");
                return false;
            }

            return true;
        }



        //public static void UserMenu()
        //{
        //    Console.WriteLine("choose an option");
        //    Console.WriteLine("1.Get user details");

        //    int choice = int.Parse(Console.ReadLine());

        //    switch (choice)
        //    {
        //        case 1:

        //            if (!checkLogin())
        //            {
        //                break;
        //            }

        //            Console.WriteLine("Enter user ID:");
        //            int id = int.Parse(Console.ReadLine());

        //            var userById = Db.Users
        //                .FirstOrDefault(u => u.UserId == id);

        //            if (userById == null)
        //            {
        //                Console.WriteLine("User not found!");
        //                break;
        //            }

        //            Console.WriteLine("=== USER DETAILS ===");
        //            Console.WriteLine("ID: " + userById.UserId);
        //            Console.WriteLine("Name: " + userById.Name);
        //            Console.WriteLine("Email: " + userById.Email);
        //            Console.WriteLine("Phone: " + userById.Phone);
        //            Console.WriteLine("Role: " + userById.Role);

        //            break;

        //        default:

        //            Console.WriteLine("Invalid option!");

        //            break;
        //    }
        //}




        public static void Getuser()
        {
            if (!checkLogin())
            {
                return;
            }

            Console.WriteLine("Enter user ID:");
            int id = int.Parse(Console.ReadLine());

            var userById = Db.Users
                .FirstOrDefault(u => u.UserId == id);

            if (userById == null)
            {
                Console.WriteLine("User not found!");
                
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
            if (!checkLogin())
            {
                return;
            }

            Console.WriteLine("Enter product name:");
            string name = Console.ReadLine();

            Console.WriteLine("Enter price:");
            decimal price = decimal.Parse(Console.ReadLine());

            // Check price
            if (price <= 0)
            {
                Console.WriteLine("Product price must be greater than zero!");
                return;
            }

            Console.WriteLine("Enter stock:");
            int stock = int.Parse(Console.ReadLine());

            if (stock < 0)
            {
                Console.WriteLine("Stock cannot be negative!");
                return;
            }

            var product = new Product
            {
                Name = name,
                Price = price,
                Stock = stock
            };


            Db.Products.Add(product);
            Db.SaveChanges();

            Console.WriteLine("Product added successfully!");
        }

        public static void UpdateProduct()

        {

            if (!checkLogin())
            {
                return;
            }

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

        }


        public static void getListOfProduct()
        {
            if (!checkLogin())
            {
                return;
            }

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


            else if (Choice == 2)
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

        }


        public static void GetProductDetailsByID()
        {
            if (!checkLogin())
            {
                return;
            }

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

        }




        //Order

        public static void PlaceAnewOrder()
        {

            if (!checkLogin())
            {
                return;
            }

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


            // Quantity must be greater than zero

            if (quantity <= 0)
            {
                Console.WriteLine("Quantity must be greater than zero!");
                return;
            }


            // Check stock availability
            if (quantity > product.Stock)
            {
                Console.WriteLine("Order cannot be placed ");
                Console.WriteLine("Insufficient stock available ");

                return;
            }


            decimal totalAmount = product.Price * quantity;


            var order = new Order
            {
                UserId = LoggedInUser.UserId,
                OrderDate = DateTime.Now
            };

            Db.Orders.Add(order);
            Db.SaveChanges();


            var orderProduct = new OrderProducts
            {
                OrderId = order.OrderId,
                ProductId = product.ProductId,
                Quantity = quantity
            };

            Db.OrderProducts.Add(orderProduct);

            //  Reduce stock after successful order

            Console.WriteLine($"Old Stock: {product.Stock}");

            product.Stock -= quantity;

            Console.WriteLine($"New Stock: {product.Stock}");


            Db.SaveChanges();

            Console.WriteLine("Order placed successfully!");
            Console.WriteLine($"Total Amount: {totalAmount}");

        }

        public static void GetAllOrdersForAUser()
        {

            if (!checkLogin())
            {
                return;
            }

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



        }


        public static void GetOrderDetailsByID()
        {
            if (!checkLogin())
            {
                return;
            }


            Console.WriteLine("Enter Order ID:");
            int orderId = int.Parse(Console.ReadLine());

            // Find order
            var orderDetails = Db.Orders.FirstOrDefault(o => o.OrderId == orderId & o.UserId == LoggedInUser.UserId);

            if (orderDetails == null)
            {
                Console.WriteLine("Order not found!");
                
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


            }


        //Review

        public static void AddAReviewForAProduct()
        {
            if (!checkLogin())
            {
                return;
            }

            Console.WriteLine("Enter Product ID:");
            int productId = int.Parse(Console.ReadLine());


            var product = Db.Products
                .FirstOrDefault(p => p.ProductId == productId);

            if (product == null)
            {
                Console.WriteLine("Product not found!");
                return;
            }


            bool hasPurchased = Db.OrderProducts.Any(op => op.ProductId == productId && Db.Orders.Any(o => o.OrderId == op.OrderId && o.UserId == LoggedInUser.UserId));

            if (!hasPurchased)
            {
                Console.WriteLine("You cannot review a product you didn't buy!");
                return;

            }


            bool alreadyReviewed = Db.Reviews.Any(r =>r.UserId == LoggedInUser.UserId &&r.ProductId == productId);

            if (alreadyReviewed)
            {
                Console.WriteLine("You already reviewed this product!");
                return;
            }


            Console.WriteLine("Enter rating (1-5):");
            int rating = int.Parse(Console.ReadLine());

            if (rating < 1 || rating > 5)
            {
                Console.WriteLine("Rating must be between 1 and 5 ");
                return;
            }

            Console.WriteLine("Enter comment:");
            string comment = Console.ReadLine();


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
            
            Console.WriteLine($"New Product Rating: {product.OverallRating}");


        }


        public static void GetAllReviewsForAProductWithPagination()
        {
            if (!checkLogin())
            {
                return;
            }

            Console.WriteLine("Enter Product ID:");
            int PRODUCTID = int.Parse(Console.ReadLine());


            var products = Db.Products
                .FirstOrDefault(p => p.ProductId == PRODUCTID);

            if (products == null)
            {
                Console.WriteLine("Product not found!");
              //  break;
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
              //  break;
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




        }

        public static void UpdateOrDeleteAReview()
        {


            if (!checkLogin())
            {
                return;
            }
            
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
              //  break;
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



        }

        //Logout

        public static void Logout()
        {
            if (LoggedInUser == null)
            {
                Console.WriteLine("You are not logged in!");
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
                    Console.WriteLine("3. Exit");

                    int authChoice = int.Parse(Console.ReadLine());

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

                            Console.WriteLine("Enter role:");
                            string role = Console.ReadLine();

                            var newUser = new User
                            {
                                Name = name,
                                Email = email,
                                Password = HashPassword(password),
                                Phone = phone,
                                Role = role

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

                            exit = true;

                            break;

                        default:

                            Console.WriteLine("Invalid option!");
                            break;
                    }
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

                            break;


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
   

