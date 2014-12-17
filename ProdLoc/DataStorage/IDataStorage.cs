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
        /// Returns the company with the given name, if it exists. If it doesn't exist returns null.
        /// </summary>
        /// <param name="companyName">The Company's name to search for.</param>
        /// <returns>The Company with the given name or null.</returns>
        Company GetCompanyByName(String companyName);

    }
}
