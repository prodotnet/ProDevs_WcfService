using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService" in both code and config file together.
[ServiceContract]
public interface IService
{


	/// <summary>
	///  Login and registeration 
	/// </summary>
	/// <param name="firstName"></param>
	/// <param name="lastName"></param>
	/// <param name="email"></param>
	/// <param name="password"></param>
	/// <param name="gender"></param>
	/// <param name="userType"></param>
	/// <param name="createDate"></param>
	/// <returns></returns>
	[OperationContract]
	bool Register(string firstName, string lastName, string email, string password, string gender ,string userType, DateTime createDate);

	[OperationContract]
	UserRegistration Login(string email, string password);


	/// <summary>
	/// 
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>

	[OperationContract]
	Product GetProduct(int id);

	[OperationContract]
	List<Product> GetAllProducts();

	[OperationContract]
	List<Product> GetBestSellingProducts();
	/// <summary>
	///  The wcf function for Sort and filter
	/// </summary>
	/// <param name="category"></param>
	/// <returns></returns>

	[OperationContract]
	List<Product> GetProductsByCategory(string category);
	[OperationContract]
	List<Product> GetProductsByPriceRange(decimal minPrice, decimal maxPrice);



	/// <summary>
	///  Product Management
	/// </summary>
	/// <param name="product"></param>
	/// <returns></returns>

	[OperationContract]
	bool AddProduct(Product product);

	[OperationContract]
	bool UpdateProduct(Product product);

	[OperationContract]
	bool DeleteProduct(int id);



	/// <summary>
	/// 
	/// </summary>
	/// <param name="userId"></param>
	/// <param name="productId"></param>
	/// <param name="quantity"></param>
	/// <returns></returns>

	[OperationContract]
	bool AddToCart(int userId, int productId, int quantity);

	[OperationContract]
	int GetCartItemCount(int userId);
	[OperationContract]
	bool RemoveFromCart(int userId, int productId);

	[OperationContract]
	bool UpdateCart(int userId, int productId, int quantity);

	[OperationContract]
	List<CartItem> GetCartItems(int userId);


	/// <summary>
	/// wcf function for Invoice
	/// </summary>
	/// <param name="userId"></param>
	/// <returns></returns>
	[OperationContract]
	Invoice Checkout(int userId);

	[OperationContract]
	Invoice GetInvoiceDetails(int userid);
	[OperationContract]
	List<InvoiceItem> GetInvoiceItems(int invoiceId);

	/// <summary>
	///ADiscount
	///
	/// </summary>
	/// <param name="totalAmount"></param>
	/// <returns></returns>
	[OperationContract]
	decimal ApplyDiscount(decimal totalAmount);

	/// <summary>
	/// //The wcf function fo dashboard queries
	/// </summary>
	/// <param ></param>
	/// <returns></returns>
	[OperationContract]
	int GetRegisteredUsersCountByDate(DateTime date);

	[OperationContract]
	int GetTotalProductsSold();


	[OperationContract]
	int GetTotalOrdersPlaced();


	[OperationContract]
	int GetProductsInSockCount();


}


