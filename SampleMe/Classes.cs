using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMe
{
    class OrderRecord
    {
        public string TransactionId { get; set; }
        public string ProductNumber { get; set; }

        public OrderRecord(string[] input)
        {
            this.TransactionId = input[0];
            this.ProductNumber = input[1];
        }
    }

    class ProductEdges
    {
        public string Source { get; set; }
        public string Target { get; set; }
        public double Weight { get; set; }
    }
}
