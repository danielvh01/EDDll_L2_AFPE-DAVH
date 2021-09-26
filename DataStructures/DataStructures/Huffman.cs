using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    public class Huffman : IHuffmanCompressor
    {
        internal Heap<HuffmanNode> heap;
        internal DoubleLinkedList<HuffmanNode> nodes;
        int bytesPerChar = 1;
        int tempLength = 0;
        public Huffman()
        {
            nodes = new DoubleLinkedList<HuffmanNode>();
        }
        
        public byte[] CompressContent(string text)
        {
            byte[] result;
            
            buildHeap(text);

            buildNodes();

            var rootNode = heap.getMin();

            buildBinaryCodes(rootNode);

            string binaryText = "";
            for (int i = 0; i < text.Length; i++)
            {
                var character = nodes.Find(x => x.value.CompareTo(text[i]));
                binaryText += character.binary_value;
            }

            int cant = 8 - binaryText.Length % 8;
            while (cant > 0)
            {
                binaryText += 0;
                cant--;
            }
            result = new byte[binaryText.Length / 8];
            for (int i = 0; i < (binaryText.Length / 8); i++)
            {
                result[i] = binaryToByte(binaryText.Substring(8 * i, 8));
            }
            return result;
        }
        private void buildNodes()
        {
            while (heap.Length() > 1)
            {
                var x = heap.extractMin().value;
                var y = heap.extractMin().value;
                HuffmanNode node = new HuffmanNode('\0', (x.frecuency + y.frecuency), false);
                node.left = x;
                node.rigth = y;
                heap.insertKey(node, node.frecuency.ToString());
            }
        }
        public byte[] Compress(string text)
        {
            byte[] result;
            byte[] content = CompressContent(text);
            tempLength = content.Length;
            int lenghtFrecuencys = (bytesPerChar + 1) * nodes.Length;
            result = new byte[2 + lenghtFrecuencys + content.Length];
            result[0] = Convert.ToByte(Convert.ToChar(bytesPerChar));
            for(int i = 0; i < nodes.Length; i++)
            {
                int character = i * (bytesPerChar + 1);
                var x = nodes.Get(i);
                result[1 + character] = Convert.ToByte(x.value);
                int number = x.frecuency;
                for(int j = 0; j < bytesPerChar; j++)
                {
                    result[1 + character + (bytesPerChar - j)] = Convert.ToByte(Convert.ToChar(number % 256));
                    number = number / 256;
                }
            }
            content.CopyTo(result, 2 + lenghtFrecuencys);
            return result;
        }

        public int getCompressedLength()
        {
            return tempLength;
        }

        private byte binaryToByte(string binaryByte)
        {
            int number = 0;
            for(int i = 0; i < 8; i++)
            {
                number += int.Parse(Math.Pow(2, 7 - i).ToString()) * int.Parse(binaryByte.Substring(i, 1));
            }
            return byte.Parse(number.ToString());
        }

        private void buildBinaryCodes(HuffmanNode root)
        {
            if(!root.leaf)
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
                for(int i = text.Length - 1; i >= 0; i--)
                {
                    if(text[i] == character)
                    {
                        text = text.Remove(i, 1);
                        cont++;
                    }
                }
                var node = new HuffmanNode(character, cont, true);
                nodes.InsertAtEnd(node);
                heap.insertKey(node, node.frecuency.ToString());
                if(cont / 256 > bytesPerChar)
                {
                    bytesPerChar = cont / 256;
                }
            }

        }

        private string byteToBinaryString(byte x)
        {
            int numericValue = Convert.ToInt32(x);
            string result = "";
            result += numericValue / 128;
            numericValue = numericValue % 128;
            result += numericValue / 64;
            numericValue = numericValue % 64;
            result += numericValue / 32;
            numericValue = numericValue % 32;
            result += numericValue / 16;
            numericValue = numericValue % 16;
            result += numericValue / 8;
            numericValue = numericValue % 8;
            result += numericValue / 4;
            numericValue = numericValue % 4;
            result += numericValue / 2;
            numericValue = numericValue % 2;
            result += numericValue / 1;
            numericValue = numericValue % 1;
            return result;
        }

        public string Decompress(byte[] compressText)
        {
            int bytesPerChar = Convert.ToInt32(compressText[0]);
            var separator = Convert.ToByte(Convert.ToChar(0));
            int index = 1;
            int cantLetters = 0;
            heap = new Heap<HuffmanNode>(compressText.Length);

            while (compressText[index] != separator)
            {
                char letter = Convert.ToChar(compressText[index]);
                int frecuency = 0;
                for (int i = 0; i < bytesPerChar; i++)
                {
                    frecuency += Convert.ToInt32(compressText[index + (bytesPerChar - i)]) * Convert.ToInt32(Math.Pow(256, i));
                }
                cantLetters += frecuency;
                var node = new HuffmanNode(letter, frecuency, true);
                nodes.InsertAtEnd(node);
                heap.insertKey(node, frecuency.ToString());
                index += 1 + bytesPerChar;
            }

            buildNodes();

            var rootNode = heap.getMin();

            buildBinaryCodes(rootNode);

            string result = "";
            string binText = "";
            for(int i = index + 1; i < compressText.Length; i++)
            {
                binText += byteToBinaryString(compressText[i]);
            }
            bool find;
            for (int i = 0; i < cantLetters; i++)
            {
                find = true;
                while(find)
                {
                    for(int j = 0; j < nodes.Length; j++)
                    {
                        var node = nodes.Get(j);
                        while(node.binary_value.Length > binText.Length)
                        {
                            binText += 0;
                        }
                        if (binText.Substring(0, node.binary_value.Length) == node.binary_value)
                        {
                            result += node.value;
                            binText = binText.Remove(0, node.binary_value.Length);
                            find = false;
                            break;
                        }
                    }
                }
            }
            return result;
        }
    }
}
