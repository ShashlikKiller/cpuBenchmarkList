
namespace cpuListApp.Model.Entities
{
    public class CPU
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
        public uint Rank { get; set; }

        public CPU()
        { }

        public CPU (uint releaseDate, string segment, string socket, uint cores, uint threads, uint freqDefault, uint freqTurbo, bool? multiplier,
            string arch, uint techproccess, float tDP, float tempLimit, uint l1cache, uint l2cache, uint l3cache, bool? aPU, string name,
            string brand, float benchPoints, uint rank)
        {
            ReleaseDate = releaseDate;
            Segment = segment;
            Socket = socket;
            Cores = cores;
            Threads = threads;
            FreqDefault = freqDefault;
            FreqTurbo = freqTurbo;
            Multiplier = multiplier;
            Arch = arch;
            Techproccess = techproccess;
            TDP = tDP;
            TempLimit = tempLimit;
            L1cache = l1cache;
            L2cache = l2cache;
            L3cache = l3cache;
            APU = aPU;
            Name = name;
            Brand = brand;
            BenchPoints = benchPoints;
            Rank = rank;
        }
    }
}
