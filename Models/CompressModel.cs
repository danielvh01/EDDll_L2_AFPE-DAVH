using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDDll_L2_AFPE_DAVH.Models
{
    public class CompressModel : IComparable
    {
        public string originalFileName { get; set; }
        public string CompressedFileName_Route { get; set; }
        public string rateOfCompression { get; set; }
        public string compressionFactor { get; set; }
        public string reductionPercentage { get; set; }

        public int CompareTo(object obj)
        {
            var comparer = ((CompressModel)obj).originalFileName;
            return comparer.CompareTo(originalFileName);
        }

    }
}
