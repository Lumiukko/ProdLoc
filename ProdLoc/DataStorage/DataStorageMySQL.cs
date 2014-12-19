using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Collections.Specialized;


namespace ProdLoc
{
    class DataStorageMySQL : IDataStorage
    {

        private String Server;
        private String Database;
        private String UserID;
        private String Password;
        private String TablePrefix;
        private String CurrentUser;

        private MySqlConnection connection;

        public DataStorageMySQL()
        {
            NameValueCollection config = ConfigurationManager.AppSettings;
            Server = config["mysql_server"];
            Database = config["mysql_db"];
            UserID = config["mysql_user"];
            Password = config["mysql_pass"];
            TablePrefix = config["mysql_prefix"];
            CurrentUser = config["current_user"];

            String connString = string.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};", Server, Database, UserID, Password);
            connection = new MySqlConnection(connString);


        }



        public bool Connect()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(string.Format("Error while connecting to MySQL server. Code: {0}", ex.Number));
                return false;
            }
        }

        public bool Disconnect()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(string.Format("Error while disconnecting from MySQL server. Code: {0}", ex.Number));
                return false;
            }
        }

        public ulong AddCompany(Company company)
        {
            // This should have a check if the company name already exists and, in this case, only return its ID.
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = string.Format("INSERT INTO {0}company(name) VALUES(@name)", TablePrefix);
            cmd.Parameters.AddWithValue("@name", company.Name);
            cmd.ExecuteNonQuery();
            return (ulong) cmd.LastInsertedId;
        }

        public Company GetCompanyByID(ulong companyID)
        {
            throw new NotImplementedException();
        }

        public Company GetCompanyByName(string companyName)
        {
            throw new NotImplementedException();
        }

        public ulong AddBrand(Brand brand)
        {
            throw new NotImplementedException();
        }

        public Brand GetBrandByID(ulong brandID)
        {
            throw new NotImplementedException();
        }

        public Brand GetBrandByName(string brandName)
        {
            throw new NotImplementedException();
        }
    }
}
