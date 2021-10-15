using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    public class LZW : ILZWCompressor
    {
        private Dictionary<int, string> dictionary;
        private Dictionary<string, int> dictionaryC;
        public LZW() 
        {
            dictionary = new Dictionary<int, string>();
            dictionaryC = new Dictionary<string, int>();
        }

        
        private int binaryToInt(string Btext)
        {
            int result = 0;
            for (int i = Btext.Length - 1; i >= 0; i--)
            {
                result += Convert.ToInt32(Btext.Substring((Btext.Length - 1) - i, 1)) * (int)(Math.Pow(2, i));
            }
            return result;
        }

        public byte[] Compress(byte[] text)
        {
            string binaryCode = "";
            int cantByte = 0;
            int dictionaryCharCant = 0;
            string compressed = string.Empty;
            byte[] result = new byte[0];
            
            if (text != null || text.Length != 0)
            {
                for (int i = 0; i < text.Length; i++)
                {
                    if (!dictionaryC.ContainsKey(((char)text[i]).ToString()))
                    {
                        dictionaryC.Add(((char)text[i]).ToString(), dictionaryC.Count + 1);
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
                        dictionaryC.Add(wk, dictionaryC.Count + 1);
                        w = k.ToString();
                    }
                }
                if (!string.IsNullOrEmpty(w))
                {
                    compressed += dictionaryC[w];
                }
                cantByte = (int)Math.Round(Math.Log2(dictionaryC.Count), MidpointRounding.ToPositiveInfinity);
                string[] code = compressed.Split("-");
                for (int i = 0; i < code.Length; i++)
                {
                    binaryCode += Convert.ToString(Convert.ToInt32(code[i]),2).PadLeft(cantByte,'0');
                }
                int cant = 8 - binaryCode.Length % 8;
                while (cant > 0)
                {
                    binaryCode += 0;
                    cant--;
                }
                result = new byte[2 + dictionaryCharCant + (binaryCode.Length / 8)];
                result[0] = (byte)cantByte;
                result[1] = (byte)(dictionaryCharCant-1);
                int count = 0;
                foreach (var x in dictionaryC.Keys)
                {
                    if (count < dictionaryCharCant)
                    {
                        result[2 + count] = (byte)(x[0]);
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }
                for (int i = 0; i < binaryCode.Length / 8; i++)
                {
                    result[i + 2 + dictionaryCharCant] = (byte)binaryToInt(binaryCode.Substring(i * 8, 8));
                }
                return result;

            }
            else
            {
                return result;
            };

        }
        public byte[] Decompression(byte[] compressedText)
        {
            string result = "";
            int bytesPerCharacter = compressedText[0];
            int alphabethLength = (compressedText[1]) + 1;
            
            for (int i = 0; i < alphabethLength; i++)
            {
                dictionary.Add(i + 1, ((char)(compressedText[2 + i])).ToString());
            }
            string binaryText = "";
            for(int i = 2 + alphabethLength; i < compressedText.Length; i++)
            {
                string x = Convert.ToString(compressedText[i], 2);
                binaryText += x.PadLeft(8, '0');
            }
            int previous;
            int current;
            string chain = "";
            string character;

            previous = binaryToInt(binaryText.Substring(0, bytesPerCharacter));
            character = dictionary[previous];
            result += character;
            for (int i = 1; i < binaryText.Length/bytesPerCharacter; i++)
            {
                current = binaryToInt(binaryText.Substring(i*bytesPerCharacter, bytesPerCharacter));
                if (current!= 0)
                {
                    if (!dictionary.ContainsKey(current))
                    {
                        chain = dictionary[previous];
                        chain = chain + character;
                    }
                    else
                    {
                        chain = dictionary[current];
                    }
                    result += chain;
                    character = chain.Substring(0,1);
                    dictionary.Add(dictionary.Count + 1, dictionary[previous] + character);
                    previous = current; 
                }
                else
                {
                    break;
                }
            }
            byte[] textDecompressed = new byte[result.Length];
            for (int i = 0; i < result.Length; i++)
            {
                textDecompressed[i] = (byte)result[i];
            }
            return textDecompressed;

        }

    }
}
