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


        // Constructor

        public DataStorageNonPersistent()
        {
            IsConnected = false;
            CompanyDict = new Dictionary<String, UInt64>();
            NextCompanyID = 0;
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
    
    }
}
