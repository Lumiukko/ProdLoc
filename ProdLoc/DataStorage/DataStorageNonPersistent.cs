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
        private Dictionary<String, UInt64> CompanyDict;
        private UInt64 NextCompanyID;
        private Dictionary<String, Tuple<UInt64, UInt64>> BrandDict;
        private UInt64 NextBrandID;


        // Constructor

        public DataStorageNonPersistent()
        {
            IsConnected = false;
            CompanyDict = new Dictionary<String, UInt64>();
            NextCompanyID = 0;
            BrandDict = new Dictionary<String, Tuple<UInt64, UInt64>>();
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

        private UInt64 GetNextCompanyID()
        {
            NextCompanyID++;
            return NextCompanyID;
        }

        public ulong AddCompany(Company company)
        {
            ulong newCompanyID;
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

        public Company GetCompanyByID(ulong companyID)
        {
            if (!IsConnected) throw new Exception("DataStorage not connected.");
            if (!CompanyDict.ContainsValue(companyID))
            {
                return null;
            }
            else
            {
                KeyValuePair<String, UInt64> firstResult = CompanyDict.Where(entry => entry.Value.Equals(companyID)).First();
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

        private ulong GetNextBrandID()
        {
            NextBrandID++;
            return NextBrandID;
        }

        public ulong AddBrand(Brand brand)
        {
            ulong newBrandID;
            if (!IsConnected) throw new Exception("DataStorage not connected.");
            // TODO: Perhaps add the possibility to commit/rollback for transactions.
            //       If the add brand fails, the add company is undone.
            ulong newCompanyID = AddCompany(brand.Company);
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

        public Brand GetBrandByID(ulong brandID)
        {
            if (!IsConnected) throw new Exception("DataStorage not connected.");
            if (!CompanyDict.ContainsValue(brandID))
            {
                return null;
            }
            else
            {
                KeyValuePair<String, Tuple<UInt64, UInt64>> firstResult = BrandDict.Where(entry => entry.Value.Equals(brandID)).First();
                Company associatedCompany = GetCompanyByID(firstResult.Value.Item2);
                return new Brand(firstResult.Value.Item1, firstResult.Key, associatedCompany);
            }
        }

        public Brand GetBrandByName(string brandName)
        {
            // TODO: Implement GetBrandByName
            throw new NotImplementedException();
        }
    }
}
