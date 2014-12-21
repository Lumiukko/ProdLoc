using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdLoc
{
    class Product
    {
        
        public Int64 ID { get; private set; }
        public String Name { get; private set; }
        public Brand Brand { get; private set; }
        //public List<Category> Categories { get; private set; }

        public Product(String name, Brand brand)
        {
            Name = name;
            Brand = brand;
        }

        public Product(Int64 id, String name, Brand brand)
        {
            ID = id;
            Name = name;
            Brand = brand;
        }

        public override String ToString()
        {
            return string.Format("Product: [ID={0}, Name=\"{1}\", Brand={2}]", ID, Name, Brand);
        }
    }
}
