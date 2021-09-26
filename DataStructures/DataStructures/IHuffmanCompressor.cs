using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    public interface IHuffmanCompressor
    {
        byte[] Compress(string text);
        string Decompress(byte[] compressText);
    }
}
