using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    public class LZW
    {
        private Dictionary<int, string> dictionary;
        private Dictionary<string, int> dictionaryC;
        public LZW() 
        {
            dictionary = new Dictionary<int, string>();
            dictionaryC = new Dictionary<string,int> ();
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

        //public string Decompression(byte[] compressedText)
        //{
        //    int bytePerCharacter = Convert.ToInt32(compressedText[0]);
        //    int alphabethLength = Convert.ToInt32(compressedText[1]);
        //    for(int i = 0; i < alphabethLength; i++)
        //    {
        //        dictionary.Add(i, );
        //    }
        //}
        private string intToBinaryString(int numericValue, int cantBytes)
        {
            string result = "";
            for (int i = cantBytes; i >= 0; i--)
            {
                int divisor = Convert.ToInt32(Math.Pow(2, i));
                result += numericValue / divisor;
                numericValue = numericValue % divisor;
            }
            return result;
        }

        private int binaryToInt(string Btext)
        {
            int result = 0;
            for (int i = Btext.Length-1; i >= 0; i--)
            {
                result += Convert.ToInt32(Btext.Substring((Btext.Length - 1) - i,1)) * Convert.ToInt32(Math.Pow(2, i));
            }
            return result;
        }


        public byte[] Compress(string text)
        {
            string binaryCode = "";
            int cantByte = 0;
            int dictionaryCharCant = 0;
            string compressed = string.Empty;
            byte[] result = new byte[10];
            if (text != "" || text != null || text.Length != 0)
            {
                for (int i = 0; i < text.Length; i++)
                {
                    if (!dictionaryC.ContainsKey(text[i].ToString()))
                    {
                        dictionaryC.Add(text[i].ToString(), i);
                    }
                }
                dictionaryCharCant = dictionaryC.Count;
                string w = "";

                foreach (char k in text)
                {
                    string wk = w + k;
                    if (dictionaryC.ContainsKey(wk))
                    {
                        w = wk;
                    }
                    else
                    {
                        compressed += dictionaryC[w] + "-";
                        dictionaryC.Add(wk, dictionaryC.Count);
                        w = k.ToString();
                    }
                }
                if (!string.IsNullOrEmpty(w))
                {
                    compressed += dictionaryC[w];
                }
                cantByte = Convert.ToInt32(Math.Log2(dictionaryC.Count));
                string[] code = compressed.Split("-");
                for(int i = 0; i < code.Length; i++)
                {
                    binaryCode += intToBinaryString(int.Parse(code[i]), cantByte);
                }
                int cant = 8 - binaryCode.Length % 8;
                while (cant > 0)
                {
                    binaryCode += 0;
                    cant--;
                }
                result = new byte[2 + dictionaryCharCant + (binaryCode.Length / 8)];
                result[0] = byte.Parse(cantByte.ToString());
                result[1] = byte.Parse(dictionaryCharCant.ToString());
                int count = 0;
                foreach (var x in dictionaryC.Keys)
                {
                    if (count < dictionaryCharCant)
                    {
                        result[2 + count] = Convert.ToByte(char.Parse(x));
                        count++;
                    }
                    else {
                        break;
                    }
                }
                for (int i = 2 + dictionaryCharCant; i < binaryCode.Length /8; i++)
                {
                    result[i] = Convert.ToByte(binaryToInt(binaryCode.Substring(i * 8, 8)));
                }
                return result;

            }
            else {
                return result;
            };

        }

    }
}
