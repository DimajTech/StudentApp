

$(document).ready(() => {
    $('#contact-form').submit(function (event) {

        event.preventDefault(); 

        AuthenticateUser();
        
        return false;
    });
})

function LoadNewsItems() {
    const mockData = [
        {
            href: "#",
            imageUrl: "images/course-01.jpg",
            category: "Webdesign",
            author: "David Hutson",
            title: "Full Stack Developer",
            price: "$240"
        },
        {
            href: "#",
            imageUrl: "images/course-02.jpg",
            category: "Development",
            author: "Cindy Walker",
            title: "Web Development Tips",
            price: "$150"
        },
        {
            href: "#",
            imageUrl: "images/course-03.jpg",
            category: "WordPress",
            author: "Stella Blair",
            title: "Latest Web Trends",
            price: "$200"
        },
        {
            href: "#",
            imageUrl: "images/course-01.jpg",
            category: "Webdesign",
            author: "David Hutson",
            title: "Full Stack Developer",
            price: "$240"
        },
        {
            href: "#",
            imageUrl: "images/course-02.jpg",
            category: "Development",
            author: "Cindy Walker",
            title: "Web Development Tips",
            price: "$150"
        },
        {
            href: "#",
            imageUrl: "images/course-03.jpg",
            category: "WordPress",
            author: "Stella Blair",
            title: "Latest Web Trends",
            price: "$200"
        },
        {
            href: "#",
            imageUrl: "images/course-01.jpg",
            category: "Webdesign",
            author: "David Hutson",
            title: "Full Stack Developer",
            price: "$240"
        },
        {
            href: "#",
            imageUrl: "images/course-02.jpg",
            category: "Development",
            author: "Cindy Walker",
            title: "Web Development Tips",
            price: "$150"
        },
        {
            href: "#",
            imageUrl: "images/course-03.jpg",
            category: "WordPress",
            author: "Stella Blair",
            title: "Latest Web Trends",
            price: "$200"
        }
    ];

    let htmlContent = "";

    mockData.forEach(item => {
        htmlContent += `
            <div class="col-lg-4 col-md-6 align-self-center mb-90 event_outer col-md-6 wordpress design">
                <div class="events_item">
                    <div class="thumb">
                        <a href="${item.href}"><img src="${item.imageUrl}" alt=""></a>
                        <span class="category">${item.category}</span>
                    </div>
                    <div class="down-content">
                        <span class="author">${item.author}</span>
                        <h4>${item.title}</h4>
                    </div>
                </div>
            </div>
        `;
    });

    // Inserta el contenido en el contenedor de noticias
    $("#news-container").html(htmlContent);

    // Recalcular el tamaño del contenedor
    $("#news-container").css('height', 'auto');
}
function AuthenticateUser() {

    // Obtener valores de los inputs
    const email = $('#email').val();
    const password = $('#password').val();

    console.log(email, password); // Verificar los valores en la consola

    $.ajax({
        url: "/User/Login",
        type: "POST",
        data: { email, password }, // Enviar como form-urlencoded
        success: function () {
            // Redirigir al usuario después del login exitoso
            window.location.href = "/";
        },
        error: function () {
            // Mostrar mensaje de error
            $('#validation').text('Credenciales inválidas. Intente de nuevo.');
        }
    });
}

