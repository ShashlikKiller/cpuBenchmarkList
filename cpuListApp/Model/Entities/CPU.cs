
using System.ComponentModel.DataAnnotations;

namespace cpuListApp.Model.Entities
{
    public class CPU
    {
        public int Id { get; set; }
        public uint ReleaseDate { get; set; }

        public string Segment { get; set; }
        //public string Socket { get; set; }

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

        //public string Brand { get; set; }

        public float BenchPoints { get; set; }
        public int Rank { get; set; }
        public int SocketId { get; set; }
        public Socket Socket { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        public CPU()
        { }

        public CPU (int id, uint releaseDate, string segment, uint cores, uint threads, uint freqDefault, uint freqTurbo, bool? multiplier,
            string arch, uint techproccess, float tDP, float tempLimit, uint l1cache, uint l2cache, uint l3cache, bool? aPU, string name,
            float benchPoints, int rank, int socketId, int brandId)
        {
            Id = id;
            ReleaseDate = releaseDate;
            Segment = segment;
            SocketId = socketId;
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
            BenchPoints = benchPoints;
            Rank = rank;
            BrandId = brandId;
        }
    }
}
