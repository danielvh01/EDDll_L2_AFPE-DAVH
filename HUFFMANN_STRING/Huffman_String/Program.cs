using System;
using System.IO;
using DataStructures;

namespace Huffman_String
{
    class Program
    {
        static void Main(string[] args)
        {
            Huffman compresor = new Huffman();
            Console.WriteLine("Escriba el mensaje que desea comprimir:");
            string text = Console.ReadLine();
            Console.WriteLine("El mensaje comprimido es el siguiente:");
            byte[] compress = compresor.Compress(text);
            string result = string.Join('\0', compress);
            MemoryStream memoryStream = new MemoryStream(compress);
            StreamReader streamReader = new StreamReader(memoryStream);
            Console.WriteLine(result);
        }
    }
}
