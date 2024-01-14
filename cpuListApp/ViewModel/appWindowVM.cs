using cpuListApp.Model.Backend;
using cpuListApp.Model.Entities;
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
            GridVisibility = "Hidden";
            LoadCPUlistFromDB();
            if (CurrentCPUlist.Count > 0) GridVisibility = "Visible";
            DataGridItem = new CPU();
            IsEnableHeroDetails = false;
            IsEnableDeleteHero = false;
            IsEnableParsing = true;
            ProgressBarVisibility = "Hidden";
            //ShablonPath = @"C:\Users\artem\Desktop\Архитектура ИС\privetVsem.doc";
            //SaveAsPath = @"C:\Users\artem\Desktop\Архитектура ИС\8И11 Принцев АИС Разработка БД и механизмов наполненияdocx.docx";
        }

        private void LoadCPUlistFromDB()
        {
            using (var db = new cpuListContext())
            {
                currentCPUlist = new ObservableCollection<CPU>(db.CPUs);
            }
        }

        private string progressBarVisibility;
        public string ProgressBarVisibility
        {
            get
            {
                return progressBarVisibility;
            }
            set
            {
                progressBarVisibility = value;
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

        private int dataGridIndex;
        public int DataGridIndex
        {
            get
            {
                return dataGridIndex;
            }
            set
            {
                dataGridIndex = value;
                OnPropertyChanged();
            }
        }

        private CPU dataGridItem;
        public CPU DataGridItem
        {
            get
            {
                return dataGridItem;
            }
            set
            {
                dataGridItem = value;
                OnPropertyChanged();
                if (dataGridItem != null)
                {
                    IsEnableDeleteHero = true;
                    IsEnableHeroDetails = true;
                }
                else
                {
                    IsEnableDeleteHero = false;
                    IsEnableHeroDetails = false;
                }
            }
        }

        private bool isEnableDeleteHero;
        public bool IsEnableDeleteHero
        {
            get
            {
                return isEnableHeroDetails;
            }
            set
            {
                isEnableHeroDetails = value;
                OnPropertyChanged();
            }
        }

        private bool isEnableParsing;
        public bool IsEnableParsing
        {
            get
            {
                return isEnableParsing;
            }
            set
            {
                isEnableParsing = value; OnPropertyChanged();
            }
        }

        private bool isEnableHeroDetails;
        public bool IsEnableHeroDetails
        {
            get
            {
                return isEnableHeroDetails;
            }
            set
            {
                isEnableHeroDetails = value;
                OnPropertyChanged();
            }
        }
        private string gridVisibility;
        public string GridVisibility
        {
            get { return gridVisibility; }
            set { gridVisibility = value; OnPropertyChanged(); }
        }
        private Command startParsing;
        public Command StartParsing
        {
            get
            {
                return startParsing = new Command(async obj =>
                {
                    GridVisibility = "Hidden";
                    ProgressBarVisibility = "True";
                    IsEnableParsing = false;
                    List<CPU> parsedCPUlist = await Parse(25);
                    using (var db = new cpuListContext())
                    {
                        foreach (var cpu in parsedCPUlist)
                        {

                            if (!(db.CPUs.Any(c => c.Name == cpu.Name)))
                            {
                                if (!(db.CPUs.Any(c => c.Rank == cpu.Rank)))
                                {
                                    CurrentCPUlist.Add(cpu);
                                }
                            }
                        }
                    }
                    IsEnableParsing = true;
                    ProgressBarVisibility = "False";
                    GridVisibility = "Visible";
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
                    trySaveCPUsToDB(CurrentCPUlist);
                });
            }
        }

        private bool trySaveCPUsToDB(ObservableCollection<CPU> CPUsToSave)
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
                        else return false;
                    }
                    db.CPUs.AddRange(CPUsToSave);
                    db.SaveChanges();
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
