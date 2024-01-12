using cpuListApp.Model.Entities;

namespace cpuListApp.Model.Backend.Patterns
{
    public class CPUBuilder
    {
        public abstract class CPUBuild
        {
            public abstract void AddReleaseDate(uint releaseDate, CPU currCPU);
            public abstract void AddSegment(string segment, CPU currCPU);
            public abstract void AddSocket(string socket, CPU currCPU);
            public abstract void AddCores(uint cores, CPU currCPU);
            public abstract void AddThreads(uint threads, CPU currCPU);
            public abstract void AddFreqDefault(uint freqDefault, CPU currCPU);
            public abstract void AddFreqTurbo(uint freqTurbo, CPU currCPU);
            public abstract void AddMultiplier(bool? multiplier, CPU currCPU);
            public abstract void AddArch(string arch, CPU currCPU);
            public abstract void AddTechproccess(uint techproccess, CPU currCPU);
            public abstract void AddTDP(float tdp, CPU currCPU);
            public abstract void AddTempLimit(float tempLimit, CPU currCPU);
            public abstract void AddL1Cache(uint l1cache, CPU currCPU);
            public abstract void AddL2Cache(uint l2cache, CPU currCPU);
            public abstract void AddL3Cache(uint l3cache, CPU currCPU);
            public abstract void AddAPU(bool? apu, CPU currCPU);
            public abstract void AddName(string name, CPU currCPU);
            public abstract void AddBrand(string brand, CPU currCPU);
            public abstract void AddBenchPoints(float benchpoints, CPU currCPU);
            public abstract void AddRank(uint rank, CPU currCPU);
            public abstract CPU GetCPU(CPU currCPU);
        }

        public class Director
        {
            CPUBuild currentCPUBuilder;
            public Director(CPUBuild builder)
            {
                this.currentCPUBuilder = builder;
            }
            public CPU CreateCPU(uint cores, string socket, uint threads, uint freqDefault, uint freqTurbo, bool? multiplier,
                                 string arch, uint techproccess, float tdp, float tempLimit, uint l1cache,
                                 uint l2cache, uint l3cache, bool apu, string name, string brand, float benchpoints, uint rank)
            {
                CPU currentCPU = new CPU();
                currentCPUBuilder.AddCores(cores, currentCPU);
                currentCPUBuilder.AddSocket(socket, currentCPU);
                currentCPUBuilder.AddThreads(threads, currentCPU);
                currentCPUBuilder.AddFreqDefault(freqDefault, currentCPU);
                currentCPUBuilder.AddFreqTurbo(freqTurbo, currentCPU);
                currentCPUBuilder.AddMultiplier(multiplier, currentCPU);
                currentCPUBuilder.AddArch(arch, currentCPU);
                currentCPUBuilder.AddTechproccess(techproccess, currentCPU);
                currentCPUBuilder.AddTDP(tdp, currentCPU);
                currentCPUBuilder.AddTempLimit(tempLimit, currentCPU);
                currentCPUBuilder.AddL1Cache(l1cache, currentCPU);
                currentCPUBuilder.AddL2Cache(l2cache, currentCPU);
                currentCPUBuilder.AddL3Cache(l3cache, currentCPU);
                currentCPUBuilder.AddAPU(apu, currentCPU);
                currentCPUBuilder.AddName(name, currentCPU);
                currentCPUBuilder.AddBrand(brand, currentCPU);
                currentCPUBuilder.AddBenchPoints(benchpoints, currentCPU);
                currentCPUBuilder.AddRank(rank, currentCPU);
                return currentCPUBuilder.GetCPU(currentCPU);
            }
        }
        public class ConcreteBuilder : CPUBuild
        {
            public override void AddSocket(string socket, CPU currCPU)
            {
                currCPU.Socket = socket;
            }
            public override void AddRank(uint rank, CPU currCPU)
            {
                currCPU.Rank = rank;
            }
            public override void AddReleaseDate(uint releaseDate, CPU currCPU)
            {
                currCPU.ReleaseDate = releaseDate;
            }

            public override void AddSegment(string segment, CPU currCPU)
            {
                currCPU.Segment = segment;
            }

            public override void AddCores(uint cores, CPU currCPU)
            {
                currCPU.Cores = cores;
            }

            public override void AddThreads(uint threads, CPU currCPU)
            {
                currCPU.Threads = threads;
            }

            public override void AddFreqDefault(uint freqDefault, CPU currCPU)
            {
                currCPU.FreqDefault = freqDefault;
            }

            public override void AddFreqTurbo(uint freqTurbo, CPU currCPU)
            {
                currCPU.FreqTurbo = freqTurbo;
            }

            public override void AddMultiplier(bool? multiplier, CPU currCPU)
            {
                currCPU.Multiplier = multiplier;
            }

            public override void AddArch(string arch, CPU currCPU)
            {
                currCPU.Arch = arch;
            }

            public override void AddTechproccess(uint techproccess, CPU currCPU)
            {
                currCPU.Techproccess = techproccess;
            }

            public override void AddTDP(float tdp, CPU currCPU)
            {
                currCPU.TDP = tdp;
            }

            public override void AddTempLimit(float tempLimit, CPU currCPU)
            {
                currCPU.TempLimit = tempLimit;
            }

            public override void AddL1Cache(uint l1cache, CPU currCPU)
            {
                currCPU.L1cache = l1cache;
            }

            public override void AddL2Cache(uint l2cache, CPU currCPU)
            {
                currCPU.L2cache = l2cache;
            }

            public override void AddL3Cache(uint l3cache, CPU currCPU)
            {
                currCPU.L3cache = l3cache;
            }

            public override void AddAPU(bool? apu, CPU currCPU)
            {
                currCPU.APU = apu;
            }

            public override void AddName(string name, CPU currCPU)
            {
                currCPU.Name = name;
            }

            public override void AddBrand(string brand, CPU currCPU)
            {
                currCPU.Brand = brand;
            }

            public override void AddBenchPoints(float benchpoints, CPU currCPU)
            {
                currCPU.BenchPoints = benchpoints;
            }

            public override CPU GetCPU(CPU currentCPU)
            {
                return currentCPU;
            }
        }
    }
}