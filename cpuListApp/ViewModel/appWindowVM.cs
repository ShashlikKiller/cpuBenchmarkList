using cpuListApp.Model.Backend;
using cpuListApp.Model.Entities;
using cpuListApp.Reports;
using cpuListApp.View.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using static cpuListApp.Model.Backend.Parser.Parser;

namespace cpuListApp.ViewModel
{
    public class appWindowVM : basicVM
    {
        private ObservableCollection<CPU> currentCPUlist;

        public appWindowVM()
        {
            LoadCPUlistFromDB();
            if (CurrentCPUlist.Count > 0) CPUListVisibility = "Visible"; else CPUListVisibility = "Hidden";
            SelectedCPU = new CPU();
            OpenInfoWindowState = false;
            DeleteSelectedCPUState = false;
            ParsingState = true;
            LoadingFrameVisibility = "Hidden";
        }

        private void LoadCPUlistFromDB()
        {
            using (var db = new cpuListContext())
            {
                currentCPUlist = new ObservableCollection<CPU>(db.CPUs);
            }
        }

        private string loadingFrameVisibility;
        public string LoadingFrameVisibility
        {
            get
            {
                return loadingFrameVisibility;
            }
            set
            {
                loadingFrameVisibility = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<CPU> CurrentCPUlist
        {
            get
            {
                return currentCPUlist;
            }
            set
            {
                currentCPUlist = value;
                OnPropertyChanged();
            }
        }

        private int selectedCPUindex;
        public int SelectedCPUindex
        {
            get
            {
                return selectedCPUindex;
            }
            set
            {
                selectedCPUindex = value;
                OnPropertyChanged();
            }
        }

        private CPU selectedCPU;
        public CPU SelectedCPU
        {
            get
            {
                return selectedCPU;
            }
            set
            {
                selectedCPU = value;
                OnPropertyChanged();
                if (selectedCPU != null && selectedCPU.Rank != 0)
                {
                    DeleteSelectedCPUState = true;
                    OpenInfoWindowState = true;
                }
                else
                {
                    DeleteSelectedCPUState = false;
                    OpenInfoWindowState = false;
                }
            }
        }

        private bool isEnableDeleteHero;
        public bool DeleteSelectedCPUState
        {
            get
            {
                return openInfoWindowState;
            }
            set
            {
                openInfoWindowState = value;
                OnPropertyChanged();
            }
        }

        private bool parsingState;
        public bool ParsingState
        {
            get
            {
                return parsingState;
            }
            set
            {
                parsingState = value; OnPropertyChanged();
            }
        }

        private bool openInfoWindowState;
        public bool OpenInfoWindowState
        {
            get
            {
                return openInfoWindowState;
            }
            set
            {
                openInfoWindowState = value;
                OnPropertyChanged();
            }
        }
        private string cpuListVisibility;
        public string CPUListVisibility
        {
            get { return cpuListVisibility; }
            set { cpuListVisibility = value; OnPropertyChanged(); }
        }
        private Command startParsing;
        public Command StartParsing
        {
            get
            {
                return startParsing = new Command(async obj =>
                {
                    CPUListVisibility = "Hidden";
                    LoadingFrameVisibility = "True";
                    ParsingState = false;
                    CurrentCPUlist = new ObservableCollection<CPU>(await Parse(20));
                    ParsingState = true;
                    LoadingFrameVisibility = "False";
                    CPUListVisibility = "Visible";
                });
            }
        }
        private Command deleteData;
        public Command DeleteData
        {
            get
            {
                return deleteData = new Command(async obj =>
                {
                    using (var db = new cpuListContext())
                    {
                        db.CPUs.RemoveRange(db.CPUs);
                        CurrentCPUlist.Clear();
                        await db.SaveChangesAsync();
                        CPUListVisibility = "Hidden";
                    }
                });
            }
        }
        private Command saveCPUlist;
        public Command SaveCPUlist
        {
            get
            {
                return saveCPUlist = new Command(async obj =>
                {
                    bool? result = await trySaveCPUsToDB(CurrentCPUlist);
                });
            }
        }
        private Command openInfoWindow;
        public Command OpenInfoWindow
        {
            get
            {
                return openInfoWindow = new Command(async obj =>
                {
                    CpuInfo informationWindow = new CpuInfo();
                    informationWindow.DataContext = new CpuInfoVM(SelectedCPU);
                    informationWindow.Show();
                });
            }
        }
        private Command openSettingsWindow;
        public Command OpenSettingsWindow
        {
            get
            {
                return openSettingsWindow = new Command(async obj =>
                {
                    Settings settingsWindow = new Settings();
                    settingsWindow.DataContext = new basicVM();
                    settingsWindow.Show();
                });
            }
        }

        private Command generateReport;
        public Command GenerateReport
        {
            get
            {
                return generateReport ??= new Command(obj =>
                {
                    ReportGenerator.GetInstance().GenerateReport(ShablonPath, SavePath);
                });
            }
        }

        private async Task<bool?> trySaveCPUsToDB(ObservableCollection<CPU> CPUsToSave)
        {
            try
            {
                using (var db = new cpuListContext())
                {
                    foreach (var cpu in db.CPUs)
                    {
                        if (CPUsToSave.Any(c => c.Rank == cpu.Rank && c.Name == cpu.Name))
                        {
                            db.CPUs.RemoveRange(db.CPUs);
                        }
                        else return null;
                    }
                    db.CPUs.AddRange(CPUsToSave);
                    await db.SaveChangesAsync();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
