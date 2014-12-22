using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdLoc
{
    public class Offer
    {
        public Int64 ID { get; private set; }
        public Int64 Sender { get; private set; }       // To be replaced later with a "user" class
        public DateTime Time { get; private set; }
        public Product Product { get; private set; }
        public float Price { get; private set; }        // Currently in the local currency of the market
        //public Location Location { get; private set; }
        //public Market Market { get; private set; }

        public Offer(Int64 sender, DateTime time, Product product, float price)
        {
            Sender = sender;
            Time = time;
            Product = product;
            Price = price;
        }

        public Offer(Int64 id, Int64 sender, DateTime time, Product product, float price)
        {
            ID = id;
            Sender = sender;
            Time = time;
            Product = product;
            Price = price;
        }

        public override String ToString()
        {
            return string.Format("Offer: [ID={0}, Sender=\"{1}\", Time={2}, Price={3}, Product={4}]", ID, Sender, Time, Price, Product);
        }
    }
}
