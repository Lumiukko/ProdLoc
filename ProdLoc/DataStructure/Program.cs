using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProdLoc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /*
            IDataStorage ds = new DataStorageNonPersistent();
            ds.Connect();

            Company c0 = new Company("Nestlé");           
            ulong c0ID = ds.AddCompany(c0);

            Company c0byName = ds.GetCompanyByName("Nestlé");
            Console.WriteLine("This company should have the ID={0}: {1}", c0ID, c0byName);

            Company c0byID = ds.GetCompanyByID(c0ID);
            Console.WriteLine("This company should have the ID={0}: {1}", c0ID, c0byID);

            ds.Disconnect();
            */


            IDataStorage ds = new DataStorageMySQL();

            //Company c0 = new Company("Foobar");   

            ds.Connect();
            //ds.AddCompany(c0);
            //Company c0byName = ds.GetCompanyByName("Foobar");
            //Console.WriteLine(c0byName.ToString());

            //Brand b1 = ds.GetBrandByName("Lätta");

            //Product p0 = ds.GetProductByID(2);
            //Console.WriteLine(p0.ToString());

            Company c0 = new Company("TestCompany");
            Brand b0 = new Brand("TestBrand", c0);
            Product p0 = new Product("TestProduct", b0, "1234567890", 500, "t");

            Product p1 = ds.GetProductByBarcode("7613032872731");
            Console.WriteLine(p1.ToString());

            ds.Disconnect();

            Console.ReadLine();
        }
    }
}
