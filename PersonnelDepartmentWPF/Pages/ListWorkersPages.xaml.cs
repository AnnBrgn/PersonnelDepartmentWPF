using PersonnelDepartmentAPI.Classes;
using PersonnelDepartmentWPF.Class;
using PersonnelDepartmentWPF.Classes;
using PersonnelDepartmentWPF.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для ListWorkersPages.xaml
    /// </summary>
    public partial class ListWorkersPages : Page, INotifyPropertyChanged
    {
        public int Number { get; set; }

        private List<WorkerDTO> workers;

        public List<WorkerDTO> Workers 
        { 
            get => workers; 
            set 
            { 
                workers = value; OnPropertyChanged(nameof(Workers)); 
            } 
        } 
        public event PropertyChangedEventHandler? PropertyChanged;

        public ListWorkersPages()
        {
            InitializeComponent();
            DataContext = this;
            LoadWorkers();
            Constants.OnListWorkersChanged += LoadWorkers;
        }

        ~ListWorkersPages(){
            Constants.OnListWorkersChanged -= LoadWorkers;
        }

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public async void LoadWorkers()
        {
            Workers = await RequestSender.GetWorkers();
            foreach (var worker in Workers)
            {
                worker.LoadPost();
            }
        }

        private void Worker(object sender, SelectionChangedEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item == null)
                return;

            new EmployCardWorker(item as WorkerDTO).Show();
            (sender as ListView).SelectedIndex = -1;
        }
    }
}
