using System;
using System.Configuration;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Collections.Specialized;
using Dapper;
using System.Collections;
using System.Collections.Generic;


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

        public long AddCompany(Company company)
        {
            // This should have a check if the company name already exists and, in this case, only return its ID.
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = string.Format("INSERT INTO {0}company(name) VALUES(@name)", TablePrefix);
            cmd.Parameters.AddWithValue("@name", company.Name);
            cmd.ExecuteNonQuery();
            return cmd.LastInsertedId;
        }

        public Company GetCompanyByID(long companyID)
        {
            Company company = connection.Query<Company>(string.Format("SELECT * FROM {0}company WHERE id = @id", TablePrefix), new { id = companyID }).FirstOrDefault();
            return company;
        }

        public Company GetCompanyByName(string companyName)
        {
            Company company = connection.Query<Company>(string.Format("SELECT * FROM {0}company WHERE name = @name", TablePrefix), new {name = companyName}).FirstOrDefault();
            return company;
        }

        public long AddBrand(Brand brand)
        {
            throw new NotImplementedException();
        }

        public Brand GetBrandByID(long brandID)
        {
            throw new NotImplementedException();
        }

        public Brand GetBrandByName(string brandName)
        {
            // The example for "Retrieve object with referenced object" does not work...
            //    http://liangwu.wordpress.com/2012/08/16/dapper-net-samples/

            // But this does....

            String query = string.Format(
                 @"SELECT {0}brand.id AS bid, {0}brand.name AS bname, {0}company.id AS cid, {0}company.name AS cname
                   FROM   {0}brand JOIN {0}company ON {0}brand.companyID = {0}company.id
                    AND   {0}brand.name = @name", TablePrefix);
            var data = (IDictionary<string, object>) connection.Query(query, new { name = brandName }).FirstOrDefault();

            Company company = new Company((long) data["cid"], (string) data["cname"]);
            Brand brand = new Brand((long)data["bid"], (string)data["bname"], company);
            
            return brand;
        }
    }
}
