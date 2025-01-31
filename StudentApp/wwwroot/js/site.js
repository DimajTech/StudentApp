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
    $.ajax({
        url: "/PieceOfNews/GetNews",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {


            var htmlContent = '';
            $.each(result, (key, item) => {

                htmlContent += `
    <div class="col-lg-4 col-md-6 align-self-center mb-90 event_outer col-md-6 wordpress design">
        <div class="events_item">
            <div class="thumb">
                <a href="javascript:void(0);" onclick="LoadNewsDetail('${item.id}')">
                    <img src="${item.picture}" alt="">
                </a>
                <span class="category">${item.date}</span>
            </div>
            <div class="down-content">
                <span class="author">${item.user.name}</span>
                <h5>${item.title}</h5>
                <br/>
                <h7>${item.description.length > 80 ? item.description.substring(0, item.description.lastIndexOf(' ', 80)) + "..." : item.description}</h7>
            </div>
        </div>
    </div>
`;

            });

            $("#news-container").html(htmlContent);
            $("#news-container").css('height', 'auto');
        },
        error: function (errorMessage) {
            console.log(errorMessage.responseText);
        }
    });
}
function LoadNewsDetail(pieceOfNewsID) {

    console.log(pieceOfNewsID)

    $.ajax({
        url: "/PieceOfNews/GetById/" + pieceOfNewsID,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        success: function (newsItem) {

            var detailHtml = `
                    <fieldset>
                        <button onclick="LoadNewsItems()" style="
                            border: none;
                            height: 50px;
                            font-size: 14px;
                            font-weight: 600;
                            background-color: #fff;
                            padding: 0px 25px;
                            border-radius: 25px;
                            color: #66c5e3;
                            transition: all .4s;
                            position: relative;
                            z-index: 3;">
                            ⬅ Volver</button>
                    </fieldset>
                    <img src="${newsItem.picture}" alt="" style="border-radius: 33px; padding-top:20px; padding-bottom:20px">
                    <h2>${newsItem.title}</h2>
                    <p><strong>Autor:</strong> ${newsItem.user.name} (${newsItem.user.role})</p>
                    <p><strong>Fecha:</strong> ${newsItem.date}</p>
                    <p style="text-align:justify; border-radius: 33px; padding-top:20px; padding-bottom:20px">${newsItem.description}</p>
            `;

            $("#news-container").html(detailHtml);
        },
        error: function (errorMessage) {
            console.log(errorMessage.responseText);
        }
    });
}

/*
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

*/