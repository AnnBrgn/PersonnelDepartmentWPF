using PersonnelDepartmentAPI.Classes;
using PersonnelDepartmentAPI.ClassesDTO;
using PersonnelDepartmentWPF.ClassesDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace PersonnelDepartmentWPF.Classes
{
    public static class RequestSender
    {
        static HttpClient client
        {
            get
            {
                return _client ?? (_client = new HttpClient()
                {
                    BaseAddress = new Uri("https://localhost:7029/api/"),
                });
            }
        }
        static HttpClient _client;

        public static async Task<UserDTO?> TryLoginAsync(string login, string password)
        {
            using StringContent jsonContent = new(
            JsonSerializer.Serialize(new LoginDTO
            {
                Login = login,
                Password = password
            }),
            Encoding.UTF8,
            "application/json");


            using HttpResponseMessage response = await client.PostAsync("Auth",jsonContent);

            if (response.StatusCode == HttpStatusCode.OK)
                return DeserializeDefaultPolicy<UserDTO>(await response.Content.ReadAsStringAsync());
            else
                return null;
        }

        public static async Task<List<WorkerDTO>> GetDirectors()
        {
            using HttpResponseMessage response = await client.GetAsync("AddWorker/GetDirectors");
            return DeserializeDefaultPolicy<List<WorkerDTO>>(await response.Content.ReadAsStringAsync());
        }

        public static async Task<List<WorkerDTO>> GetWorkers()
        {
            using HttpResponseMessage response = await client.GetAsync("AddWorker/GetWorkers");
            return DeserializeDefaultPolicy<List<WorkerDTO>>(await response.Content.ReadAsStringAsync());
        }
        /// 
        public static async Task<List<PostDTO>> GetPosts()
        {
            using HttpResponseMessage response = await client.GetAsync("AddPost/GetPosts");
            return DeserializeDefaultPolicy<List<PostDTO>>(await response.Content.ReadAsStringAsync());
        }

        private static T DeserializeDefaultPolicy<T>(string data)
        {
            return JsonSerializer.Deserialize<T>(data, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
        public static async Task<bool> TryEditWorker(WorkerDTO workerDTO)
        {
            using StringContent jsonContent = new(
            JsonSerializer.Serialize(workerDTO),
            Encoding.UTF8,
            "application/json");

            using HttpResponseMessage response = await client.PostAsync("AddWorker/EditWorker", jsonContent);
            return response.StatusCode == HttpStatusCode.OK;
        }

        public static async Task<bool> TryEditInfoWorker(InfoWorkerDTO infoWorkerDTO)
        {
            using StringContent jsonContent = new(
            JsonSerializer.Serialize(infoWorkerDTO),
            Encoding.UTF8,
            "application/json");

            using HttpResponseMessage response = await client.PostAsync("AddWorker/EditInfoWorker", jsonContent);
            return response.StatusCode == HttpStatusCode.OK;
        }

        internal static async Task<WorkerDTO> TryAddWorker(WorkerDTO worker)
        {
            if (worker.Director !=null)
                worker.IdDirector = worker.Director.Id;

            if (worker.Role != null)
                worker.IdRole = worker.Role.Id;

            using StringContent jsonContent = new(
                JsonSerializer.Serialize(worker, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = null
                }),
            Encoding.UTF8,
            "application/json");
            
            using HttpResponseMessage response = await client.PostAsync("AddWorker/AddWorker", jsonContent);
            response.EnsureSuccessStatusCode();

            var infoWorker = worker.InfoWorker;

            worker = DeserializeDefaultPolicy<WorkerDTO>(await response.Content.ReadAsStringAsync());

            if (infoWorker != null)
            {
                infoWorker.IdWorker = worker.Id;
                infoWorker = await AddInfoWorker(infoWorker);
                worker.InfoWorker = infoWorker;
            }

            return worker;
        }

        private static async Task<InfoWorkerDTO> AddInfoWorker(InfoWorkerDTO infoWorker)
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(infoWorker, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = null
                    }),
            Encoding.UTF8,
            "application/json");

            using HttpResponseMessage response = await client.PostAsync("AddInfoWorker/AddWorkerInfo", jsonContent);
            response.EnsureSuccessStatusCode();
            return DeserializeDefaultPolicy<InfoWorkerDTO>(await response.Content.ReadAsStringAsync());

        }

        internal static async Task<List<RoleDTO>> GetRoles()
        {
            using HttpResponseMessage response = await client.GetAsync("Roles/GetRoles");
            return DeserializeDefaultPolicy<List<RoleDTO>>(await response.Content.ReadAsStringAsync());
        }

        internal static async Task<List<PostDTO>> TryGetPosts()
        {
            using HttpResponseMessage response = await client.GetAsync("AddPost/GetPosts");
            return DeserializeDefaultPolicy<List<PostDTO>>(await response.Content.ReadAsStringAsync());
        }

        internal static async Task<WorkerDTO> GetWorker(int? id)
        {
            if (id == null)
                return null;
            using HttpResponseMessage response = await client.GetAsync("AddWorker/GetWorker?id=" + id);
            return DeserializeDefaultPolicy<WorkerDTO>(await response.Content.ReadAsStringAsync());
        }

        internal static async Task<RoleDTO> GetRole(int? id)
        {
            if (id == null)
                return null;
            using HttpResponseMessage response = await client.GetAsync("Roles/GetRole?id=" + id);
            return DeserializeDefaultPolicy<RoleDTO>(await response.Content.ReadAsStringAsync());

        }

        internal static async Task<PostDTO> GetPost(int? id)
        {
            if (id == null)
                return null;
            using HttpResponseMessage response = await client.GetAsync("AddPost/GetPost?id=" + id);
            return DeserializeDefaultPolicy<PostDTO>(await response.Content.ReadAsStringAsync());
        }

        internal static async Task<byte[]> GetImage(int? id)
        {
            if (id == null)
                return null;
            using HttpResponseMessage response = await client.GetAsync("AddWorker/GetImage?id=" + id);
            if(response.StatusCode==HttpStatusCode.OK)
                return DeserializeDefaultPolicy<byte[]>(await response.Content.ReadAsStringAsync());
            return null;
        }

        internal static async Task DeleteWorker(WorkerDTO worker)
        {
            if (worker == null)
                return;

            using HttpResponseMessage response = await client.PostAsync("AddWorker/DeleteWorker?id=" + worker.Id, null);
            response.EnsureSuccessStatusCode();
        }

        internal static async Task<InfoWorkerDTO> GetInfoWorker(int? id)
        {
            if (id == null)
                return null;
            using HttpResponseMessage response = await client.GetAsync("AddInfoWorker/GetWorkerInfo?id=" + id);
            if (response.StatusCode == HttpStatusCode.OK)
                return DeserializeDefaultPolicy<InfoWorkerDTO>(await response.Content.ReadAsStringAsync());
            return null;

        }

        internal static async Task<bool> TryAddWorking(WorkingDTO working)
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(working, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = null
                }),
            Encoding.UTF8,
            "application/json");

            using HttpResponseMessage response = await client.PostAsync("AddDay/AddWorkingDay", jsonContent);
            response.EnsureSuccessStatusCode();

            return response.IsSuccessStatusCode;
        }
        internal static async Task<bool> TryAddOmission(OmissionDTO omission)
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(omission, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = null
                }),
            Encoding.UTF8,
            "application/json");

            using HttpResponseMessage response = await client.PostAsync("AddDay/AddOmissionDay", jsonContent);
            response.EnsureSuccessStatusCode();

            return response.IsSuccessStatusCode;
        }
        internal static async Task<bool> TryAddVacation(VacationDTO vacation)
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(vacation, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = null
                }),
            Encoding.UTF8,
            "application/json");

            using HttpResponseMessage response = await client.PostAsync("AddDay/AddVacationDay", jsonContent);
            response.EnsureSuccessStatusCode();

            return response.IsSuccessStatusCode;
        }
        internal static async Task<bool> TryAddSick(SickDTO sick)
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(sick, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = null
                }),
            Encoding.UTF8,
            "application/json");

            using HttpResponseMessage response = await client.PostAsync("AddDay/AddSickDay", jsonContent);
            response.EnsureSuccessStatusCode();

            return response.IsSuccessStatusCode;
        }
        public static async Task<PostDTO> TryAddPost(PostDTO post)
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(post, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = null
                }),
            Encoding.UTF8,
            "application/json");

            using HttpResponseMessage response = await client.PostAsync("AddPost/AddWorkerPost", jsonContent);
            response.EnsureSuccessStatusCode();
            return post;
        }
        
        public static async Task<WorkerMatchingResultDTO> TryGetMatching(int workerId, DateTime date)
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(new WorkerDateMatchingDTO
                {
                    Date = date,
                    IdWorker = workerId
                }, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = null
                }),
            Encoding.UTF8,
            "application/json");

            using HttpResponseMessage response = await client.PostAsync("AddInfoWorker/GetMatchingWorker", jsonContent);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return DeserializeDefaultPolicy<WorkerMatchingResultDTO>(await response.Content.ReadAsStringAsync());
            }
            return null;
        }
        public static async Task TryDeletePost(PostDTO postDTO)
        {
            if (postDTO == null)
                return;

            using HttpResponseMessage response = await client.PostAsync("AddPost/DeletePost?id=" + postDTO.Id, null);
            response.EnsureSuccessStatusCode();
        }
    }
}
