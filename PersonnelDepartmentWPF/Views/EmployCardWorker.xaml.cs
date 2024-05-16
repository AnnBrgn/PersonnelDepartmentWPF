using PersonnelDepartmentAPI.Classes;
using PersonnelDepartmentWPF.Class;
using PersonnelDepartmentWPF.Classes;
using PersonnelDepartmentWPF.Pages;
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
using System.Windows.Shapes;

namespace PersonnelDepartmentWPF.Views
{
    /// <summary>
    /// Логика взаимодействия для EmployCardWorker.xaml
    /// </summary>
    public partial class EmployCardWorker : Window, INotifyPropertyChanged
    {
        public Page PageWorker {  get; set; }
        private WorkerDTO _worker;
        public EmployCardWorker(WorkerDTO worker)
        {
            InitializeComponent();
            DataContext = this;
            _worker = worker;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private void GenInfo(object sender, RoutedEventArgs e)
        {
            PageWorker = new GeneralInformation(_worker);
            OnPropertyChanged(nameof(PageWorker));
        }
        private void EditWorker(object sender, RoutedEventArgs e)
        {
            PageWorker = new EditWorkerPages(_worker);
            OnPropertyChanged(nameof(PageWorker));
        }

        private async void DeleteWorker(object sender, RoutedEventArgs e)
        {
            var answer = MessageBox.Show("Вы уверены, что хотите удалить анкету?", 
                "Данная запись будет безвозвратно удалена", 
                MessageBoxButton.YesNo);
            if (answer != MessageBoxResult.Yes)
                return;
            await RequestSender.DeleteWorker(_worker);
            Constants.OnListWorkersChanged?.Invoke();
            Close();
        }

        private void Accounting(object sender, RoutedEventArgs e)
        {
            AccountingWorker accountingWorker = new AccountingWorker(_worker);
            accountingWorker.Show();
        }
    }
}
