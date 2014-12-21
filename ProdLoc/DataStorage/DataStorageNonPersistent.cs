using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdLoc
{
    public class DataStorageNonPersistent : IDataStorage
    {
        private Boolean IsConnected;
        private Dictionary<String, Int64> CompanyDict;
        private Int64 NextCompanyID;
        private Dictionary<String, Tuple<Int64, Int64>> BrandDict;
        private Int64 NextBrandID;


        // Constructor

        public DataStorageNonPersistent()
        {
            IsConnected = false;
            CompanyDict = new Dictionary<String, Int64>();
            NextCompanyID = 0;
            BrandDict = new Dictionary<String, Tuple<Int64, Int64>>();
            NextBrandID = 0;
        }


        // Connection Modifiers

        public bool Connect()
        {
            if (!IsConnected)
            {
                IsConnected = true;
                return true;
            }
            return false;
        }

        public bool Disconnect()
        {
            if (IsConnected)
            {
                IsConnected = false;
                return true;
            }
            return false;
        }


        // Company Accessors

        private Int64 GetNextCompanyID()
        {
            NextCompanyID++;
            return NextCompanyID;
        }

        public long AddCompany(Company company)
        {
            long newCompanyID;
            if (!IsConnected) throw new Exception("DataStorage not connected.");
            if (!CompanyDict.ContainsKey(company.Name))
            {
                newCompanyID = GetNextCompanyID();
                CompanyDict.Add(company.Name, newCompanyID);
            }
            else
            {
                newCompanyID = CompanyDict[company.Name];
            }
            return newCompanyID;
        }

        public Company GetCompanyByID(long companyID)
        {
            if (!IsConnected) throw new Exception("DataStorage not connected.");
            if (!CompanyDict.ContainsValue(companyID))
            {
                return null;
            }
            else
            {
                KeyValuePair<String, Int64> firstResult = CompanyDict.Where(entry => entry.Value.Equals(companyID)).First();
                return new Company(firstResult.Value, firstResult.Key);
            }
        }

        public Company GetCompanyByName(string companyName)
        {
            if (!IsConnected) throw new Exception("DataStorage not connected.");
            if (!CompanyDict.ContainsKey(companyName))
            {
                return null;
            }
            else
            {
                return new Company(CompanyDict[companyName], companyName);
            }
        }


        // Brand Accessors

        private long GetNextBrandID()
        {
            NextBrandID++;
            return NextBrandID;
        }

        public long AddBrand(Brand brand)
        {
            long newBrandID;
            if (!IsConnected) throw new Exception("DataStorage not connected.");
            // TODO: Perhaps add the possibility to commit/rollback for transactions.
            //       If the add brand fails, the add company is undone.
            long newCompanyID = AddCompany(brand.Company);
            if (!BrandDict.ContainsKey(brand.Name))
            {
                newBrandID = GetNextBrandID();
                BrandDict.Add(brand.Name, Tuple.Create(newBrandID, newCompanyID));
            }
            else
            {
                newBrandID = BrandDict[brand.Name].Item1;
            }
            return newBrandID;
        }

        public Brand GetBrandByID(long brandID)
        {
            if (!IsConnected) throw new Exception("DataStorage not connected.");
            if (!CompanyDict.ContainsValue(brandID))
            {
                return null;
            }
            else
            {
                KeyValuePair<String, Tuple<Int64, Int64>> firstResult = BrandDict.Where(entry => entry.Value.Equals(brandID)).First();
                Company associatedCompany = GetCompanyByID(firstResult.Value.Item2);
                return new Brand(firstResult.Value.Item1, firstResult.Key, associatedCompany);
            }
        }

        public Brand GetBrandByName(string brandName)
        {
            // TODO: Implement GetBrandByName
            throw new NotImplementedException();
        }


        public long AddProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public Product GetProductByID(long productID)
        {
            throw new NotImplementedException();
        }

        public Product GetProductByName(string productName)
        {
            throw new NotImplementedException();
        }
    }
}
