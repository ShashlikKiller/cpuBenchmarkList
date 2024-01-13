using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cpuListApp.Model.Entities
{
    public class CPUVM
    {
        public uint ReleaseDate { get; set; }

        public string Segment { get; set; }
        public string Socket { get; set; }

        public uint Cores { get; set; }

        public uint Threads { get; set; }

        public uint FreqDefault { get; set; }

        public uint FreqTurbo { get; set; }

        public bool? Multiplier { get; set; }

        public string Arch { get; set; }

        public uint Techproccess { get; set; }

        public float TDP { get; set; }

        public float TempLimit { get; set; }

        public uint L1cache { get; set; }

        public uint L2cache { get; set; }

        public uint L3cache { get; set; }

        public bool? APU { get; set; }

        public string Name { get; set; }

        public string Brand { get; set; }

        public float BenchPoints { get; set; }
    }
}
