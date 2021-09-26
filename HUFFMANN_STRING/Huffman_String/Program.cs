using System;
using System.IO;
using System.Text;
using DataStructures;

namespace Huffman_String
{
    class Program
    {
        
        static void Main(string[] args)
        {
            LZW compresor = new LZW();
            string text;
            do
            {
                Console.WriteLine("Escriba el mensaje que desea comprimir:");
                text = Console.ReadLine();
                if(text.Length == 0)
                {
                    Console.WriteLine("Error, el texto esta vacio por favor intentelo de nuevo");
                }                
            }
            while (text.Length == 0);
            Console.WriteLine("El mensaje comprimido es el siguiente:");
            byte[] compress = compresor.Compress(text);
            Console.WriteLine(Encoding.UTF8.GetString(compress));
            Console.ReadLine();
        }
    }
}
