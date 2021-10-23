using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    class HuffmanHeap
    {
        internal int length;
        int capacidad;
        internal HuffmanHeapNode[] array;

        internal HuffmanHeap (int capacidad)
        {
            length = 0;
            this.capacidad = capacidad;
            array = new HuffmanHeapNode[capacidad];
        }

        internal void swap(ref HuffmanHeapNode a, ref HuffmanHeapNode b)
        {
            HuffmanHeapNode temp = a;
            a = b;
            b = temp;
        }

        internal void heapify(int idx)
        {
            int menor = idx;
            int left = 2 * idx + 1;
            int right = 2 * idx + 2;

            if(left < length && array[left].frecuencia < array[menor].frecuencia)
            {
                menor = left;
            }
            if(right < length && array[right].frecuencia < array[menor].frecuencia)
            {
                menor = right;
            }
            if(menor != idx)
            {
                swap(ref array[menor], ref array[idx]);
                heapify(menor);
            }
        }

        internal bool lenghtUno()
        {
            return length == 1;
        }

        internal HuffmanHeapNode extractMin()
        {
            HuffmanHeapNode temp = array[0];
            array[0] = array[length - 1];
            --length;
            heapify(0);
            return temp;
        }

        internal void Insert(HuffmanHeapNode newNode)
        {
            int i = length++;

            while(i > 0 && newNode.frecuencia < array[(i-1)/2].frecuencia)
            {
                array[i] = array[(i - 1) / 2];
                i = (i - 1) / 2;
            }

            array[i] = newNode;
        }

        internal void buildHeap()
        {
            int n = length - 1;
            for(int i = (n - 1) / 2; i >= 0; --i)
            {
                heapify(i);
            }
        }


    }
}
