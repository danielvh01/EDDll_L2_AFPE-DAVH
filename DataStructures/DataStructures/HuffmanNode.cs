using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    internal class HuffmanNode : IComparable
    {
        public char value;
        public string binary_value;
        public int frecuency;
        public double probability;
        public HuffmanNode rigth;
        public HuffmanNode left;
        public bool leaf;

        public HuffmanNode(char new_value, int new_frecuency, double new_probability, bool l)
        {
            value = new_value;
            frecuency = new_frecuency;
            probability = new_probability;
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
