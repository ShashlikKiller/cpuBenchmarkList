﻿using AngleSharp;
using cpuListApp.Model.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static cpuListApp.Model.Backend.Patterns.CPUBuilder;

namespace cpuListApp.Model.Backend.Parser
{
    public static class Parser
    {
        private static int ExtractMhzValue(string freq)
        {
            string[] parts = freq.Split(' ');
            if (parts.Length == 2 && parts[1].Equals("Mhz", StringComparison.OrdinalIgnoreCase))
            {
                if (int.TryParse(parts[0], out int result))
                {
                    return result;
                }
            }
            return 0;
            //throw new ArgumentException("Invalid frequency input format");
        }

        private static float ExtractTDP(string tdp)
        {
            string[] parts = tdp.Split(' ');
            if (parts.Length == 2 && parts[1].Equals("W", StringComparison.OrdinalIgnoreCase))
            {
                if (float.TryParse(parts[0], out float result))
                {
                    return result;
                }
            }
            return 0;
            throw new ArgumentException("Invalid TDP input format");
        }

        private static int ExtractTechproccess(string techproccess)
        {
            string[] parts = techproccess.Split(' ');
            if (parts.Length == 2 && parts[1].Equals("nm", StringComparison.OrdinalIgnoreCase))
            {
                if (int.TryParse(parts[0], out int result))
                {
                    return result;
                }
            }
            return 0;
            throw new ArgumentException("Invalid techprocess input format");
        }
        private static float TryGetTemperature(string temp)
        {
            float result = 0;
            string[] parts = temp.Split('°');
            if (parts.Length == 2 && parts[1].Trim() == "C")
            {
                float.TryParse(parts[0], out result);
            }
            return result;
        }

        private static int GetBrandId(string name)
        {
            switch (name)
            {
                case string n when n.Contains("AMD"):
                    return 1;
                case string n when n.Contains("Intel"):
                    return 2;
                default:
                    return 3;
            }
        }

        private static List<CPU> dbCPUs()
        {
            using (var db = new cpuListContext())
            {
                return db.CPUs.ToList();
            }
        }

