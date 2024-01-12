﻿using AngleSharp;
using cpuListApp.Model.Entities;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static cpuListApp.Model.Backend.Patterns.CPUBuilder;

namespace cpuListApp.Model.Backend.Parser
{
    public class Parser
    {
        private static uint ExtractMhzValue(string freq)
        {
            string[] parts = freq.Split(' ');
            if (parts.Length == 2 && parts[1].Equals("Mhz", StringComparison.OrdinalIgnoreCase))
            {
                if (uint.TryParse(parts[0], out uint result))
                {
                    return result;
                }
                else return 0;
            }
            return 0;
            throw new ArgumentException("Invalid frequency input format");
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
                else return 0;
            }
            return 0;
            throw new ArgumentException("Invalid TDP input format");
        }

        private static uint ExtractTechproccess(string techproccess)
        {
            string[] parts = techproccess.Split(' ');
            if (parts.Length == 2 && parts[1].Equals("nm", StringComparison.OrdinalIgnoreCase))
            {
                if (uint.TryParse(parts[0], out uint result))
                {
                    return result;
                }
                else return 0;
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

        public static string GetBrand(string name)
        {
            switch (name)
            {
                case string n when n.Contains("AMD"):
                    return "AMD";
                case string n when n.Contains("Intel"):
                    return "Intel";
                default:
                    return "N/A";
            }
        }

        public static async Task Parse()
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
            uint releaseDate;
            string segment;
            string socket;
            uint cores, threads;
            uint freqDef, freqTurbo;
            bool? multiplier;
            string arch;
            uint techproccess;
            float tdp, tempLimit;
            uint l1cache, l2cache, l3cache;
            bool? apu;
            string name, brand;
            uint rank;
            float points;
            // Builder zone
            foreach (var row in processorRows)
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
                    if (additionalInfoRows != null)
                    {
                        foreach (var item in additionalInfoRows)
                        {
                            switch (item.Children[0].InnerHtml)
                            {
                                case "Год выхода":
                                    if (!UInt32.TryParse(item.Children[1].InnerHtml, out releaseDate)) releaseDate = 0;
                                    builder.AddReleaseDate(releaseDate, currCPU);
                                    break;
                                case "Сегмент":
                                    segment = item.Children[1].InnerHtml;
                                    builder.AddSegment(segment, currCPU);
                                    break;
                                case "Socket":
                                    socket = item.Children[1].InnerHtml;
                                    builder.AddSocket(socket, currCPU);
                                    break;
                                case "Количество ядер":
                                    if (!UInt32.TryParse(item.Children[1].InnerHtml, out cores)) cores = 0;
                                    builder.AddCores(cores,currCPU);
                                    break;
                                case "Количество потоков":
                                    if (!UInt32.TryParse(item.Children[1].InnerHtml, out threads)) threads = 0;
                                    builder.AddThreads(threads, currCPU);
                                    break;
                                case "Базовая частота":
                                    freqDef = ExtractMhzValue(item.Children[1].InnerHtml);
                                    builder.AddFreqDefault(freqDef, currCPU);
                                    break;
                                case "Turbo Core":
                                    freqTurbo = ExtractMhzValue(item.Children[1].InnerHtml);
                                    builder.AddFreqTurbo(freqTurbo, currCPU);
                                    break;
                                case "Разблокированный множитель":
                                    switch (item.Children[1].InnerHtml)
                                    {
                                        case "да":
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
                                    arch = item.Children[1].InnerHtml;
                                    builder.AddArch(arch, currCPU);
                                    break;
                                case "Техпроцесс":
                                    techproccess = ExtractTechproccess(item.Children[1].InnerHtml);
                                    builder.AddTechproccess(techproccess, currCPU);
                                    break;
                                case "TDP":
                                    tdp = ExtractTDP(item.Children[1].InnerHtml);
                                    builder.AddTDP(tdp, currCPU);
                                    break;
                                case "Макс. температура":
                                    tempLimit = TryGetTemperature(item.Children[1].InnerHtml);
                                    builder.AddTempLimit(tempLimit, currCPU);
                                    break;
                                case "Кэш L1, КБ":
                                    if (!UInt32.TryParse(item.Children[1].InnerHtml, out l1cache)) l1cache = 0;
                                    builder.AddL1Cache(l1cache, currCPU);
                                    break;
                                case "Кэш L2, КБ":
                                    if (!UInt32.TryParse(item.Children[1].InnerHtml, out l2cache)) l2cache = 0;
                                    builder.AddL2Cache(l2cache, currCPU);
                                    break;
                                case "Кэш L3, КБ":
                                    if (!UInt32.TryParse(item.Children[1].InnerHtml, out l3cache)) l3cache = 0;
                                    builder.AddL3Cache(l3cache, currCPU);
                                    break;
                                case "Графический процессор":
                                    switch (item.Children[1].InnerHtml)
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
                    var columns = row.QuerySelectorAll("td");
                    if (!UInt32.TryParse(columns[0].TextContent, out rank)) rank = 0;
                    builder.AddRank(rank, currCPU);
                    name = columns[1].TextContent;
                    builder.AddName(name, currCPU);
                    brand = GetBrand(name);
                    builder.AddBrand(brand, currCPU);
                    if (!float.TryParse(columns[3].TextContent, out points)) points = 0;
                    builder.AddBenchPoints(points, currCPU);
                }
            }
        }
    }
}