using Microsoft.Win32;
using PersonnelDepartmentAPI.Classes;
using PersonnelDepartmentWPF.Class;
using PersonnelDepartmentWPF.Classes;
using PersonnelDepartmentWPF.ClassesDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
    /// Логика взаимодействия для AddWorker.xaml
    /// </summary>
    public partial class AddWorker : Window, INotifyPropertyChanged
    {
        public WorkerDTO SelectedDirector { get; set; }
        public WorkerDTO Worker { get; set; }
        public List<RoleDTO> Roles { get; set; }
        public List<PostDTO> Posts { get; set; }
        public List<WorkerDTO> AllWorkers { get; set; }

        public IEnumerable<string> GetRoles
        {
            get
            {
                if (Roles == null || Roles.Count <= 0)
                    return null;
                return Roles.Select(s => s.RoleTitle);
            }
        }

        public IEnumerable<string> GetPosts
        {
            get
            {
                if (Roles == null || Roles.Count <= 0)
                    return null;
                return Posts.Select(s => s.Title);
            }
        }

        public IEnumerable<string> GetDirectors
        {
            get
            {
                if (AllWorkers == null || AllWorkers.Count <= 0)
                    return null;
                return AllWorkers.Where(s => s != null && s.IdRole == 2).Select(s=>s.GetFIO);
            }
        }
        public AddWorker()
        {
            Worker = new WorkerDTO();
            Worker.InfoWorker = new InfoWorkerDTO();
            InitializeComponent();
            DataContext = this;
            LoadData();
        }

        private async void LoadData()
        {
            await LoadWorkers();
            await LoadRoles();
            await LoadPosts();
        }

        private async Task LoadWorkers()
        {
            AllWorkers = await RequestSender.GetWorkers();

            OnPropertyChanged(nameof(AllWorkers));
            OnPropertyChanged(nameof(GetDirectors));
        }

        private async Task LoadRoles()
        {
            Roles = await RequestSender.GetRoles();

            OnPropertyChanged(nameof(Roles));
            OnPropertyChanged(nameof(GetRoles));
        }

        private async Task LoadPosts()
        {
            Posts = await RequestSender.GetPosts();

            OnPropertyChanged(nameof(Posts));
            OnPropertyChanged(nameof(GetPosts));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void AddPhoto(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                Worker.Image = File.ReadAllBytes(op.FileName);
                OnPropertyChanged(nameof(Worker.Image));
                OnPropertyChanged(nameof(Worker));
            }
        }

        private void OnGenderSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = (sender as ComboBox);
            if(comboBox.SelectedItem != null)
                Worker.Gender = comboBox.SelectedItem.ToString();
        }

        private void OnBirthdaySelected(object sender, SelectionChangedEventArgs e)
        {
            Worker.Birthday = (sender as DatePicker).SelectedDate;
        }

        private async void SaveWorker(object sender, RoutedEventArgs e)
        {
            if (await RequestSender.TryAddWorker(Worker) != null)
            {
                Constants.OnListWorkersChanged?.Invoke();
                Close();
            }
            else
            {
                MessageBox.Show("Не удалось сохранить. Повторите попытку.");
            }
        }

        private void OnRoleSelected(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = (sender as ComboBox);
            if (comboBox.SelectedItem != null)
            {
                Worker.Role = Roles.FirstOrDefault(s=>s.RoleTitle == comboBox.SelectedItem.ToString());
                if (Worker.Role != null)
                    Worker.IdRole = Worker.Role.Id;
            }
        }

        private void OnDirectorSelected(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = (sender as ComboBox);
            if (comboBox.SelectedItem != null)
            {
                Worker.Director = AllWorkers.FirstOrDefault(s=>s.GetFIO == comboBox.SelectedItem.ToString());
                if (Worker.Director != null)
                    Worker.IdDirector = Worker.Director.Id;
            }
        }

        private void OnPostSelected(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = (sender as ComboBox);
            if (comboBox.SelectedItem != null)
            {
                Worker.Post = Posts.FirstOrDefault(s => s.Title == comboBox.SelectedItem.ToString());
                if (Worker.Post != null)
                    Worker.IdPost = Worker.Post.Id;
            }
        }
    }
}
