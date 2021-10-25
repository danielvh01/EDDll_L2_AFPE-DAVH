using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    public class HuffmanTest
    {
        const int MAX_TREE_HT = 100;

        List<HuffmanHeapNode> nodes = new List<HuffmanHeapNode>();

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
                nodes.Add(root);
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

            int mayor = 0;
            int menor = 0;
            for (int i = 0; i < dictionary.Length; i++)
            {
                var temp = dictionary.Get(i);
                caracteres[i] = temp.caracter;
                frecuencias[i] = temp.frecuencia;
                if(temp.frecuencia < frecuencias[menor])
                {
                    menor = i;
                }
                else if(frecuencias[mayor] < temp.frecuencia)
                {
                    mayor = i;
                }
            }

            HuffmanCodes(caracteres, frecuencias, dictionary.Length);

            nodes.Sort((x,y)=> y.frecuencia.CompareTo(x.frecuencia));
            
            int bytesForFrecuencys = (int)Math.Round(Math.Log(frecuenciaMayor) / Math.Log(256), 0, MidpointRounding.ToPositiveInfinity);

            string textInBinary = "";

            string textResult = "";

            foreach (char letra in content)
            {
                textInBinary += nodes.Find(x => x.caracter == letra).binaryCode;
                if(textInBinary.Length >= 8)
                {
                    textResult += (char)Convert.ToInt32(textInBinary.Substring(0, 8), 2);
                    textInBinary = textInBinary.Remove(0, 8);
                }
            }

            if (textInBinary.Length > 0)
            {
                while(textInBinary.Length < 8)
                {
                    textInBinary += 0;
                }
                textResult += (char)Convert.ToInt32(textInBinary.Substring(0, 8), 2);
                textInBinary = textInBinary.Remove(0, 8);
            }

            byte[] result = new byte[2 + (bytesForFrecuencys + 1) * nodes.Count + textResult.Length];

            result[0] = (byte)nodes.Count;
            result[1] = (byte)bytesForFrecuencys;

            
            for(int i = 0; i < nodes.Count; i++)
            {
                var x = nodes[i];
                result[2 + i*(bytesForFrecuencys + 1)] = (byte)x.caracter;
                int numero = x.frecuencia;
                for(int j = bytesForFrecuencys - 1; j >= 0; j--)
                {
                    result[3 + i * (bytesForFrecuencys + 1) + j] = (byte)(numero / Math.Pow(256, j));
                }
            }

            for(int i = 0; i < textResult.Length; i++)
            {
                result[2 + (bytesForFrecuencys + 1) * nodes.Count + i] = (byte)textResult[i];
            }

            return result;
        }

        public byte[] Decompress(byte[] content)
        {
            int CantCaracteres = content[0];
            int bytesForFrecuency = content[1];

            char[] caracteres = new char[CantCaracteres];
            int[] frecuencias = new int[CantCaracteres];

            int cantTotal = 0;

            for(int i = 0; i < CantCaracteres; i++)
            {
                caracteres[i] = (char)content[2 + i * (bytesForFrecuency + 1)];
                for(int j = 0; j < bytesForFrecuency; j++)
                {
                    frecuencias[i] += (int)(content[3 + i * (bytesForFrecuency + 1) + j] * Math.Pow(256, j));
                }
                cantTotal += frecuencias[i];
            }

            HuffmanCodes(caracteres, frecuencias, CantCaracteres);

            nodes.Sort((x, y) => y.frecuencia.CompareTo(x.frecuencia));

            string binaryText = "";
            string resultText = "";
            int cantMin = (bytesForFrecuency * 8);

            for (int i = 2 + (bytesForFrecuency + 1) * nodes.Count; i < content.Length && cantTotal > 0; i++)
            {
                binaryText += Convert.ToString(content[i], 2).PadLeft(8, '0');
                if (binaryText.Length >= cantMin)
                {
                    bool encontrado = false;
                    do
                    {
                        encontrado = false;
                        foreach (var x in nodes)
                        {
                            if (binaryText.StartsWith(x.binaryCode) && cantTotal > 0)
                            {
                                resultText += x.caracter;
                                binaryText = binaryText.Remove(0, x.binaryCode.Length);
                                encontrado = true;
                                cantTotal--;
                                break;
                            }
                        }
                    }
                    while (encontrado && cantTotal > 0);
                }
            }

            

            byte[] result = new byte[resultText.Length];

            for(int i = 0; i < resultText.Length; i++)
            {
                result[i] = (byte)resultText[i];
            }

            return result;
        }

    }
}
