using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in code, svc and config file together.
public class Service : IService
{
    DataClassesDataContext data = new DataClassesDataContext();

    public  UserRegistration Login(string email, string password)
    {

        var login = (from s in data.UserRegistrations where s.Email.Equals(email) && s.Password.Equals(password) select s).FirstOrDefault();

        if (login != null)
        {

            var User = new UserRegistration
            {
                Id = login.Id,
                FirstName = login.FirstName,
                LastName = login.LastName,
                Email = login.Email,
                UserType = login.UserType,
            
            };

            return User;

        }
        else
        {

            return  null;
        }
    }


    public bool Register(string firstName, string lastName, string email, string password,  string gender, string userType, DateTime createDate)
    {
        

        // Checking if the user already exists
        var IsUserExit = data.UserRegistrations.FirstOrDefault(x => x.Email == email);
        if (IsUserExit != null)
        {
            return false; 
        }

        // Create a new user registration
        var newUser = new UserRegistration
        {
           
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password,
            Gender = gender,
            UserType = userType,
            CreateDate = createDate
        };

        data.UserRegistrations.InsertOnSubmit(newUser);

        try
        {
            data.SubmitChanges(); 
            return true; 
        }
        catch (Exception)
        {
            return false; 
        }
    }


    //Method to Add Product
    public bool AddProduct(Product product)
    {
       
            
            var newProduct = new Product
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Category = product.Category,
                ImageUrl_ = product.ImageUrl_,
                Active = 1 
            };

          
            data.Products.InsertOnSubmit(newProduct);
         
