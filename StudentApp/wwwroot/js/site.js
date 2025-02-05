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

//------------------------------------------------
//---------LOGIN & REGISTER SECTION---------------
//------------------------------------------------
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

//------------------------------------------------
//--------------APPOINTMENTS SECTION--------------
//------------------------------------------------
function GetAppointments() {

    //TODO: Obtener usuario por cookie

    $.ajax({
        url: "/Appointment/GetAllAppointmentsByUser",
        type: "GET",
        data: {
            email: 'test4@example.com'
        },
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var htmlTable = '';
            $.each(result, function (key, item) {
                htmlTable += '<tr>';
                htmlTable += '<td>' + item.date + '</td>';
                htmlTable += '<td>' + item.mode + '</td>';
                htmlTable += '<td>' + item.status + '</td>';
                htmlTable += '<td>' + item.course.name + '</td>';
                htmlTable += '<td>' + item.professorComment + '</td>';
                htmlTable += '</tr>';
            });
            $('#myappointments-tbody').html(htmlTable); //shows table on screen


        },
        error: function (errorMessage) {
            
            alert(errorMessage.responseText);
        }
    });
}

//------------------------------------------------
//--------------PROFILE SECTION-------------------
//------------------------------------------------
function GetUserData() {
    console.log("GetUserData called");

    //TODO: Obtener usuario por cookie

    $.ajax({
        url: "/User/GetByEmail",
        type: "GET",
        data: { email: "test3@example.com" },
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            console.log("User data retrieved:", result);
            $('#p-id').val(result.id);
            $('#p-name').text(result.name);
            $('#p-name2').val(result.name);
            $('#p-email').val(result.email);
            $('#p-description').val(result.description);
            $('#p-linkedin').val(result.linkedIn);
            $('#p-password').val(result.password);

            if (result.picture) {
                $('#p-picture').attr('src', result.picture);
            }
        },
        error: function (errorMessage) {
            console.error(errorMessage);
        }
    });
}

function HandleEditing() {
    if ($('#p-button').text() === 'Editar') {
        AllowFieldEditing();
    } else if ($('#p-button').text() === 'Confirmar cambios') {
        EditUser();
    }
}

function AllowFieldEditing() {
    var originalValues = {
        id: $('#p-id').val(),
        name: $('#p-name2').val(),
        email: $('#p-email').val(),
        password: $('#p-password').val(),
        description: $('#p-description').val(),
        linkedIn: $('#p-linkedin').val(),
        picture: $('#p-picture').val()
    };

    console.log('Original values', originalValues);

    $('#p-name2').prop("readonly", false);
    $('#p-email').prop("readonly", false);
    $('#p-password').prop("readonly", false);
    $('#p-description').prop("readonly", false);
    $('#p-linkedin').prop("readonly", false);

    $('#p-button').text("Confirmar cambios");
    $('#p-cancel-button').prop("hidden", false);
}

function EditUser() {
    var newValues = {
        id: $('#p-id').val(),
        name: $('#p-name2').val(),
        email: $('#p-email').val(),
        password: $('#p-password').val(),
        description: $('#p-description').val(),
        linkedIn: $('#p-linkedin').val()
    }

    console.log('New Values', newValues);

    $.ajax({
        url: "/User/UpdateUser",
        type: "PUT",
        data: JSON.stringify(newValues),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            GetUserData();

            $('#p-name2').prop("readonly", true);
            $('#p-email').prop("readonly", true);
            $('#p-password').prop("readonly", true);
            $('#p-description').prop("readonly", true);
            $('#p-linkedin').prop("readonly", true);

            $('#p-button').text("Editar");
            $('#p-cancel-button').prop("hidden", true);
        },
        error: function (errorMessage) {
            console.error(errorMessage);
        }
    });
    
}

function CancelEditing() {
    GetUserData();

    $('#p-name2').prop("readonly", true);
    $('#p-email').prop("readonly", true);
    $('#p-password').prop("readonly", true);
    $('#p-description').prop("readonly", true);
    $('#p-linkedin').prop("readonly", true);

    $('#p-button').text("Editar");
    $('#p-cancel-button').prop("hidden", true);
}

//------------------------------------------------
//--------------NEWS SECTION----------------------
//------------------------------------------------
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

            try {
                $("#news-container").html(htmlContent);
                $("#news-container").css('height', 'auto');
            } catch (error) {
                console.error("Error al cargar las noticias:", error);
            }
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