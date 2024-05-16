using PersonnelDepartmentAPI.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PersonnelDepartmentWPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для GeneralInformation.xaml
    /// </summary>
    public partial class GeneralInformation : Page, INotifyPropertyChanged
    {   
        public WorkerDTO Worker { get; set; }
        public GeneralInformation(WorkerDTO worker)
        {
            InitializeComponent();
            DataContext = this;
            Worker = worker;
            Worker.Load();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            MessageBox.Show(Worker.GetDirectorFIO);
        }
    }
}
