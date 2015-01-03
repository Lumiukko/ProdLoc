using System;

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
            : this(name, company)
        {
            ID = id;
        }

        public override String ToString()
        {
            return string.Format("Brand: [ID={0}, Name=\"{1}\", Company={2}]", ID, Name, Company);
        }

    }
}
