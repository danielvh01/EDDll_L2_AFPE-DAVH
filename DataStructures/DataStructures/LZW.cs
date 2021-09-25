using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    public class LZW
    {
        private Dictionary<string, int> dictionary;
        public LZW() 
        {
            dictionary = new Dictionary<string, int>();
        }

        private byte binaryToByte(string binaryByte)
        {
            int number = 0;
            for (int i = 0; i < 8; i++)
            {
                number += int.Parse(Math.Pow(2, 7 - i).ToString()) * int.Parse(binaryByte.Substring(i, 1));
            }
            return byte.Parse(number.ToString());
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

        public string Decompression(byte[] compressedText)
        {

        }

    }
}
