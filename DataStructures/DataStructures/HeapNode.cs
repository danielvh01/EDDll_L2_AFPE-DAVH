using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataStructures
{
    internal class HeapNode<T> : IComparable
    {
        public string priority;
        public T value { get; set; }

        public int height;

        public HeapNode(T newvalue, string p)
        {
            value = newvalue;
            height = 1;
            priority = p;
        }

        public int CompareTo(object obj)
        {
            var comparer = ((HeapNode<T>)obj).priority;
            return comparer.CompareTo(priority);
        }

    }
}
