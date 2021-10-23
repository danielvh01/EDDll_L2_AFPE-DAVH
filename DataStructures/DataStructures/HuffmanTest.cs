using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    public class HuffmanTest
    {
        const int MAX_TREE_HT = 100;

        

        HuffmanHeap heap;
        HuffmanHeapNode root;

        bool isLeaf(HuffmanHeapNode root)
        {
            return (root.left == null) && (root.right == null);
        }

        HuffmanHeap crearHeap(char[] datos, int[] frecuencias, int length)
        {
            heap = new HuffmanHeap(length);

            for (int i = 0; i < length; i++)
            {
                heap.array[i] = new HuffmanHeapNode(datos[i], frecuencias[i]);
            }
            heap.length = length;
            heap.buildHeap();

            return heap;
        }

        HuffmanHeapNode buildTree(char[] datos, int[] frecuencias, int lenght)
        {
            HuffmanHeapNode left, right, top;

            HuffmanHeap heap = crearHeap(datos, frecuencias, lenght);

            while (!heap.lenghtUno())
            {
                left = heap.extractMin();
                right = heap.extractMin();

                top = new HuffmanHeapNode('$', left.frecuencia + right.frecuencia);

                top.left = left;
                top.right = right;

                heap.Insert(top);
            }

            return heap.extractMin();
        }

        void codigosBinarios(HuffmanHeapNode root, int[] arr, int top)
        {
            if (root.left != null)
            {
                arr[top] = 0;
                codigosBinarios(root.left, arr, top + 1);
            }
            if (root.right != null)
            {
                arr[top] = 1;
                codigosBinarios(root.right, arr, top + 1);
            }
            if(isLeaf(root))
            {
                root.binaryCode = "";
                for(int i = 0; i < top; i++)
                {
                    root.binaryCode += arr[i].ToString();
                }

            }
        }

        void HuffmanCodes(char[] datos, int[] frecuencias, int size)
        {
            root = buildTree(datos, frecuencias, size);

            int[] arr = new int[MAX_TREE_HT];
            codigosBinarios(root, arr, 0);
        }

        public byte[] Compress(byte[] content)
        {
            int frecuenciaMayor = 0;
            DoubleLinkedList<HuffmanHeapNode> dictionary = new DoubleLinkedList<HuffmanHeapNode>();
            for (int i = 0; i < content.Length; i++)
            {
                var temp = dictionary.Find(x => x.caracter.CompareTo((char)content[i]));
                if (temp != null)
                {
                    temp.frecuencia++;
                    if(temp.frecuencia > frecuenciaMayor)
                    {
                        frecuenciaMayor = temp.frecuencia;
                    }
                }
                else
                {
                    dictionary.InsertAtEnd(new HuffmanHeapNode((char)content[i], 1));
                }
            }

            char[] caracteres = new char[dictionary.Length];
            int[] frecuencias = new int[dictionary.Length];

            for(int i = 0; i < dictionary.Length; i++)
            {
                var temp = dictionary.Get(i);
                caracteres[i] = temp.caracter;
                frecuencias[i] = temp.frecuencia;
            }

            HuffmanCodes(caracteres, frecuencias, dictionary.Length);

            int bytesForFrecuencys = (int)Math.Round(Math.Log(frecuenciaMayor) / Math.Log(256), 0, MidpointRounding.ToPositiveInfinity);
            return new byte[0];
        }

    }
}
