using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdLoc
{
    public class Product
    {
        
        public Int64 ID { get; private set; }
        public String Name { get; private set; }
        public Brand Brand { get; private set; }
        public String Barcode { get; private set; }
        public Int32 Amount { get; private set; }
        public String MeasuringUnit { get; private set; }
        //public List<Category> Categories { get; private set; }

        public Product(String name, Brand brand)
        {
            Name = name;
            Brand = brand;
        }

        public Product(Int64 id, String name, Brand brand, String barcode, Int32 amount, String measuringUnit)
        {
            ID = id;
            Name = name;
            Brand = brand;
            Barcode = barcode;
            Amount = amount;
            MeasuringUnit = measuringUnit;
        }

        public override String ToString()
        {
            return string.Format("Product: [ID={0}, Name=\"{1}\", Brand={2}, Barcode={3}, Amount={4}, MeasuringUnit={5}]", ID, Name, Brand, Barcode, Amount, MeasuringUnit);
        }
    }
}