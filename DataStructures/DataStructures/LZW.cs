using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    public class LZW
    {
        private Dictionary<int, string> dictionary;
        public LZW() 
        {
            dictionary = new Dictionary<int, string>();
        }

        private byte binaryToByte(string binaryByte)
        {
            int number = 0;
            int cantBits = binaryByte.Length;
            for (int i = cantBits; i >= 0; i++)
            {
                number += int.Parse(Math.Pow(2, i).ToString()) * int.Parse(binaryByte.Substring(0, 1));
                binaryByte = binaryByte.Remove(0, 1);
            }
            return byte.Parse(number.ToString());
        }
        private int binaryToInt(string binaryByte)
        {
            int number = 0;
            int cantBits = binaryByte.Length;
            for (int i = cantBits; i >= 0; i++)
            {
                number += int.Parse(Math.Pow(2, i).ToString()) * int.Parse(binaryByte.Substring(0, 1));
                binaryByte = binaryByte.Remove(0, 1);
            }
            return number;
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
            return result;
        }
        
        public string Decompression(byte[] compressedText)
        {
            string result = "";
            int bytesPerCharacter = Convert.ToInt32(compressedText[0]);
            int alphabethLength = Convert.ToInt32(compressedText[1]);
            for(int i = 0; i < alphabethLength; i++)
            {
                dictionary.Add(i, Convert.ToString(compressedText[2 + i]));
            }
            string binaryText = "";
            for(int i = 2 + alphabethLength; i < compressedText.Length; i++)
            {
                binaryText += byteToBinaryString(compressedText[i]);
            }
            int previous;
            int current;
            string chain = "";
            string character = "";

            previous = binaryToInt(binaryText.Substring(0, bytesPerCharacter));
            character = dictionary[Convert.ToInt32(previous)];
            result += character;
            for(int i = 1; i < binaryText.Length/bytesPerCharacter; i++)
            {
                current = binaryToInt(binaryText.Substring(i*bytesPerCharacter, bytesPerCharacter));
                if(!dictionary.ContainsKey(current))
                {
                    chain = dictionary[Convert.ToInt32(previous)];
                    chain = chain + character;
                }
                else
                {
                    chain = dictionary[Convert.ToInt32(current)];
                }
                result += chain;
                character = chain.Substring(0,1);
                dictionary.Add(dictionary.Count, dictionary[Convert.ToInt32(previous)] + character);
                previous = current;
            }
            return result;

        }

    }
}
