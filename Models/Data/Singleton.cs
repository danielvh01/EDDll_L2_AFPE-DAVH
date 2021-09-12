using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataStructures;

namespace EDDll_L1_AFPE_DAVH.Models.Data
{
    public sealed class Singleton
    {                
        private readonly static Singleton _instance = new Singleton();
        private Singleton()
        {


        }
        public static Singleton Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
