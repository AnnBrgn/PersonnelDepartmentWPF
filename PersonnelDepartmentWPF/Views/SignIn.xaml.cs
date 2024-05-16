using PersonnelDepartmentWPF.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Логика взаимодействия для SignIn.xaml
    /// </summary>
    public partial class SignIn : Window
    {
        public string Login {  get; set; }
        public string Password { get; set; }

        public bool CanLogin { get; set; } = true;
        public SignIn()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Enter(object sender, RoutedEventArgs e)
        {
            EnterAsync();
            CanLogin = false;
        }

        private async void EnterAsync()
        {
            if (!CanLogin)
                return;
            var user = await RequestSender.TryLoginAsync(Login, Password);
            if(user != null)
            {
                MessageBox.Show("Приветствуем!"+" "+user.Name);
                MainWindow mainWindow = new MainWindow(user);
                mainWindow.Show();
                Close();
            }
            else
                MessageBox.Show("Неверный логин или пароль");
            CanLogin = true;
        }
    }
}
