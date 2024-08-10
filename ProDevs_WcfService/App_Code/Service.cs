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

    public int Login(string email, string password)
    {

        var login = (from s in data.UserRegistrations where s.Email.Equals(email) && s.Password.Equals(password) select s).FirstOrDefault();

        if (login != null)
        {
            return login.Id;

        }
        else
        {

            return 0;
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
        var product = (from p in data.Products where p.Id == id select p).FirstOrDefault();

        if (product != null)
        {
            var tempProd = product;

            return tempProd;

        }
        else

            return null;
    }



    //Method  to get all  Products
    public List<Product> GetAllProducts()
    {
       

        dynamic getProdcts = (from p in data.Products where p.Active == 1 select p).DefaultIfEmpty();


        if (getProdcts != null)
        {

            List<Product> Products = new List<Product>();
           
            foreach (Product p in getProdcts)
            {


                Products.Add(p);
            }

            return Products;
        }
        else
        {
            return null;
        }


    }

  

    //method to sort by catagory
    public List<Product> GetProductsByCategory(string category)
    {
        var products = data.Products.Where(p => p.Active == 1);

        switch (category.ToLower())
        {
            case "smart watches":
                products = products.Where(p => p.Category == "Smart Watches");
                break;
            case "rolex":
                products = products.Where(p => p.Category == "Rolex");
                break;
            case "omega":
                products = products.Where(p => p.Category == "Omega");
                break;
            default:
                break;
        }

        return products.ToList();
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
                Quantity = quantity
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
        return data.CartItems.Where(c => c.UserId == userId).ToList();
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
