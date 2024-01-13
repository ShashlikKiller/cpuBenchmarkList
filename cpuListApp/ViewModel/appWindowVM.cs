using cpuListApp.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cpuListApp.ViewModel
{
    public class appWindowVM : basicVM
    {
        private List<CPU> currentCPUlist;

        public appWindowVM()
        {
            currentCPUlist = new List<CPU>();
            DataGridItem = new CPUVM();
            IsEnableHeroDetails = false;
            IsEnableDeleteHero = false;
            IsEnableParsing = true;
            ProgressBarVisibility = "Hidden";
            //ShablonPath = @"C:\Users\artem\Desktop\Архитектура ИС\privetVsem.doc";
            //SaveAsPath = @"C:\Users\artem\Desktop\Архитектура ИС\8И11 Принцев АИС Разработка БД и механизмов наполненияdocx.docx";
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

        public List<CPU> CurrentCPUlist
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

        private CPUVM dataGridItem;
        public CPUVM DataGridItem
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

        private List<CPU> GetCPUsFromDB()
        {
            List<CPU> cpus = new List<CPU>();
            using (var db = new cpuListContext())
            {
                cpus = db.CPUs.OrderBy(x => x.Rank).ToList();
            }
            return cpus;
        }

        private bool trySaveCPUsToDB(List<CPU> CPUsToSave)
        {
            try
            {
                using (var db = new cpuListContext())
                {
                    db.CPUs.AddRange(CPUsToSave);
                    db.SaveChangesAsync();
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
