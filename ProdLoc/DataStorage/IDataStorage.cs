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
        UInt64 AddCompany(Company company);

        /// <summary>
        /// Returns the company with the given ID, if it exists. Returns null if it doesn't exist.
        /// </summary>
        /// <param name="companyID">The Company's ID to search for.</param>
        /// <returns>The Company with the given ID or null.</returns>
        Company GetCompanyByID(UInt64 companyID);

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
        UInt64 AddBrand(Brand brand);

        /// <summary>
        /// Returns the brand with the given ID, if it exists. Returns null if it doesn't exist.
        /// </summary>
        /// <param name="brandID">The Brands ID to search for.</param>
        /// <returns>The Brand with the given ID or null</returns>
        Brand GetBrandByID(UInt64 brandID);

        /// <summary>
        /// Returns the brand with the given name, if it exists. Returns null if it doesn't exist.
        /// </summary>
        /// <param name="brandName">The Brands name to search for.</param>
        /// <returns>The Brand with the given name or null.</returns>
        Brand GetBrandByName(String brandName);

    }
}
