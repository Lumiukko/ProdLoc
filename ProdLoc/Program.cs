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
            IDataStorage ds = new DataStorageNonPersistent();
            ds.Connect();

            Company c0 = new Company("Nestlé");           
            ulong c0ID = ds.AddCompany(c0);

            Company c0read = ds.GetCompanyByName("Nestlé");
            Console.WriteLine("This company should have the ID={0}: {1}", c0ID, c0read);
            
            ds.Disconnect();


            Console.ReadLine();
        }
    }
}
