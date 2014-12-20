using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdLoc
{
    public class Brand
    {

        public Int64 ID { get; private set; }
        public String Name { get; private set; }
        public Company Company { get; private set; }

        public Brand(String name, Company company)
        {
            Name = name;
            Company = company;
        }

        public Brand(Int64 id, String name, Company company)
        {
            ID = id;
            Name = name;
            Company = company;
        }

        public override String ToString()
        {
            return string.Format("Brand: [ID={0}, Name=\"{1}\", Company={2}]", ID, Name, Company);
        }

    }
}
