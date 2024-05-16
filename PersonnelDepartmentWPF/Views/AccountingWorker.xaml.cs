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
using System.Windows.Shapes;

namespace PersonnelDepartmentWPF.Views
{
    /// <summary>
    /// Логика взаимодействия для AccountingWorker.xaml
    /// </summary>
    public partial class AccountingWorker : Window, INotifyPropertyChanged
    {
        public int DayType { get; set; } = -1;
        public DateTime? Date { get; set; }
        public string Description {  get; set; }
        public int NumDays { get; set; }
        public Visibility DescriptionVisibility
        {
            get
            {
                if(DayType == 2||DayType==3)
                    return Visibility.Visible;
                return Visibility.Hidden;
            }
        }

        public WorkerDTO Worker { get; set; }
        public AccountingWorker(WorkerDTO worker)
        {
            Worker = worker;
            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void OnDaySelected(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ComboBox;
            if (cb.SelectedIndex >= 0)
            {
                DayType = cb.SelectedIndex;
            }
            OnPropertyChanged(nameof(DescriptionVisibility));
        }

        private void SaveDay(object sender, RoutedEventArgs e)
        {
            ProceedSaving();
        }
        private async void ProceedSaving()
        {
            switch (DayType)
            {
                case 0:
                    var day = new WorkingDTO
                    {
                        Date = Date,
                        IdWorker = Worker.Id,
                        WorkingDay = NumDays
                    };
                    if (await RequestSender.TryAddWorking(day))
                        Close();
                    break;
                case 1:
                    var dayVacation = new VacationDTO
                    {
                        Date = Date,
                        IdWorker = Worker.Id,
                        VacationDay = NumDays
                    };
                    if(await RequestSender.TryAddVacation(dayVacation))
                        Close();
                    break;
                case 2:
                    var daySick = new SickDTO
                    {
                        Date = Date,
                        IdWorker = Worker.Id,
                        SickDay = NumDays,
                        Description = Description
                    };
                    if(await RequestSender.TryAddSick(daySick))
                        Close();
                    break;
                case 3:
                    var dayOmission = new OmissionDTO
                    {
                        Date = Date,
                        IdWorker = Worker.Id,
                        OmissionDay = NumDays,
                        Description = Description
                    };
                    if(await RequestSender.TryAddOmission(dayOmission))
                        Close();
                    break;
                default:
                    MessageBox.Show("Пожалуйста, выберите день.");
                    return;

            }
            


        }
    }

}
