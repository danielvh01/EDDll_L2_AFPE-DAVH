using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    public class Huffman
    {
        internal Heap<HuffmanNode> heap;

        Huffman()
        {

        }

        public string Compress(string text)
        {

        }

        private void buildHeap(string text)
        {
            DoubleLinkedList<HuffmanNode> list = new DoubleLinkedList<HuffmanNode>();
            int cant = 0;
            while(text.Length > 0)
            {
                cant++;
                int cont = 1;
                char character = text[0];
                text = text.Remove(0, 1);
                for(int i = 0; i < text.Length; i++)
                {
                    if(text[i] == character)
                    {
                        text = text.Remove(i, 1);
                        cont++;
                    }
                }
                list.InsertAtEnd(new HuffmanNode(character, cont));
            }

            heap = new Heap<HuffmanNode>(cant);
            foreach(var node in list)
            {
                node.frecuency = node.frecuency / cant;
                heap.insertKey(node, node.frecuency.ToString());
            }
        }
    }
}
