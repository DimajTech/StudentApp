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
                htmlTable += '<td>' + (item.mode == '1' ? 'Virtual' : 'Presencial') + '</td>';
                let statusText;
                switch (item.status) {
                    case 'pending':
                        statusText = 'Pendiente';
                        break;
                    case 'approved':
                        statusText = 'Aprobada';
                        break;
                    case 'rejected':
                        statusText = 'Rechazada';
                        break;
                    default:
                        statusText = item.status; // Mostrar el valor original si no coincide con ninguno de los anteriores
                }
                htmlTable += '<td>' + statusText + '</td>'; htmlTable += '<td>' + item.course.name + '</td>';
                htmlTable += '<td>' + (item.professorComment == null ? 'Sin comentarios' : item.professorComment) + '</td>';
                htmlTable += '</tr>';
            });
            $('#myappointments-tbody').html(htmlTable); //shows table on screen


        },
        error: function (errorMessage) {
            configureToastr();
            toastr.error(errorMessage.responseText);
        }
    });
}
function GetCourses() {
    $.ajax({
        url: "/Course/GetAllCourses",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var htmlSelect = '';
            $.each(result, function (key, item) {
                htmlSelect += '<option value="' + item.id + '">' + item.name + '</option>';
            });
            $("#course").append(htmlSelect);

        },
        error: function (errorMessage) {
            configureToastr();
            toastr.error(errorMessage.responseText);
        }
    });

}

function AddAppointment() {

    var appointment = {
        date: $('#datetime').val(),
        mode: $('#mode').val(),
        courseid: $('#course').val(),
        userid: 'a4163958-6f5a-4e0a-bd31-4f27abefa04d',
    };
    var course = {
        id: $('#course').val(),
        name: $('#course').find('option:selected').text(),
    };
    var user = {
        id: 'a4163958-6f5a-4e0a-bd31-4f27abefa04d',
        name: 'Pruebaaaaaaaa',
    }
    appointment.course = course;
    appointment.user = user;
    if (course.name == 'Seleccione un curso') {

    } else {
        $.ajax({
            url: "/Appointment/CreateNewAppointment",
            data: JSON.stringify(appointment),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#datetime').val('');
                $("#course").val(0);
                $("#mode").val(0);
                configureToastr();

                toastr.success('Registrado con éxito');
                GetAppointments();
            },
            error: function (errorMessage) {
                toastr.error(errorMessage.responseText);
            }
        });
    }

};


//------------------------------------------------
//---------ADVISEMENT SECTION---------------
//------------------------------------------------

function GetAdvisementsByUser(email) {
    $.ajax({
        url: "/Advisement/GetAdvisementsByUser",
        type: "GET",
        data: { email: email },
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

            var userHtmlTable = '';
            $.each(result, function (key, item) {
                userHtmlTable += '<tr>';
                userHtmlTable += '<td>' + item.course.code + '</td>';
                userHtmlTable += '<td>' + item.user.name + '</td>';
                userHtmlTable += '<td>' + new Date(item.createdAt).toLocaleDateString() + '</td>';
                userHtmlTable += '<td><button class="btn btn-info" onclick="GetAdvisementDetails(' + item.id + ')">Ver más</button></td>';
                userHtmlTable += '</tr>';
            });

            $('#user-advisements').html(userHtmlTable);
        },
        error: function (errorMessage) {
            console.error("Error en GetAdvisementsByUser:", errorMessage.responseText);
        }
    });
}

function GetPublicAdvisements() {
    $.ajax({
        url: "/Advisement/GetPublicAdvisements",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            console.log("Datos de consultas públicas recibidos:", result); // Verificación en consola

            var publicHtmlTable = '';
            $.each(result, function (key, item) {
                publicHtmlTable += '<tr>';
                publicHtmlTable += '<td>' + item.course.code + '</td>';
                publicHtmlTable += '<td>' + item.user.name + '</td>';
                publicHtmlTable += '<td>' + new Date(item.createdAt).toLocaleDateString() + '</td>';
                publicHtmlTable += '<td><button class="btn btn-info" onclick="GetAdvisementDetails(' + item.id + ')">Ver más</button></td>';
                publicHtmlTable += '</tr>';
            });

            $('#public-advisements').html(publicHtmlTable);
        },
        error: function (errorMessage) {
            console.error("Error en GetPublicAdvisements:", errorMessage.responseText);
        }
    });
}

function GetAdvisementDetail(id) {
    $.ajax({
        url: "/Advisement/GetAdvisementById",
        type: "GET",
        data: { id: id }, // Enviar ID
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            // Asignar los datos a la vista
            $("#course").text(result.course.name);
            $("#author").text(result.user.name);
            $("#content").text(result.content);

            // Mostrar la sección de detalles
            $("#advisement-details").show();
        },
        error: function (errorMessage) {
            console.error("Error en GetAdvisementDetail:", errorMessage.responseText);
        }
    });
}


function AddAdvisement() { 
    var selectedCourseId = $('#course').val();  // esto esta incorrecto
    var selectedCourseName = $('#course option:selected').text();   // se puede hacer SP getCourseByName para traer el id, o hacerlo en el que ya esta 
    var advisementContent = $('#advisement-content').val(); 
    var isPublic = $('#publicCheck').is(':checked'); 

    var user = {
        id: "57f90130-0dee-4f6a-90bb-d00f37583cc0" 
    };

    var course = {
        id: selectedCourseId, 
        name: selectedCourseName
    };

    var advisement = {
        course: course,
        content: advisementContent,
        isPublic: isPublic,
        user: user
    };

    $.ajax({
        url: "/Advisement/CreateNewAdvisement",
        type: "POST",
        data: JSON.stringify(advisement),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (response) {
            alert("Consulta creada con éxito.");
            $("#create-advisement").hide();
        },
        error: function (errorMessage) {
            console.error("Error al crear la consulta:", errorMessage.responseText);
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
            toastr.error(errorMessage.responseText);
        }
    });
}

//------------------------------------------------
//-------------------UTILITY----------------------
//------------------------------------------------
function configureToastr() {
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-bottom-right",
        // ... otras opciones
    };
}