        private static string DeletedTabSpaceStr(string innertext)
        {
            var input = innertext.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
            return input.Last().Trim();
        }
        public static async Task<List<CPU>> Parse(int listLength)
        {
            // HTML zone
            string url = "https://www.chaynikam.info/en/cpu_table.html";
            var config = AngleSharp.Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(url);
            var processorRows = document.QuerySelectorAll("table.cpus tbody tr");
            // HTML zone

            // Builder zone
            ConcreteBuilder builder = new ConcreteBuilder();
            Director director = new Director(builder);
            int releaseDate;
            string segment;
            string socket;
            int cores, threads;
            int freqDef, freqTurbo;
            bool? multiplier;
            string arch;
            int techproccess;
            float tdp, tempLimit;
            int l1cache, l2cache, l3cache;
            bool? apu;
            string name;
            int brandId;
            int rank;
            float points;
            // Builder zone
            List<CPU> cpuList_parsed = new List<CPU>();
            List<CPU> cpuList_db = dbCPUs();
            foreach (var row in processorRows.Take(listLength))
            {
                CPU currCPU = new CPU();
                var link = row.QuerySelector("a");
                if (link != null)
                {
                    var linkUrl = link.Attributes["href"].Value;
                    var cpuUrl = document.Origin + "/" + linkUrl;
                    var newDocument = await context.OpenAsync(cpuUrl);
                    // Открываем дополнительное окошко по ссылке
                    var additionalInfoRows = newDocument.QuerySelectorAll("body div.body div div:nth-child(3) div table tbody tr").Where(x => x.ChildElementCount == 2); // Извлекаем информацию из дополнительного окошка
                    var columns = row.QuerySelectorAll("td");
                    if (!Int32.TryParse(columns[0].TextContent, out rank)) rank = cpuList_parsed.Last().Rank;
                    builder.AddRank(rank, currCPU);
                    name = columns[1].TextContent;
                    builder.AddName(name, currCPU);
                    brandId = GetBrandId(name);
                    if ((cpuList_db.Any(c => c.Name == currCPU.Name)))
                    {
                        if ((cpuList_db.Any(c => c.Rank == currCPU.Rank)))
                        {
                            continue;
                            //CurrentCPUlist.Add(cpu);
                        }
                    }

                    builder.AddBrand(brandId, currCPU);
                    if (!float.TryParse(columns[3].TextContent.Replace('.', ','), out points)) points = 0;
                    builder.AddBenchPoints(points, currCPU);
                    if (additionalInfoRows != null)
                    {
                        foreach (var item in additionalInfoRows)
                        {
                            string rowSwitcher = DeletedTabSpaceStr(item.Children[0].InnerHtml);
                            switch (rowSwitcher)
                            {
                                case "Год выхода":
                                    if (!Int32.TryParse(DeletedTabSpaceStr(item.Children[1].InnerHtml), out releaseDate)) releaseDate = 0;
                                    builder.AddReleaseDate(releaseDate, currCPU);
                                    break;
                                case "Сегмент":
                                    segment = DeletedTabSpaceStr(item.Children[1].InnerHtml);
                                    builder.AddSegment(segment, currCPU);
                                    break;
                                case "Socket":
                                    using (var db = new cpuListContext())
                                    {
                                        socket = DeletedTabSpaceStr(item.Children[1].InnerHtml);
                                        if (!db.Sockets.Any(c => c.Name == socket))
                                        {
                                            Socket newSocket = new Socket()
                                            {
                                                Name = socket
                                            };
                                            db.Sockets.Add(newSocket);
                                            await db.SaveChangesAsync();
                                        }
                                        builder.AddSocket(db.Sockets.Where(c => c.Name == socket).FirstOrDefault().Id, currCPU);
                                    }
                                    break;
                                case "Количество ядер":
                                    if (!Int32.TryParse(DeletedTabSpaceStr(item.Children[1].InnerHtml), out cores)) cores = 0;
                                    builder.AddCores(cores, currCPU);
                                    break;
                                case "Количество потоков":
                                    if (!Int32.TryParse(DeletedTabSpaceStr(item.Children[1].InnerHtml), out threads)) threads = 0;
                                    builder.AddThreads(threads, currCPU);
                                    break;
                                case "Базовая частота":
                                    freqDef = ExtractMhzValue(DeletedTabSpaceStr(item.Children[1].InnerHtml));
                                    builder.AddFreqDefault(freqDef, currCPU);
                                    break;
                                case "Turbo Core":
                                    freqTurbo = ExtractMhzValue(DeletedTabSpaceStr(item.Children[1].InnerHtml));
                                    builder.AddFreqTurbo(freqTurbo, currCPU);
                                    break;
                                case "Разблокированный множитель":
                                    switch (DeletedTabSpaceStr(item.Children[1].InnerHtml))
                                    {
                                        case "да":
                                        case "есть":
                                            multiplier = true;
                                            break;
                                        case "нет":
                                            multiplier = false;
                                            break;
                                        default:
                                            multiplier = null;
                                            break;
                                    }
                                    builder.AddMultiplier(multiplier, currCPU);
                                    break;
                                case "Архитектура (ядро)":
                                    arch = DeletedTabSpaceStr(item.Children[1].InnerHtml);
                                    builder.AddArch(arch, currCPU);
                                    break;
                                case "Техпроцесс":
                                    techproccess = ExtractTechproccess(DeletedTabSpaceStr(item.Children[1].InnerHtml));
                                    builder.AddTechproccess(techproccess, currCPU);
                                    break;
                                case "TDP":
                                    tdp = ExtractTDP(DeletedTabSpaceStr(item.Children[1].InnerHtml));
                                    builder.AddTDP(tdp, currCPU);
                                    break;
                                case "Макс. температура":
                                    tempLimit = TryGetTemperature(DeletedTabSpaceStr(item.Children[1].InnerHtml));
                                    builder.AddTempLimit(tempLimit, currCPU);
                                    break;
                                case "Кэш L1, КБ":
                                    if (!Int32.TryParse(DeletedTabSpaceStr(item.Children[1].InnerHtml), out l1cache)) l1cache = 0;
                                    builder.AddL1Cache(l1cache, currCPU);
                                    break;
                                case "Кэш L2, КБ":
                                    if (!Int32.TryParse(DeletedTabSpaceStr(item.Children[1].InnerHtml), out l2cache)) l2cache = 0;
                                    builder.AddL2Cache(l2cache, currCPU);
                                    break;
                                case "Кэш L3, КБ":
                                    if (!Int32.TryParse(DeletedTabSpaceStr(item.Children[1].InnerHtml), out l3cache)) l3cache = 0;
                                    builder.AddL3Cache(l3cache, currCPU);
                                    break;
                                case "Графический процессор":
                                case "iGPU":
                                    switch (DeletedTabSpaceStr(item.Children[1].InnerHtml))
                                    {
                                        case "да":
                                            apu = true;
                                            break;
                                        case "нет":
                                            apu = false;
                                            break;
                                        default:
                                            apu = null;
                                            break;
                                    }
                                    builder.AddAPU(apu, currCPU);
                                    break;
                                default: break;
                            }
                        }
                    }
                    cpuList_parsed.Add(currCPU);
                }
            }
            cpuList_db.AddRange(cpuList_parsed);
            cpuList_db.OrderBy(cpu => cpu.Rank);
            return cpuList_db;
        }
    }
}