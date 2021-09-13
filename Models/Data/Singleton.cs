using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataStructures;
using EDDll_L2_AFPE_DAVH.Models;
using System.Text.Json;

namespace EDDll_L2_AFPE_DAVH.Models.Data
{
    public sealed class Singleton
    {                
        private readonly static Singleton _instance = new Singleton();
        public string TCompressions = "";
        public DoubleLinkedList<CompressModel> compressions;
        private Singleton()
        {
            compressions = new DoubleLinkedList<CompressModel>();
        }
        public static Singleton Instance
        {
            get
            {
                return _instance;
            }
        }

        public string getCompressions()
        {
            for (int i = 0; i < compressions.Length; i++)
            {
                TCompressions += JsonSerializer.Serialize(compressions.Get(i));
            }
            return TCompressions;
        }
    }
}
