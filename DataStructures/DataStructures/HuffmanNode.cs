using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    internal class HuffmanNode : IComparable
    {
        public char value;
        public string binary_value;
        public double frecuency;
        HuffmanNode rigth;
        HuffmanNode left;

        public HuffmanNode(char new_value, double new_frecuency)
        {
            value = new_value;
            frecuency = new_frecuency;
            binary_value = "";
            rigth = null;
            left = null;
        }

        public bool leaf()
        {
            return (rigth == null && left == null);
        }

    }
}
