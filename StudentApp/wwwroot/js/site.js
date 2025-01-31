$(document).ready(() => {

    $(document).on('submit', '#contact-form', function (event) {

        event.preventDefault();

        AuthenticateUser();
    });

    $(document).on('submit', '#register-form', function (event) {

        event.preventDefault();
        Add();
    });
});

//Login
function AuthenticateUser() {
    const email = $('#email').val();
    const password = $('#password').val();

    $.ajax({
        url: "/User/Login",
        type: "POST",
        data: { email, password },
        success: function (response) {
            if (response.success) {

                window.location.href = "/";

            } else {

                $('#validation').text(response.message).css('color', '#900C3F');
            }
        },
        error: function () {

            $('#validation').text('Hubo un problema con el servidor. Intente de nuevo más tarde.').css('color', '#900C3F');
        }
    });
}

//Register
function Add() {

    var user = {

        name: $('#r-name').val(),
        email: $('#r-email').val(),
        password: $('#r-password').val(),
    };

    if (user.password != $('#confirm-password').val()) {

        $('#validation').text('Las contraseñas no coinciden');

    } else {
        $.ajax({
            url: "/User/Register",
            data: JSON.stringify(user), //converte la variable estudiante en tipo json
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (response) {


                if (response.success) {

                    $('#r-name').val('');
                    $('#r-email').val('');
                    $('#r-password').val('');
                    $('confirm-password').val('');
                    $('#validation').text(response.message);
                    $('#validation').css('color', 'green');

                } else {

                    $('#validation').text(response.message).css('color', '#900C3F');
                }


            },
            error: function (errorMessage) {
                $('#validation').text(errorMessage.message).css('color', '#900C3F');
            }
        });
    }
}
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

function GetUserData() {
    console.log("GetUserData called");
    $.ajax({
        url: "/User/GetByEmail",
        type: "GET",
        data: { email: "test3@example.com" },
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            console.log("User data retrieved:", result);
            $('#p-name').text(result.name);
            $('#p-name2').val(result.name);
            $('#p-email').val(result.email);
            $('#p-description').val(result.description);
            $('#p-linkedin').val(result.linkedIn);
            $('#p-password').val(result.password);
        },
        error: function (errorMessage) {
            console.error(errorMessage);
            alert(errorMessage.responseText);
        }
    });
}