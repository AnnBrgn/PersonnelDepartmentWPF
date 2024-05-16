using PersonnelDepartmentAPI.Classes;
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
    /// Логика взаимодействия для AddPost.xaml
    /// </summary>
    public partial class AddPost : Window, INotifyPropertyChanged
    {
        private List<PostDTO> listPosts;
        public PostDTO Post { get; set; }
        public PostDTO? DeletePost { get; set; }
        public List<PostDTO> ListPosts 
        { 
            get => listPosts;
            set
            {
                listPosts = value;
                OnPropertyChanged(nameof(ListPosts));
            }
        }
        public AddPost()
        {
            InitializeComponent();
            DataContext = this;
            TryGetPosts();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private async void SavePost(object sender, RoutedEventArgs e)
        {
            if (await RequestSender.TryAddPost(Post) != null)
            {
                Constants.OnListWorkersChanged?.Invoke();
                Close();
            }
            else
            {
                MessageBox.Show("Не удалось сохранить. Повторите попытку.");
            }
        }
        private async void TryGetPosts()
        {
            ListPosts = await RequestSender.GetPosts();
        }

        private async void TryDeletePost(object sender, RoutedEventArgs e)
        {
            var answer = MessageBox.Show("Вы уверены, что хотите удалить эту должность?",
                    "Данная запись будет безвозвратно удалена",
                    MessageBoxButton.YesNo);
            if (answer != MessageBoxResult.Yes)
                return;
            await RequestSender.TryDeletePost(DeletePost);
            Constants.OnListWorkersChanged?.Invoke();
            Close();
        }
    }
}
