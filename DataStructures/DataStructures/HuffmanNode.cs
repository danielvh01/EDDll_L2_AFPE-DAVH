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
        public HuffmanNode rigth;
        public HuffmanNode left;
        public bool leaf;

        public HuffmanNode(char new_value, double new_frecuency, bool l)
        {
            value = new_value;
            frecuency = new_frecuency;
            binary_value = "";
            rigth = null;
            left = null;
            leaf = l;
        }

        public int CompareTo(object obj)
        {
            var comparer = ((HuffmanNode)obj).frecuency;
            return frecuency.CompareTo(comparer);
        }
    }
}
