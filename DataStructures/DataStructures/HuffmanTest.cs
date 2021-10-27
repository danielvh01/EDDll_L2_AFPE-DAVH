using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataStructures
{
    public class HuffmanTest
    {
        const int MAX_TREE_HT = 100;

        Dictionary<byte, string> codigos = new Dictionary<byte, string>();

        HuffmanHeap heap;
        HuffmanHeapNode root;

        bool isLeaf(HuffmanHeapNode root)
        {
            return (root.left == null) && (root.right == null);
        }

        HuffmanHeap crearHeap(byte[] datos, int[] frecuencias, int length)
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

        HuffmanHeapNode buildTree(byte[] datos, int[] frecuencias, int lenght)
        {
            HuffmanHeapNode left, right, top;

            HuffmanHeap heap = crearHeap(datos, frecuencias, lenght);

            while (!heap.lenghtUno())
            {
                left = heap.extractMin();
                right = heap.extractMin();

                top = new HuffmanHeapNode((byte)'$', left.frecuencia + right.frecuencia);

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
            if (isLeaf(root))
            {
                root.binaryCode = "";
                for (int i = 0; i < top; i++)
                {
                    root.binaryCode += arr[i].ToString();
                }
                codigos.Add((byte)root.caracter, root.binaryCode);
            }
        }

        void HuffmanCodes(byte[] datos, int[] frecuencias, int size)
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
                var temp = dictionary.Find(x => x.caracter.CompareTo(content[i]));
                if (temp != null)
                {
                    temp.frecuencia++;
                    if (temp.frecuencia > frecuenciaMayor)
                    {
                        frecuenciaMayor = temp.frecuencia;
                    }
                }
                else
                {
                    dictionary.InsertAtEnd(new HuffmanHeapNode(content[i], 1));
                }
            }

            byte[] caracteres = new byte[dictionary.Length];
            int[] frecuencias = new int[dictionary.Length];

            int mayor = 0;
            int menor = 0;
            for (int i = 0; i < dictionary.Length; i++)
            {
                var temp = dictionary.Get(i);
                caracteres[i] = temp.caracter;
                frecuencias[i] = temp.frecuencia;
                if (temp.frecuencia < frecuencias[menor])
                {
                    menor = i;
                }
                else if (frecuencias[mayor] < temp.frecuencia)
                {
                    mayor = i;
                }
            }

            Array.Sort(frecuencias, caracteres);
            Array.Reverse(frecuencias);
            Array.Reverse(caracteres);

            HuffmanCodes(caracteres, frecuencias, dictionary.Length);

            int bytesForFrecuencys = (int)Math.Round(Math.Log(frecuenciaMayor) / Math.Log(256), 0, MidpointRounding.ToPositiveInfinity);

            string textInBinary = "";

            string textResult = "";

            foreach (byte letra in content)
            {
                textInBinary += codigos[letra];
                if (textInBinary.Length >= 8)
                {
                    textResult += (char)Convert.ToInt32(textInBinary.Substring(0, 8), 2);
                    textInBinary = textInBinary.Remove(0, 8);
                }
            }

            if (textInBinary.Length > 0)
            {
                while (textInBinary.Length < 8)
                {
                    textInBinary += 0;
                }
                textResult += (char)Convert.ToInt32(textInBinary.Substring(0, 8), 2);
                textInBinary = textInBinary.Remove(0, 8);
            }

            byte[] result = new byte[2 + (bytesForFrecuencys + 1) * codigos.Count + textResult.Length];

            result[0] = (byte)codigos.Count;
            result[1] = (byte)bytesForFrecuencys;

            for (int i = 0; i < frecuencias.Length; i++)
            {
                result[2 + i * (bytesForFrecuencys + 1)] = caracteres[i];
                int numero = frecuencias[i];
                for (int j = bytesForFrecuencys - 1; j >= 0; j--)
                {
                    result[3 + i * (bytesForFrecuencys + 1) + j] = (byte)(numero / Math.Pow(256, j));
                }
            }

            for (int i = 0; i < textResult.Length; i++)
            {
                result[2 + (bytesForFrecuencys + 1) * codigos.Count + i] = (byte)textResult[i];
            }

            return result;
        }

        public byte[] Decompress(byte[] content)
        {
            int CantCaracteres = content[0];
            if(CantCaracteres == 0)
            {
                CantCaracteres = 256;
            }
            int bytesForFrecuency = content[1];

            byte[] caracteres = new byte[CantCaracteres];
            int[] frecuencias = new int[CantCaracteres];

            int cantTotal = 0;

            for (int i = 0; i < CantCaracteres; i++)
            {
                caracteres[i] = content[2 + i * (bytesForFrecuency + 1)];
                for (int j = 0; j < bytesForFrecuency; j++)
                {
                    frecuencias[i] += (short)(content[3 + i * (bytesForFrecuency + 1) + j] * Math.Pow(256, j));
                }
                cantTotal += frecuencias[i];
            }

            HuffmanCodes(caracteres, frecuencias, CantCaracteres);

            string binaryText = "";
            int cont = 0;
            byte[] result = new byte[cantTotal];
            int bitsIntercambiados = 0;
            var current = root;

            for (int i = 2 + (bytesForFrecuency + 1) * CantCaracteres; i < content.Length && cont < cantTotal; i++)
            {
                binaryText += Convert.ToString(content[i], 2).PadLeft(8, '0');
                current = root;
                for (int j = 0; j < binaryText.Length && cont < cantTotal; j++)
                {
                    if (binaryText[j] == '0')
                    {
                        current = current.left;
                    }
                    else
                    {
                        current = current.right;
                    }
                    if(isLeaf(current))
                    {
                        result[cont] = current.caracter;
                        cont++;
                        bitsIntercambiados += current.binaryCode.Length;
                        current = root;
                    }
                }
                binaryText = binaryText.Remove(0, bitsIntercambiados);
                bitsIntercambiados = 0;
            }
            
            return result;
        }

    }
}
