using cpuListApp.Model.Backend;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace cpuListApp.ViewModel
{
    public class basicVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public basicVM()
        {
            ShablonPath = @"C:\Users\rogers\source\repos\cpuBenchmarkList\cpuListApp\Reports\shablon.doc";
            SavePath = @"C:\Users\rogers\source\repos\cpuBenchmarkList\cpuListApp\Reports\newreport.docx";
        }

        private string shablonPath;
        public string ShablonPath
        {
            get { return shablonPath; }
            set { shablonPath = value;
                OnPropertyChanged(); }
        }

        private string savePath;
        public string SavePath
        {
            get { return savePath; }
            set { savePath = value; OnPropertyChanged(); }
        }

        private Command applyPath;
        public Command ApplyPath
        {
            get
            {
                return applyPath ??= new Command(obj =>
                {
                    if (File.Exists(ShablonPath) && File.Exists(SavePath)
                        && (Path.GetExtension(ShablonPath) == ".docx" || Path.GetExtension(ShablonPath) == ".doc")
                        && (Path.GetExtension(SavePath) == ".docx" || Path.GetExtension(SavePath) == ".doc"))
                    {
                        MessageBox.Show("Пути изменены.");
                    }
                    else
                    {
                        MessageBox.Show("Пути введены неверно! (Требуемый формат: doc или docx!)");
                    }
                });
            }
        }
    }
}
