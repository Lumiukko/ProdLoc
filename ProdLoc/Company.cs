using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdLoc
{
    public class Company
    {
        public UInt64 ID { get; private set; }
        public String Name { get; private set; }


        public Company(String name)
        {
            Name = name;
        }

        public Company(UInt64 id, String name)
        {
            ID = id;
            Name = name;
        }

        public override string ToString()
        {
            return string.Format("Company: [ID={0}, Name=\"{1}\"], ", ID, Name);
        }
    }
}
