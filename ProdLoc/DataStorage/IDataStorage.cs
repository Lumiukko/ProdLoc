using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdLoc
{
    public interface IDataStorage
    {
        /// <summary>
        /// Connects to the database.
        /// </summary>
        /// <returns>True if the connection succeeded, otherwise false.</returns>
        Boolean Connect();

        /// <summary>
        /// Disconnects from the database explicitly.
        /// </summary>
        /// <returns>True if disconnect succeeded, otherwise false.</returns>
        Boolean Disconnect();

        /// <summary>
        /// Adds a company if it does not exist yet.
        /// </summary>
        /// <param name="company">The Company to add.</param>
        /// <returns>ID of the newly created company, or the ID of the company if it already exists.</returns>
        Int64 AddCompany(Company company);

        /// <summary>
        /// Returns the company with the given ID, if it exists. Returns null if it doesn't exist.
        /// </summary>
        /// <param name="companyID">The Company's ID to search for.</param>
        /// <returns>The Company with the given ID or null.</returns>
        Company GetCompanyByID(Int64 companyID);

        /// <summary>
        /// Returns the company with the given name, if it exists. Returns null if it doesn't exist.
        /// </summary>
        /// <param name="companyName">The Company's name to search for.</param>
        /// <returns>The Company with the given name or null.</returns>
        Company GetCompanyByName(String companyName);

        /// <summary>
        /// Adds a brand and if it does not exist yet. Persists associated Company before adding the Brand.
        /// </summary>
        /// <param name="brand">The Brand to add.</param>
        /// <returns>The ID of the newly created brand, or the ID of the brand if it already exists.</returns>
        Int64 AddBrand(Brand brand);

        /// <summary>
        /// Returns the brand with the given ID, if it exists. Returns null if it doesn't exist.
        /// </summary>
        /// <param name="brandID">The Brands ID to search for.</param>
        /// <returns>The Brand with the given ID or null</returns>
        Brand GetBrandByID(Int64 brandID);

        /// <summary>
        /// Returns the brand with the given name, if it exists. Returns null if it doesn't exist.
        /// </summary>
        /// <param name="brandName">The Brands name to search for.</param>
        /// <returns>The Brand with the given name or null.</returns>
        Brand GetBrandByName(String brandName);

        /// <summary>
        /// Adds a product and if it does not exist yet. Persists associated Brand before adding the Product.
        /// </summary>
        /// <param name="product">The Product to add.</param>
        /// <returns>The ID of the newly created product, or the ID of the product if it already exists.</returns>
        Int64 AddProduct(Product product);

        /// <summary>
        /// Returns the product with the given ID, if it exists. Returns null if it doesn't exist.
        /// </summary>
        /// <param name="productID">The Products ID to search for.</param>
        /// <returns>The Product with the given ID or null</returns>
        Product GetProductByID(Int64 productID);

        /// <summary>
        /// Returns the product with the given barcode, if it exists. Returns null if it doesn't exist.
        /// </summary>
        /// <param name="barcode">The Products barcode to search for.</param>
        /// <returns>The Product with the given name or null.</returns>
        Product GetProductByBarcode(String barcode);

        /// <summary>
        /// Returns the offer with the given ID, if it exists. Returns null if it doesn't exist.
        /// </summary>
        /// <param name="offerID">The Offers ID to search for.</param>
        /// <returns>The Offer with the given ID or null</returns>
        Offer GetOfferByID(Int64 offerID);

    }
}
