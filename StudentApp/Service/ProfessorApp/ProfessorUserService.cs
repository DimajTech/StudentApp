﻿using Azure;
using StudentApp.Models.DTO;
using StudentApp.Models.Entity;

namespace StudentApp.Service.ProfessorApp
{
    public class ProfessorUserService
    {
        private readonly IConfiguration _configuration;
        private readonly string PROFESSOR_API_URL;

        public ProfessorUserService(IConfiguration configuration)
        {
            _configuration = configuration;
            PROFESSOR_API_URL = _configuration["EnvironmentVariables:PROFESSOR_API_URL"];
        }

        public void UpdateStudent(UpdateStudentDTO user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{PROFESSOR_API_URL}api/User/");

                var response = client.PutAsJsonAsync("UpdateStudent/" + user.Id, user).Result;

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error al actualizar el estudiante. Código: {response.StatusCode}, Mensaje: {response.Content.ReadAsStringAsync().Result}");
                }
            }
        }

    }
}
