using PersonnelDepartmentAPI.Classes;
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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public Page PageWorker { get; set; }
        public UserDTO CurrentUser { get; set; }
        public MainWindow(UserDTO? user)
        {
            InitializeComponent();
            CurrentUser = user;
            DataContext = this;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private void Browse(object sender, RoutedEventArgs e)
        {
            PageWorker = new ListWorkersPages();
            OnPropertyChanged(nameof(PageWorker));
        }

        private void AddNewWorker(object sender, RoutedEventArgs e)
        {
            AddWorker addWorker = new AddWorker();
            addWorker.Show();
        }

        private void TransitionAddPost(object sender, RoutedEventArgs e)
        {
            AddPost post = new AddPost();
            post.Show();
        }

        private void AccountWindow(object sender, RoutedEventArgs e)
        {
            PageWorker = new AccountingWorkerPages();
            OnPropertyChanged(nameof(PageWorker));
        }
    }
}