           try
           {

              data.SubmitChanges();

               return true;
           }
           catch (Exception ex)
           {
              return false;
           }
    }



    public bool UpdateProduct(Product product)
    {

        // Checking if the user already exists
        var IsProductExit = data.Products.FirstOrDefault(p => p.Id == product.Id);


        try
        {
            
            if (IsProductExit != null)
            {
                IsProductExit.Name = product.Name;
                IsProductExit.Description = product.Description;
                IsProductExit.Price = product.Price;
                IsProductExit.Category = product.Category;
                IsProductExit.ImageUrl_ = product.ImageUrl_;

                data.SubmitChanges();
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
           
            return false;
        }
    }

    //Method  to get Product
    public Product GetProduct(int id)
    {
        var get = (from p in data.Products where p.Id.Equals(id) && p.Active.Equals(1) select p).FirstOrDefault();

        if (get != null)
        {
            var Prod = new Product
            {
                Id = get.Id,
                Name = get.Name,
                Description = get.Description,
                ImageUrl_ = get.ImageUrl_,
                Price = get.Price,


            };


            return Prod;
        }
        else

            return null;
    }



    //Method  to get all  Products dynamically 
    public List<Product> GetAllProducts()
    {
        dynamic Prods = new List<Product>();

        dynamic tempProds = (from p in data.Products
                             where p.Active == 1
                             select p).DefaultIfEmpty();

        if (tempProds != null)
        {
            foreach (Product p in tempProds)
            {
                var AllProds = new Product
                {
                    Id = p.Id,
                    Name = p.Name,                    
                    Description = p.Description,
                    ImageUrl_ = p.ImageUrl_,
                    Price = p.Price,                  
                    Category = p.Category,
                    Active = 1,

                };

                Prods.Add(AllProds);
            }

            return Prods;
        }
        else
        {
            return null;
        }


    }



    //method to sort by catagory by name
    public List<Product> GetProductsByCategory(string category)
    {
        dynamic Prods = new List<Product>();

      
        var tempProds = (from p in data.Products
                         where p.Active == 1
                         select p).DefaultIfEmpty();

    
        if (tempProds != null && tempProds.Any())
        {
            foreach (Product p in tempProds)
            {
                // Filter products based on category
                bool categoryMatches = false;

                switch (category.ToLower())
                {
                    case "smart watches":
                        categoryMatches = p.Category == "Smart Watches";
                        break;
                    case "rolex":
                        categoryMatches = p.Category == "Rolex";
                        break;
                    case "omega":
                        categoryMatches = p.Category == "Omega";
                        break;
                    default:
                        categoryMatches = true; 
                        break;
                }

                // Add to result list if category matches
                if (categoryMatches)
                {
                    var AllProds = new Product
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        ImageUrl_ = p.ImageUrl_,
                        Price = p.Price,
                        Category = p.Category,
                        Active = 1,
                    };

                    Prods.Add(AllProds);
                }
            }

            return Prods;
        }
        else
        {
            return new List<Product>(); // Return an empty list instead of null
        }
    }



    public List<Product> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
    {
        dynamic Prods = new List<Product>();

        // Get all active products
        var tempProds = (from p in data.Products
                         where p.Active == 1
                         select p).DefaultIfEmpty();

       
        if (tempProds != null && tempProds.Any())
        {
            foreach (Product p in tempProds)
            {
               
                if (p.Price >= minPrice && p.Price <= maxPrice)
                {
                    var AllProds = new Product
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        ImageUrl_ = p.ImageUrl_,
                        Price = p.Price,
                        Category = p.Category,
                        Active = 1,
                    };

                    Prods.Add(AllProds);
                }
            }

            return Prods;
        }
        else
        {
            return new List<Product>(); 
        }
    }



    //a function to delete products
    public bool DeleteProduct(int id)
    {
        try
        {
            var product = data.Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                data.Products.DeleteOnSubmit(product);
                data.SubmitChanges();
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
           
            return false;
        }
    }




    public bool AddToCart(int userId, int productId, int quantity)
    {
        var cartItem = data.CartItems.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);

        var get = (from p in data.Products where p.Id.Equals(productId)  select p).FirstOrDefault();

        if (cartItem != null)
        {
            cartItem.Quantity += quantity;
        }
        else
        {
            cartItem = new CartItem
            {
                UserId = userId,
                ProductId = productId,
                Quantity = quantity,
                Name = get.Name,
                Price = get.Price,
                ImageUrl = get.ImageUrl_

            };
            data.CartItems.InsertOnSubmit(cartItem);
        }

        try
        {
            data.SubmitChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }


    //a method to count cart items
    public int GetCartItemCount(int userId)
    {
        try
        {
            
            int itemCount = data.CartItems.Count(c => c.UserId == userId);
            return itemCount;
        }
        catch (Exception ex)
        {
          
            return 0; 
        }
    }



    public bool RemoveFromCart(int userId, int productId)
    {
        var cartItem = data.CartItems.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);

        if (cartItem != null)
        {
            data.CartItems.DeleteOnSubmit(cartItem);
            try
            {
                data.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        return false;
    }

    public bool UpdateCart(int userId, int productId, int quantity)
    {
        var cartItem = data.CartItems.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);

        if (cartItem != null)
        {
            cartItem.Quantity = quantity;

            try
            {
                data.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        return false;
    }



    public List<CartItem> GetCartItems(int userId)
    {
    
      

       dynamic Lists_CartItems = new List<CartItem>();

       var tempCartItem = (from p in data.CartItems
                             where p.UserId == userId
                             select p).DefaultIfEmpty();


        if (tempCartItem != null)
        {
            foreach (CartItem C in tempCartItem)
            {
                var Items = new CartItem
                {
                    Id = C.Id,
                    UserId = C.UserId,
                    ProductId = C.ProductId,
                    Quantity = C.Quantity,
                    Price = C.Price,
                    Name = C.Name,
                    ImageUrl = C.ImageUrl

                };

                Lists_CartItems.Add(Items);
            }

            return Lists_CartItems;
        }
        else
        {
            return null;
        }


    }


    public Invoice Checkout(int userId)
    {
        var cartItems = GetCartItems(userId);

        if (cartItems.Count > 0)
        {
            var invoice = new Invoice
            {
                UserId = userId,
                Date = DateTime.Now,
                TotalAmount = cartItems.Sum(c => c.Product.Price * c.Quantity)
            };

            data.Invoices.InsertOnSubmit(invoice);

            foreach (var item in cartItems)
            {
                var invoiceItem = new InvoiceItem
                {
                    Id = invoice.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Product.Price
                };

                data.InvoiceItems.InsertOnSubmit(invoiceItem);
                data.CartItems.DeleteOnSubmit(item);
            }

            try
            {
                data.SubmitChanges();
                return invoice;
            }
            catch
            {
                return null;
            }
        }
        return null;
    }
}
