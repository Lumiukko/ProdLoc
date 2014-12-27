using System;
using System.Configuration;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Collections.Specialized;
using Dapper;
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
        private MySqlTransaction transaction;

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

            transaction = null;

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
            //TODO: This should have a check if the company name already exists and, in this case, only return its ID.
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
            Boolean myTransaction = false;
            if (transaction == null)
            {
                myTransaction = true;
                transaction = connection.BeginTransaction();
            }
            var companyID = AddCompany(brand.Company);
            //TODO: This should have a check if the company name already exists and, in this case, only return its ID.
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = string.Format("INSERT INTO {0}brand(name, companyID) VALUES(@name, @companyID)", TablePrefix);
            cmd.Parameters.AddWithValue("@name", brand.Name);
            cmd.Parameters.AddWithValue("@companyID", companyID);
            cmd.ExecuteNonQuery();
            if (myTransaction)
            {
                transaction.Commit();
                transaction = null;
            }
            return cmd.LastInsertedId;
        }


        public Brand GetBrandByID(long brandID)
        {
            String query = string.Format(
                 @"SELECT {0}brand.id AS bid, {0}brand.name AS bname, {0}company.id AS cid, {0}company.name AS cname
                   FROM   {0}brand JOIN {0}company ON {0}brand.companyID = {0}company.id
                    AND   {0}brand.id = @id", TablePrefix);
            var data = (IDictionary<string, object>)connection.Query(query, new { id = brandID }).FirstOrDefault();

            Company company = new Company((long)data["cid"], (string)data["cname"]);
            Brand brand = new Brand((long)data["bid"], (string)data["bname"], company);

            return brand;
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


        public long AddProduct(Product product)
        {
            Boolean myTransaction = false;
            if (transaction == null)
            {
                myTransaction = true;
                transaction = connection.BeginTransaction();
            }
            var brandID = AddBrand(product.Brand);
            //TODO: This should have a check if the company name already exists and, in this case, only return its ID.
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = string.Format(@"
                  INSERT INTO {0}product(name, brandID, barcode, measuringUnit, amount)
                  VALUES(@name, @brandID, @barcode, @measuringUnit, @amount)", TablePrefix);
            cmd.Parameters.AddWithValue("@name", product.Name);
            cmd.Parameters.AddWithValue("@brandID", brandID);
            cmd.Parameters.AddWithValue("@barcode", product.Barcode);
            cmd.Parameters.AddWithValue("@amount", product.Amount);
            cmd.Parameters.AddWithValue("@measuringUnit", product.MeasuringUnit);
            cmd.ExecuteNonQuery();
            if (myTransaction)
            {
                transaction.Commit();
                transaction = null;
            }
            return cmd.LastInsertedId;
        }


        public Product GetProductByID(long productID)
        {
            String query = string.Format(@"
                   SELECT {0}product.id AS pid, {0}product.name AS pname, {0}product.barcode, {0}product.amount, {0}product.measuringUnit,
                          {0}brand.id AS bid, {0}brand.name AS bname,
                          {0}company.id AS cid, {0}company.name AS cname
                   FROM   {0}product
                   JOIN   {0}brand ON {0}product.brandID = {0}brand.id
                   JOIN   {0}company ON {0}brand.companyID = {0}company.id
                    AND   {0}product.id = @id", TablePrefix);
            var data = (IDictionary<string, object>)connection.Query(query, new { id = productID }).FirstOrDefault();

            Company company = new Company((long)data["cid"], (string)data["cname"]);
            Brand brand = new Brand((long)data["bid"], (string)data["bname"], company);
            Product product = new Product((long)data["pid"], (string)data["pname"], brand, (string)data["barcode"], (int)data["amount"], (string)data["measuringUnit"]);

            return product;
        }


        public Product GetProductByBarcode(string barcode)
        {
            String query = string.Format(@"
                   SELECT {0}product.id AS pid, {0}product.name AS pname, {0}product.barcode, {0}product.amount, {0}product.measuringUnit,
                          {0}brand.id AS bid, {0}brand.name AS bname,
                          {0}company.id AS cid, {0}company.name AS cname
                   FROM   {0}product
                   JOIN   {0}brand ON {0}product.brandID = {0}brand.id
                   JOIN   {0}company ON {0}brand.companyID = {0}company.id
                    AND   {0}product.barcode = @barcode", TablePrefix);
            var data = (IDictionary<string, object>)connection.Query(query, new { barcode = barcode }).FirstOrDefault();

            Company company = new Company((long)data["cid"], (string)data["cname"]);
            Brand brand = new Brand((long)data["bid"], (string)data["bname"], company);
            Product product = new Product((long)data["pid"], (string)data["pname"], brand, (string)data["barcode"], (int)data["amount"], (string)data["measuringUnit"]);

            return product;
        }


        public Offer GetOfferByID(long offerID)
        {
            String query = string.Format(@"
                   SELECT {0}offer.id AS oid, {0}offer.senderID, {0}offer.time, {0}offer.price, {0}offer.marketID,
                          {0}product.id AS pid, {0}product.name AS pname, {0}product.barcode, {0}product.amount, {0}product.measuringUnit,
                          {0}brand.id AS bid, {0}brand.name AS bname,
                          {0}company.id AS cid, {0}company.name AS cname,
                          {0}location.id AS lid, {0}location.longitude, {0}location.latitude, {0}location.accuracy
                   FROM   {0}offer
                   JOIN   {0}product ON {0}offer.productID = {0}product.id
                   JOIN   {0}brand ON {0}product.brandID = {0}brand.id
                   JOIN   {0}company ON {0}brand.companyID = {0}company.id
                   JOIN   {0}location ON {0}offer.locationID = {0}location.id
                    AND   {0}offer.id = @id", TablePrefix);
            var data = (IDictionary<string, object>)connection.Query(query, new { id = offerID }).FirstOrDefault();

            Company company = new Company((long)data["cid"], (string)data["cname"]);
            Brand brand = new Brand((long)data["bid"], (string)data["bname"], company);
            Product product = new Product((long)data["pid"], (string)data["pname"], brand, (string)data["barcode"], (int)data["amount"], (string)data["measuringUnit"]);
            GeoLocation location = new GeoLocation((long)data["lid"], (double)data["longitude"], (double)data["latitude"], (int)data["accuracy"]);
            
            Market market = null;
            if (data["marketID"] != null)
            {
                market = GetMarketByID((long)data["marketID"]);
            }

            Offer offer = new Offer((long)data["oid"], (long)data["senderID"], (DateTime)data["time"], product, (float)data["price"], location, market);
            return offer;
        }


        public Market GetMarketByID(Int64 marketID)
        {
            String query = string.Format(@"
                   SELECT {0}market.id AS mid, {0}market.name AS mname, {0}market.address,
                          {0}marketchain.id AS mcid, {0}marketchain.name AS mcname,
                          {0}location.id as lid, {0}location.longitude, {0}location.latitude, {0}location.accuracy, 
                          {0}market_location.order
                   FROM   {0}market
                   JOIN   {0}marketchain ON {0}market.chainID = {0}marketchain.id
                   JOIN   {0}market_location ON {0}market_location.marketID = {0}market.id
                   JOIN   {0}location ON {0}market_location.locationID = {0}location.id
                    AND   {0}market.id = @id
                   ORDER BY {0}market_location.order ASC", TablePrefix);

            var data = (List<object>)connection.Query(query, new { id = marketID }).ToList();

            List<GeoLocation> vertices = new List<GeoLocation>();
            foreach (var item in data)
            {
                var dataSet = (IDictionary<string, object>)item;
                vertices.Add(new GeoLocation((long)dataSet["lid"], (double)dataSet["longitude"], (double)dataSet["latitude"], (int)dataSet["accuracy"]));
            }
            GeoPolygon locationArea = new GeoPolygon(vertices);

            var first = (IDictionary<string, object>)data.First();
            MarketChain mchain = new MarketChain((long)first["mcid"], (string)first["mcname"]);
            Market market = new Market((long)first["mid"], (string)first["mname"], (string)first["address"], mchain, locationArea);

            return market;
        }


        public List<Offer> GetUnmappedOffers(Int32 count)
        {
            String query = string.Format(@"
                   SELECT {0}offer.id AS oid, {0}offer.senderID, {0}offer.time, {0}offer.price, {0}offer.marketID,
                          {0}product.id AS pid, {0}product.name AS pname, {0}product.barcode, {0}product.amount, {0}product.measuringUnit,
                          {0}brand.id AS bid, {0}brand.name AS bname,
                          {0}company.id AS cid, {0}company.name AS cname,
                          {0}location.id AS lid, {0}location.longitude, {0}location.latitude, {0}location.accuracy
                   FROM   {0}offer
                   JOIN   {0}product ON {0}offer.productID = {0}product.id
                   JOIN   {0}brand ON {0}product.brandID = {0}brand.id
                   JOIN   {0}company ON {0}brand.companyID = {0}company.id
                   JOIN   {0}location ON {0}offer.locationID = {0}location.id
                    AND   {0}offer.marketID IS NULL
                   LIMIT  0, @numOffers", TablePrefix);

            var data = (List<object>)connection.Query(query, new { numOffers = count }).ToList();

            List<Offer> offerList = new List<Offer>();

            foreach (var item in data)
            {
                var dataSet = (IDictionary<string, object>)item;
                Company company = new Company((long)dataSet["cid"], (string)dataSet["cname"]);
                Brand brand = new Brand((long)dataSet["bid"], (string)dataSet["bname"], company);
                Product product = new Product((long)dataSet["pid"], (string)dataSet["pname"], brand, (string)dataSet["barcode"], (int)dataSet["amount"], (string)dataSet["measuringUnit"]);
                GeoLocation location = new GeoLocation((long)dataSet["lid"], (double)dataSet["longitude"], (double)dataSet["latitude"], (int)dataSet["accuracy"]);
                Market market = null;
                Offer offer = new Offer((long)dataSet["oid"], (long)dataSet["senderID"], (DateTime)dataSet["time"], product, (float)dataSet["price"], location, market);
                offerList.Add(offer);
            }

            return offerList;
        }


        public void TableSetUp() {
            string query = string.Format(@"
                CREATE TABLE IF NOT EXISTS `{0}brand` (
                  `id` bigint(20) NOT NULL AUTO_INCREMENT,
                  `name` text NOT NULL,
                  `companyID` bigint(20) NOT NULL,
                  PRIMARY KEY (`id`)
                ) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=5 ;

                CREATE TABLE IF NOT EXISTS `{0}company` (
                  `id` bigint(20) NOT NULL AUTO_INCREMENT,
                  `name` text NOT NULL,
                  PRIMARY KEY (`id`)
                ) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=6 ;

                CREATE TABLE IF NOT EXISTS `{0}location` (
                  `id` bigint(20) NOT NULL AUTO_INCREMENT,
                  `longitude` double NOT NULL,
                  `latitude` double NOT NULL,
                  `accuracy` int(11) NOT NULL,
                  PRIMARY KEY (`id`)
                ) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=2 ;

                CREATE TABLE IF NOT EXISTS `pl_market` (
                  `id` bigint(20) NOT NULL AUTO_INCREMENT,
                  `name` text NOT NULL,
                  `address` text NOT NULL,
                  `chainID` bigint(20) NOT NULL,
                  PRIMARY KEY (`id`)
                ) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=2 ;

                CREATE TABLE IF NOT EXISTS `pl_marketchain` (
                  `id` bigint(20) NOT NULL AUTO_INCREMENT,
                  `name` text NOT NULL,
                  PRIMARY KEY (`id`)
                ) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=2 ;

                CREATE TABLE IF NOT EXISTS `pl_market_location` (
                  `marketID` bigint(20) NOT NULL,
                  `locationID` bigint(20) NOT NULL,
                  `order` int(11) NOT NULL,
                  PRIMARY KEY (`marketID`,`locationID`,`order`)
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8;

                CREATE TABLE IF NOT EXISTS `{0}offer` (
                  `id` bigint(20) NOT NULL AUTO_INCREMENT,
                  `senderID` bigint(20) NOT NULL,
                  `time` datetime NOT NULL,
                  `productID` bigint(20) NOT NULL,
                  `price` float NOT NULL,
                  `locationID` bigint(20) NOT NULL,
                  `marketID` bigint(20) DEFAULT NULL,
                  PRIMARY KEY (`id`)
                ) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=2 ;

                CREATE TABLE IF NOT EXISTS `{0}product` (
                  `id` bigint(20) NOT NULL AUTO_INCREMENT,
                  `name` text NOT NULL,
                  `brandID` bigint(20) NOT NULL,
                  `barcode` text NOT NULL,
                  `amount` int(11) NOT NULL,
                  `measuringUnit` text NOT NULL,
                  PRIMARY KEY (`id`)
                ) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=5 ;
                ", TablePrefix);
            connection.QueryMultiple(query);
        }


        public void TableTearDown()
        {
            string query = string.Format(@"
                DROP TABLE IF EXISTS `{0}brand`;
                DROP TABLE IF EXISTS `{0}company`;
                DROP TABLE IF EXISTS `{0}location`;
                DROP TABLE IF EXISTS `{0}market`;
                DROP TABLE IF EXISTS `{0}marketchain`;
                DROP TABLE IF EXISTS `{0}market_location`;
                DROP TABLE IF EXISTS `{0}offer`;
                DROP TABLE IF EXISTS `{0}product`;
                
                ", TablePrefix);
            connection.QueryMultiple(query);
        }
    }
}
