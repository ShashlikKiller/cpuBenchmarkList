using System.Windows;

namespace cpuListApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new ViewModel.appWindowVM();
        }
    }
}
