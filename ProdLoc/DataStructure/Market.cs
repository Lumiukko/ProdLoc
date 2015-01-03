using System;

namespace ProdLoc
{
    public class Market
    {
        public Int64 ID { get; private set; }
        public String Name { get; private set; }
        public String Address { get; private set; }
        public MarketChain MarketChain { get; private set; }
        public GeoPolygon LocationArea { get; private set; }

        public Market(String name, String address, MarketChain marketChain, GeoPolygon locationArea)
        {
            Name = name;
            Address = address;
            MarketChain = marketChain;
            LocationArea = locationArea;
        }

        public Market(Int64 id, String name, String address, MarketChain marketChain, GeoPolygon locationArea)
            : this(name, address, marketChain, locationArea)
        {
            ID = id;
        }

        public override String ToString()
        {
            return string.Format("Market: [ID={0}, Name=\"{1}\", Address={2}, MarketChain={3}, LocationArea={4}]", ID, Name, Address, MarketChain, LocationArea);
        }
    }
}
