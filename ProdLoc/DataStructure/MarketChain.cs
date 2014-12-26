using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdLoc
{
    public class MarketChain
    {
        public Int64 ID { get; private set; }
        public String Name { get; private set; }

        public MarketChain(String name)
        {
            Name = name;
        }

        public MarketChain(Int64 id, String name)
        {
            ID = id;
            Name = name;
        }

        public override string ToString()
        {
            return string.Format("MarketChain: [ID={0}, Name=\"{1}\"]", ID, Name);
        }
    }
}
