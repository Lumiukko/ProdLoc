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
        public Market Market { get; private set; }
        public GeoLocation Location { get; private set; }

        public Offer(Int64 sender, DateTime time, Product product, float price, GeoLocation location, Market market)
        {
            Sender = sender;
            Time = time;
            Product = product;
            Price = price;
            Location = location;
            Market = market;
        }

        public Offer(Int64 id, Int64 sender, DateTime time, Product product, float price, GeoLocation location, Market market)
        {
            ID = id;
            Sender = sender;
            Time = time;
            Product = product;
            Price = price;
            Location = location;
            Market = market;
        }

        /// <summary>
        /// Returns a map of Markets with an associated probability value of how likely the
        /// particular Market is associated with the offers location.
        /// </summary>
        /// <returns>Map of Markets and probabilities that correspond to the offers location.</returns>
        public Dictionary<Market, float> GetMarketMappings()
        {
            throw new NotImplementedException();
        }


        public override String ToString()
        {
            return string.Format("Offer: [ID={0}, Sender={1}, Time={2}, Price={3}, Product={4}, Location={5}, Market={6}]", ID, Sender, Time, Price, Product, Location, (Market != null ? Market.ToString() : "NONE"));
        }
    }
}
