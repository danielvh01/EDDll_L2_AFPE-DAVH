using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    public class Huffman
    {
        internal Heap<HuffmanNode> heap;
        internal DoubleLinkedList<HuffmanNode> binaryCodes;
        int bytesPerChar = 1;

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
            byte[] content = new byte[binaryText.Length / 8];
            for(int i = 0; i < (binaryText.Length / 8); i++)
            {
                content[i] = binaryToByte(binaryText.Substring(8*i, 8));
            }
            int lenghtFrecuencys = (bytesPerChar + 1) * binaryCodes.Length;
            result = new byte[3 + lenghtFrecuencys + content.Length];
            result[0] = Convert.ToByte(char.Parse(bytesPerChar.ToString()));
            for(int i = 0; i < binaryCodes.Length; i++)
            {
                int character = i * (bytesPerChar + 1);
                var x = binaryCodes.Get(i);
                result[2 + character] = Convert.ToByte(x.value);
                int number = x.frecuency;
                for(int j = 0; j < bytesPerChar; j++)
                {
                    result[2 + character + (bytesPerChar - j)] = Convert.ToByte(Convert.ToChar(number % 256));
                    number = number / 256;
                }
            }
            content.CopyTo(result, 3 + lenghtFrecuencys);
            return result;
        }

        public byte binaryToByte(string binaryByte)
        {
            int number = 0;
            for(int i = 0; i < 8; i++)
            {
                number += int.Parse(Math.Pow(2, 7 - i).ToString()) * int.Parse(binaryByte.Substring(i, 1));
            }
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
            heap = new Heap<HuffmanNode>(text.Length);
            int cant = text.Length;
            while(text.Length > 0)
            {
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
                var node = new HuffmanNode(character, cont, true);
                heap.insertKey(node, node.frecuency.ToString());
                if(cont / 256 > bytesPerChar)
                {
                    bytesPerChar = cont / 256;
                }
            }

        }
    }
}
