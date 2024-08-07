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
        

        // Check if the user already exists
        var IsUserExit = data.UserRegistrations.FirstOrDefault(x => x.Email == email);
        if (IsUserExit != null)
        {
            return false; // User already exists
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

    public Product GetProduct(int id)
    {
        var product = (from p in data.Products  where p.Id == id select p).FirstOrDefault();

        if (product != null)
        {
            var tempProd = product;

            return tempProd;

        }
        else
       
            return null;
    }

}
