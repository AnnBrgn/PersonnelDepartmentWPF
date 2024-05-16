using PersonnelDepartmentWPF.Classes;
using PersonnelDepartmentWPF.ClassesDTO;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media.Imaging;

namespace PersonnelDepartmentAPI.Classes
{
    public class WorkerDTO : INotifyPropertyChanged
    {
        [JsonInclude]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Patronymic { get; set; }
        public DateTime? Birthday { get; set; }
        public int? IdPost { get; set; }
        public int? IdRole { get; set; }
        public byte[]? Image { get; set; }
        public string? Gender { get; set; }
        public string? FamilyStatus { get; set; }
        public int? IdDirector { get; set; }
        [JsonIgnore]
        public InfoWorkerDTO InfoWorker { get; set; }
        [JsonIgnore]
        public RoleDTO Role { get; set; }

        [JsonIgnore]
        public WorkerDTO Director { get; set; }

        [JsonIgnore]
        public PostDTO Post { get; set; }

        [JsonIgnore]
        public BitmapImage ImageConverted
        {
            get
            {
                if (Image == null)
                    return null;
                using (var ms = new System.IO.MemoryStream(Image))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad; // here
                    image.StreamSource = ms;
                    image.EndInit();
                    return image;
                }
            }
        }

        [JsonIgnore]
        public string? GetDirectorFIO
        {
            get
            {
                if (Director != null)
                {
                    return Director.GetFIO;
                }
                return null;
            }
        }
        [JsonIgnore]
        public string? GetFIO 
        { 
            get 
            {
                if(LastName==null|| Name == null) return string.Empty;
                return $"{LastName} {Name} {Patronymic}";
            } 
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async void Load()
        {
            Director = await RequestSender.GetWorker(IdDirector);
            Role = await RequestSender.GetRole(IdRole);
            Post = await RequestSender.GetPost(IdPost);
            Image = await RequestSender.GetImage(Id);
            InfoWorker = await RequestSender.GetInfoWorker(Id);

            OnPropertyChanged(nameof(Director));
            
            OnPropertyChanged(nameof(InfoWorker));

            OnPropertyChanged(nameof(GetDirectorFIO));
            OnPropertyChanged(nameof(GetFIO));

            OnPropertyChanged(nameof(Role));
            
            OnPropertyChanged(nameof(Post));
            
            OnPropertyChanged(nameof(ImageConverted));
        }

        public async void LoadPost()
        {
            Post = await RequestSender.GetPost(IdPost);
            OnPropertyChanged(nameof(Post));
        }
    }

}
