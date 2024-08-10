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

	[OperationContract]
	bool Register(string firstName, string lastName, string email, string password, string gender ,string userType, DateTime createDate);

	[OperationContract]
	
	int Login(string email, string password);


	[OperationContract]
	bool AddProduct(Product product);


	[OperationContract]
	bool UpdateProduct(Product product);

	[OperationContract]
	Product GetProduct(int id);


	[OperationContract]
	List<Product> GetAllProducts();


	[OperationContract]
	List<Product> GetProductsByCategory(string category);


	[OperationContract]
	bool DeleteProduct(int id);

}


