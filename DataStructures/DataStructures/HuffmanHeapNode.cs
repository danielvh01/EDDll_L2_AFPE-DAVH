using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    internal class HuffmanHeapNode : IComparable
    {
        internal byte caracter;
        internal int frecuencia;
        internal HuffmanHeapNode left;
        internal HuffmanHeapNode right;
        internal string binaryCode;

        internal HuffmanHeapNode(byte c, int f)
        {
            caracter = c;
            frecuencia = f;
            left = null;
            right = null;
        }

        public int CompareTo(object obj)
        {
            var comparer = ((HuffmanHeapNode)obj).caracter;
            return caracter.CompareTo(comparer);
        }

    }
}
