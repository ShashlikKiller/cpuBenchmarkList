using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace cpuParseConsole
{
    class Program
    {

        static async Task Main(string[] args)
        {
            string url = "https://www.chaynikam.info/en/cpu_table.html";
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);

            var document = await context.OpenAsync(url);
            var processorRows = document.QuerySelectorAll("table.cpus tbody tr");

            foreach (var row in processorRows)
            {
                var link = row.QuerySelector("a");
                if (link != null)
                {
                    var linkUrl = link.Attributes["href"].Value;
                    var cpuUrl = document.Origin + "/" + linkUrl;
                    var newDocument = await context.OpenAsync(cpuUrl);
                    // Открываем дополнительное окошко по ссылке
                    var additionalInfoRows = newDocument.QuerySelectorAll("body div.body div div:nth-child(3) div table tbody tr").Where(x=>x.ChildElementCount==2); // Извлекаем информацию из дополнительного окошка
                    if (additionalInfoRows != null)
                    {
                        foreach (var item in additionalInfoRows)
                        {
                            switch (item.Children[0].InnerHtml)
                            {
                                case "Год выхода":

                                    break;
                                case "Сегмент":

                                    break;
                                case "Socket":

                                    break;
                                case "Количество ядер":

                                    break;
                                case "Количество потоков":

                                    break;
                                case "Базовая частота":

                                    break;
                                case "Turbo Core":

                                    break;
                                case "Разблокированный множитель":

                                    break;
                                case "Архитектура (ядро)":

                                    break;
                                case "Техпроцесс":

                                    break;
                                case "TDP":

                                    break;
                                case "Макс. температура":

                                    break;
                                case "Кэш L1, КБ":

                                    break;
                                case "Кэш L2, КБ":

                                    break;
                                case "Кэш L3, КБ":

                                    break;
                                case "Графический процессор":

                                    break;
                            }

                        }
                    }
                    var columns = row.QuerySelectorAll("td");
                    var cpuRank = columns[0].TextContent;
                    var cpuName = columns[1].TextContent;
                    var points = columns[3].TextContent;
                    var cpuFreqDef = columns[4].TextContent;
                    var cpuFreqTurbo = columns[5].TextContent;
                    var CoresNThreads = columns[6].TextContent;


                    Console.WriteLine($"Model: {cpuName},\n" +
                        $" Clock Speed: {cpuFreqDef},\n" +
                        $" Turbo Speed: {cpuFreqTurbo}, \n" +
                        $" Cores: {CoresNThreads},\n" +
                        $" Points: {points}\n" +
                        "--------------------------------------------------");
                }
            }
        }
    }
}
