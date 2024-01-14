
using System.ComponentModel.DataAnnotations;

namespace cpuListApp.Model.Entities
{
    public class CPU
    {
        private int releasedate, cores, threads, freqDefault, freqTurbo, techproccess, l1cache, l2cache, l3cache;
        public int Id { get; set; }
        public int ReleaseDate { get => releasedate; set => releasedate = value > 0 ? value : 0; }

        public string Segment { get; set; }
        //public string Socket { get; set; }

        public int Cores { get => cores; set => cores = value > 0 ? value : 0; }

        public int Threads { get => threads; set => threads = value > 0 ? value : 0; }

        public int FreqDefault { get => freqDefault; set => freqDefault = value > 0 ? value : 0; }

        public int FreqTurbo { get => freqTurbo; set => freqTurbo = value > 0 ? value : 0; }

        public bool? Multiplier { get; set; }

        public string Arch { get; set; }

        public int Techproccess { get => techproccess; set => techproccess = value > 0 ? value : 0; }

        public float TDP { get; set; }

        public float TempLimit { get; set; }

        public int L1cache { get => l1cache; set => l1cache = value > 0 ? value : 0; }

        public int L2cache { get => l2cache; set => l2cache = value > 0 ? value : 0; }

        public int L3cache { get => l3cache; set => l3cache = value > 0 ? value : 0; }

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

        public CPU (int id, int releaseDate, string segment, int cores, int threads, int freqDefault, int freqTurbo, bool? multiplier,
            string arch, int techproccess, float tDP, float tempLimit, int l1cache, int l2cache, int l3cache, bool? aPU, string name,
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
