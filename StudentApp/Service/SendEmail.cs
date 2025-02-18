using StudentApp.Models.Entity;

namespace StudentApp.Service
{
    public static class SendEmail
    {
        static Email email = new Email();
        public static void RegisterEmail(User user)
        {
            using (var client = new HttpClient())
            {

                email.ToUser = user.Email;
                email.Subject = "Su cuenta ha sido registrada con éxito";
                email.Content = "<html lang='es'>\n<head>\n    <meta charset='UTF-8'>\n    <meta name='viewport' content='width=device-width, initial-scale=1.0'>\n    <title>Email de Bienvenida</title>\n    <style>\n        body { font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px; }\n        .container { max-width: 600px; background: #ffffff; padding: 20px; border-radius: 8px; box-shadow: 0px 0px 10px rgba(0,0,0,0.1); text-align: center; }\n        .logo { width: 300px; margin-bottom: 20px; }\n        h1 { color: #00693e; }\n        p { color: #333; font-size: 16px; }\n        .footer { margin-top: 20px; font-size: 12px; color: #666; }\n    </style>\n</head>\n<body>\n    <div class='container'>\n        <img src='https://blogger.googleusercontent.com/img/b/R29vZ2xl/AVvXsEh5057rSlx1GVQGuaLOV0oMvYZnSlUUm5szF7HaA_sSFdbVuAQ-oSBKMjBHjjX2YSZtBaIhY6ccW7jGKp3j_mi-eYL58Sz2oS3ZRDMY0V1bzOEaRUsnUsMqEGvaT7-zcwqIxGdkmEi-0DsO/w1200-h630-p-k-no-nu/UCR+logo+transparente.png' alt='Logo UCR' class='logo'>\n        <h1>¡Bienvenido a la Universidad de Costa Rica!</h1>\n        <p>Hola, <strong>" + user.Name + "</strong></p>\n        <p>Gracias por registrarse en nuestro sitio web. Actualmente estamos procesando su solicitud, y su cuenta estará activa una vez que sea validada por el administrador.</p>\n        <p class='footer'>Si tiene alguna duda, no dude en ponerte en contacto con nosotros.</p>\n    </div>\n</body>\n</html>"
;
                client.BaseAddress = new Uri("http://localhost:5092/api/SendEmail/");

                var postTask = client.PostAsJsonAsync("SendEmail", email);
                postTask.Wait();

            }
        }

        public static void AppointmentEmail(string professorEmail)
        {
            using (var client = new HttpClient())
            {

                email.ToUser = professorEmail;
                email.Subject = "Solicitud de horas consulta";
                email.Content = "<html lang='es'>\n<head>\n    <meta charset='UTF-8'>\n    <meta name='viewport' content='width=device-width, initial-scale=1.0'>\n    <title>Email de Bienvenida</title>\n    <style>\n        body { font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px; }\n        .container { max-width: 600px; background: #ffffff; padding: 20px; border-radius: 8px; box-shadow: 0px 0px 10px rgba(0,0,0,0.1); text-align: center; }\n        .logo { width: 300px; margin-bottom: 20px; }\n        h1 { color: #00693e; }\n        p { color: #333; font-size: 16px; }\n        .footer { margin-top: 20px; font-size: 12px; color: #666; }\n    </style>\n</head>\n<body>\n    <div class='container'>\n        <img src='https://blogger.googleusercontent.com/img/b/R29vZ2xl/AVvXsEh5057rSlx1GVQGuaLOV0oMvYZnSlUUm5szF7HaA_sSFdbVuAQ-oSBKMjBHjjX2YSZtBaIhY6ccW7jGKp3j_mi-eYL58Sz2oS3ZRDMY0V1bzOEaRUsnUsMqEGvaT7-zcwqIxGdkmEi-0DsO/w1200-h630-p-k-no-nu/UCR+logo+transparente.png' alt='Logo UCR' class='logo'>\n        <h1>¡Solicitud de Horas Consulta!</h1>\n       <p>Ha recibido una nueva solicitud de horas consulta, en la aplicación podrá aceptarla o rechazarla.</p>\n        <p class='footer'>Si tiene alguna duda, no dude en ponerte en contacto con nosotros.</p>\n    </div>\n</body>\n</html>"
;
                client.BaseAddress = new Uri("http://localhost:5092/api/SendEmail/");

                var postTask = client.PostAsJsonAsync("SendEmail", email);
                postTask.Wait();

            }
        }

    }
}
