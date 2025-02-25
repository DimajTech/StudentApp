using StudentApp.Models.DTO;

namespace StudentApp.Service.AdminApp
{
    public class AdminUserService
    {
        private readonly IConfiguration _configuration;
        private readonly string ADMIN_API_URL;

        public AdminUserService(IConfiguration configuration)
        {
            _configuration = configuration;
            ADMIN_API_URL = _configuration["EnvironmentVariables:ADMIN_API_URL"];
        }
        public void RegisterStudentToAdmin(InsertStudentDTO user)
        {
            user.RegistrationStatus = "pending";
            user.Role = "Student";
            
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ADMIN_API_URL);
                var postTask = client.PostAsJsonAsync("/api/user/saveUser", user);
                postTask.Wait();

                var result = postTask.Result;

                if (!result.IsSuccessStatusCode)
                {
                    throw new Exception($"Error al ingresar el estudiante. Código: {result.StatusCode}, Mensaje: {result.Content.ReadAsStringAsync().Result}");
                }


            }
        }

        public void UpdateStudent(UpdateStudentDTO user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ADMIN_API_URL);

                var response = client.PutAsJsonAsync("/api/user/updateStudent", user).Result;


                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error al actualizar el estudiante. Código: {response.StatusCode}, Mensaje: {response.Content.ReadAsStringAsync().Result}");
                }
            }
        }
    }
}
