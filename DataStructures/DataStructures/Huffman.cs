using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    public class Huffman
    {
        internal Heap<HuffmanNode> heap;
        internal DoubleLinkedList<HuffmanNode> binaryCodes;

        public Huffman()
        {
            binaryCodes = new DoubleLinkedList<HuffmanNode>();
        }

        public byte[] Compress(string text)
        {
            byte[] result;
            buildHeap(text);
            while (heap.Length() > 1)
            {
                var x = heap.extractMin().value;
                var y = heap.extractMin().value;
                HuffmanNode node = new HuffmanNode('\0', (x.frecuency + y.frecuency), false);
                node.left = x;
                node.rigth = y;
                heap.insertKey(node, node.frecuency.ToString());
            }
            string binaryText = "";
            var rootNode = heap.getMin();
            buildBinaryCodes(rootNode);
            for (int i = 0; i < text.Length; i++)
            {
                var character = binaryCodes.Find(x => x.value.CompareTo(text[i]));
                binaryText += character.binary_value;
            }

            int cant = 8 - binaryText.Length % 8;
            while (cant > 0)
            {
                binaryText += 0;
                cant--;
            }
            result = new byte[binaryText.Length / 8];
            for(int i = 0; i < (binaryText.Length / 8); i++)
            {
                result[i] = binaryToByte(binaryText.Substring(8*i, 8));
            }
            return result;
        }

        public byte binaryToByte(string binaryByte)
        {
            int number = 0;
            for(int i = 0; i < 8; i++)
            {
                number += int.Parse(Math.Pow(2, 7 - i).ToString()) * int.Parse(binaryByte.Substring(i, 1));
            }
            char.ConvertFromUtf32(number);
            return byte.Parse(number.ToString());
        }

        void buildBinaryCodes(HuffmanNode root)
        {
            if(root.leaf)
            {
                binaryCodes.InsertAtEnd(root);
            }
            else
            {
                root.left.binary_value = root.binary_value + 0;
                root.rigth.binary_value = root.binary_value + 1;
                buildBinaryCodes(root.left);
                buildBinaryCodes(root.rigth);
            }
            
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
                        cant++;
                    }
                }
                list.InsertAtEnd(new HuffmanNode(character, cont, true));
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
