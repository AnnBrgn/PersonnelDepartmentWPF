using PersonnelDepartmentAPI.Classes;
using PersonnelDepartmentAPI.ClassesDTO;
using PersonnelDepartmentWPF.Class;
using PersonnelDepartmentWPF.Classes;
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
    /// Логика взаимодействия для AccountingWorkerPages.xaml
    /// </summary>
    public partial class AccountingWorkerPages : Page, INotifyPropertyChanged
    {
        private List<WorkerDTO> listWorkers;
        public DateTime? SelectedDate { get; set; }
        public string Description { get; set; }
        public string TypeDay {  get; set; }
        public WorkerDTO? SelectedWorker { get; set; }
        

        public List<WorkerDTO> ListWorkers 
        { 
            get => listWorkers;
            set
            {
                listWorkers = value;
                OnPropertyChanged(nameof(ListWorkers));
            }
        }
        public AccountingWorkerPages()
        {
            InitializeComponent();
            DataContext = this;
            LoadWorkers();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public async void LoadWorkers()
        {
            ListWorkers = await RequestSender.GetWorkers();
        }
        public async void GetTypeDay()
        {
            var typeDay = await RequestSender.TryGetMatching(SelectedWorker.Id, (DateTime)SelectedDate);
            if (typeDay != null)
            {
                TypeDay = Constants.DATE_TYPE[typeDay.DayType];
                Description = typeDay.Description;
                OnPropertyChanged(nameof(TypeDay));
                OnPropertyChanged(nameof(Description));
            }
            else
            {
                MessageBox.Show("Информация не найдена.");
            }
        }

        private void DateSelected(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedDate == null)
                return;
            if(SelectedWorker == null) 
                return;
            GetTypeDay();
        }
    }
}
