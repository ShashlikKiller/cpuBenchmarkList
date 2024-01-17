using cpuListApp.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using Microsoft.Office.Interop.Excel;

namespace cpuListApp.Reports
{
    public class ReportGenerator
    {
        private static ReportGenerator instance;
        public static ReportGenerator GetInstance()
        {
            instance = new ReportGenerator();
            return instance;
        }

        public void GenerateReport(object ShablonFile, object SaveAsFile)
        {
            //открываем excel
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Add();
            Excel.Worksheet worksheet = workbook.Sheets.Add();

            //массивы параметров героев
            CPU[] CPUs = GetCPUs();

            if (CPUs == null)
            {
                MessageBox.Show("Сначала заполните БД!");
                return;
            }
            string[] cpuNames = CPUs.Select(x => x.Name).ToArray();
            int[] cpuPoints = (CPUs.Select(x => Convert.ToInt32(x.BenchPoints * 100)).ToArray());
            int[] cpuBrands = CPUs.Select(x => x.BrandId).ToArray();
            int[] cpuClockspeed = CPUs.Select(x => x.FreqTurbo).ToArray();
            int[] cpuThreads = CPUs.Select(x => x.Threads).ToArray();
            int[] cpuCores = CPUs.Select(x => x.Cores).ToArray();

            CreateChart(worksheet, cpuNames, cpuThreads, "Кол-во потоков", Excel.XlChartType.xlLineMarkers, 1);
            CreateChart(worksheet, cpuNames, cpuPoints, "Очки производительности", Excel.XlChartType.xlColumnClustered, 2);
            CreateChart(worksheet, cpuNames, cpuBrands, "Брэнд", Excel.XlChartType.xlLineMarkers, 3);
            CreateChart(worksheet, cpuNames, cpuClockspeed, "Скорость ядра (базовая)", Excel.XlChartType.xlBarClustered, 4);
            CreateChart(worksheet, cpuNames, cpuCores, "Кол-во ядер", Excel.XlChartType.xlLineMarkers, 5);
            string excelFilePath = @"graphics.xlsx";
            workbook.SaveAs(excelFilePath);
            workbook.Close();
            excelApp.Quit();

            //открываем word
            Microsoft.Office.Interop.Word.Application wordApp = new();

            // Открываем документ
            Document wDoc = wordApp.Documents.Add(ref ShablonFile, false, Microsoft.Office.Interop.Word.WdNewDocumentType.wdNewBlankDocument, true);


            Microsoft.Office.Interop.Excel.Workbook excelbook = excelApp.Workbooks.Open(excelFilePath);
            Microsoft.Office.Interop.Excel.ChartObjects chartObjects = excelbook.Sheets["Лист2"].ChartObjects();
            foreach (ChartObject item in chartObjects)
            {
                Microsoft.Office.Interop.Word.Range range1 = wDoc.Content.Paragraphs.Last.Range;
                item.Copy();
                range1.Paste();
                wDoc.Content.Paragraphs.Add();
                range1 = wDoc.Content.Paragraphs.Last.Range;
            }
            try
            {
                wDoc.SaveAs2(SaveAsFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            // Закрываем приложение 
            wordApp.Quit(Microsoft.Office.Interop.Word.WdSaveOptions.wdPromptToSaveChanges);

        }

        private CPU[] GetCPUs()
        {
            using (var db = new cpuListContext())
            {
                CPU[] cpus = db.CPUs.OrderBy(x => x.Rank).ToArray();
                foreach (CPU cpu in cpus)
                {
                    cpus.Append(cpu);
                }
                return cpus;
            }
        }
        private static void CreateChart(Microsoft.Office.Interop.Excel.Worksheet worksheet, string[] xValues, int[] yValues, string chartTitle, Excel.XlChartType chartType, int switcher)
        {
            Microsoft.Office.Interop.Excel.ChartObjects chartObjects = worksheet.ChartObjects();
            Excel.ChartObject chartObject = chartObjects.Add(0, 0, 900, 300);
            Excel.Chart chart = chartObject.Chart;
            Excel.Range range = worksheet.Range[$"A1:B{xValues.Length}"];
            switch (switcher)
            {
                case 1:
                    range = worksheet.Range[$"A1:B{xValues.Length}"];
                    break;
                case 2:
                    range = worksheet.Range[$"C1:D{xValues.Length}"];
                    break;
                case 3:
                    range = worksheet.Range[$"F1:G{xValues.Length}"];
                    break;
                case 4:
                    range = worksheet.Range[$"L1:O{xValues.Length}"];
                    break;
                case 5:
                    range = worksheet.Range[$"M1:N{xValues.Length}"];
                    break;
            }

            chart.ChartType = chartType;
            range.Value = new object[xValues.Length, 2];
            for (int i = 0; i < xValues.Length; i++)
            {
                range.Cells[i + 1, 1].Value = xValues[i];
                range.Cells[i + 1, 2].Value = yValues[i];
            }
            chart.SetSourceData(range);
            chart.HasTitle = true;
            chart.ChartTitle.Text = chartTitle;
        }

        //private static void InsertChartIntoWord(Microsoft.Office.Interop.Word.Document wordDoc, string excelFilePath, string chartTitle, float left, float top)
        //{
        //    Microsoft.Office.Interop.Word.Paragraph paragraph = wordDoc.Content.Paragraphs.Add();
        //    Microsoft.Office.Interop.Word.InlineShape inlineShape = paragraph.Range.InlineShapes.AddOLEObject(
        //        ClassType: "Excel.Chart.12",
        //        FileName: excelFilePath,
        //        LinkToFile: false,
        //        DisplayAsIcon: false,
        //        IconFileName: ""
        //    );
        //    paragraph.Range.InsertParagraphAfter();
        //}
    }
}
