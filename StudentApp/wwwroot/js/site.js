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

function setLoading(isloading) {

    if (isloading) {
        $('#loading-overlay').css('display', 'flex');

    } else {
        $('#loading-overlay').css('display', 'none');

    }
}
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

                localStorage.setItem("email", email);
                localStorage.setItem("userId", response.userId);

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
            data: JSON.stringify(user), //convierte la variable estudiante en tipo json
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
    const userEmail = localStorage.getItem("email");

    $.ajax({
        url: "/Appointment/GetAllAppointmentsByUser",
        type: "GET",
        data: {
            email: userEmail
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
                        statusText = item.status; //Mostrar el valor original si no coincide con ninguno de los anteriores
                }
                htmlTable += '<td>' + statusText + '</td>'; htmlTable += '<td>' + item.course.name + '</td>';
                htmlTable += '<td>' + (item.professorComment == null ? 'Sin comentarios' : item.professorComment) + '</td>';
                htmlTable += '</tr>';
            });
            $('#myappointments-tbody').html(htmlTable); //shows table on screen


        },
        error: function (errorMessage) {
            configureToastr();
            toastr.error(errorMessage.me);
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

    configureToastr();

    const userId = localStorage.getItem("userId");

    var appointment = {
        date: $('#datetime').val(),
        mode: $('#mode').val(),
        courseid: $('#course').val(),
        userId,
    };
    var course = {
        id: $('#course').val(),
        name: $('#course').find('option:selected').text(),
    };
    var user = {
        id: userId,
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
//--------------PROFILE SECTION-------------------
//------------------------------------------------
function GetUserData() {
    console.log("GetUserData called");

    const userEmail = localStorage.getItem("email");

    $.ajax({
        url: "/User/GetByEmail",
        type: "GET",
        data: { email: userEmail },
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
                //console.log("Imagen asignada:", result.picture);

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
        if ($('#p-name2').val() == '' || $('#p-email').val() == '' || $('#p-password').val() == '') {
            configureToastr();
            toastr.error('Por favor rellene todos los campos');
        } else if (!ValidatePassword()) {
            configureToastr();
            toastr.error('La contraseña debe tener una longitud mínima de 8 caracteres y debe contener al menos un número');
        }
        else {
            Swal.fire({
                text: "¿Deseas guardar los cambios realizados?",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Sí, guardar',
                cancelButtonText: 'Cancelar'
            }).then((result) => {
                if (result.isConfirmed) {
                    EditUser();
                }
            });
        }
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

    $('#p-name2').prop("readonly", false);
    $('#email-warning').prop("hidden", false);
    $('#p-password').prop("readonly", false);
    $('#p-description').prop("readonly", false);
    $('#p-linkedin').prop("readonly", false);

    $('#p-button').text("Confirmar cambios");
    $('#p-cancel-button').prop("hidden", false);
    $('#p-upload-img-label').prop("hidden", false);
    $('#p-delete-button').prop("hidden", true);

    $('#p-email').css("margin-bottom", "0px");
}

function EditUser() {
    var newValues = {
        id: $('#p-id').val(),
        name: $('#p-name2').val(),
        password: $('#p-password').val(),
        description: $('#p-description').val(),
        linkedIn: $('#p-linkedin').val(),
        picture: $('#p-picture').attr("src")
    }

    $.ajax({
        url: "/User/UpdateUser",
        type: "PUT",
        data: JSON.stringify(newValues),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            GetUserData();

            $('#p-name2').prop("readonly", true);
            $('#email-warning').prop("hidden", true);
            $('#p-password').prop("readonly", true);
            $('#p-description').prop("readonly", true);
            $('#p-linkedin').prop("readonly", true);

            $('#p-button').text("Editar");
            $('#p-cancel-button').prop("hidden", true);
            $('#p-upload-img-label').prop("hidden", true);

            $('#p-email').css("margin-bottom", "30px");

            configureToastr();
            toastr.success('Los datos fueron actualizados correctamente');
        },
        error: function (errorMessage) {
            toastr.error('Algo salió mal');
        }
    });
    
}

function CancelEditing() {
    GetUserData();

    $('#p-name2').prop("readonly", true);
    $('#email-warning').prop("hidden", true);
    $('#p-password').prop("readonly", true);
    $('#p-description').prop("readonly", true);
    $('#p-linkedin').prop("readonly", true);

    $('#p-button').text("Editar");
    $('#p-cancel-button').prop("hidden", true);
    $('#p-upload-img-label').prop("hidden", true);
    $('#p-delete-button').prop("hidden", false);

    $('#p-email').css("margin-bottom", "30px");
}

function ValidatePassword(){
    var password = $('#p-password').val();

    if (password.length < 8) {
        return false;
    }

    var hasNumber = /\d/.test(password); //verifies that it contains at least a number
    return hasNumber;
}


function DeleteAccount() {
    const userId = localStorage.getItem("userId");

    Swal.fire({
        text: "¿Seguro de que deseas eliminar la cuenta?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `/User/DeleteUser?id=${userId}`,
                type: "DELETE",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.success) {
                        Swal.fire({
                            icon: 'success',
                            title: 'Cuenta eliminada',
                            text: 'Tu cuenta ha sido eliminada exitosamente'
                        }).then(() => {
                            //TODO: Redirect to login
                        });
                    }
                },
                error: function (xhr, status, error) {
                    console.error("Error:", error);
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'No se pudo eliminar la cuenta'
                    });
                }
            });
        }
    });
}

function ShowImage(input) {
    if (input.files && input.files[0]) {
        var filerdr = new FileReader();
        filerdr.onload = function (e) {
            $('#p-picture').attr('src', e.target.result);
        };
        filerdr.readAsDataURL(input.files[0]);
    }
}

//------------------------------------------------
//--------------NEWS SECTION----------------------
//------------------------------------------------
function LoadNewsItems() {

    setLoading(true);

    $.ajax({
        url: "/PieceOfNews/GetNews",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

            htmlContent =  '';

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
                </div> `;

            });
            $("#news-container").html(htmlContent);
            setLoading(false);

            $("#news-container").css('height', 'auto');

        },
        error: function (errorMessage) {
            console.log(errorMessage.responseText);
        }
    });
}

function LoadNewsDetail(pieceOfNewsID) {

    setLoading(true);

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
                    <p style="text-align:justify; border-radius: 33px; padding-top:20px; padding-bottom:20px; white-space: pre-line;">${newsItem.description}</p>
                    <div id="commentsContainer"></div>
            `;

            setLoading(false);

            $("#news-container").html(detailHtml);
            LoadNewsComments(pieceOfNewsID);
        },
        error: function (errorMessage) {
            console.log(errorMessage.responseText);
        }
    });
}

function LoadNewsComments(pieceOfNewsID) {

    $.ajax({
        url: "/CommentNews/GetCommentNewsByPieceOfNewsId/" + pieceOfNewsID,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        success: function (newsComments) {



            var htmlContent = `<br><h3>Comentarios</h3>`;
            $("#commentsContainer>").html(`<div class="loader"></div>`);

            htmlContent += `
                <div class="comment-add-container" id="comment-news-form">

                    <textarea id="comment-text-area" class="comment-text-area" maxlength="200" rows="4" placeholder="Escribe tu comentario..." required></textarea><br>
                    <button id="addCommentBtn" class="comment-button" onclick="AddNewsComment('${pieceOfNewsID}')">
                        Agregar Comentario
                    </button>
                </div>
                <div style="text-align:right; padding-right:10px;">
                    <br> <h6>${newsComments.length !== 1 ? newsComments.length + " comentarios" : " comentario"} </h6><br>
                </div>
            `;

            newsComments.forEach(comment => {

                htmlContent += `
                    <div class="comment-card">
                        <div class="comment-header">
                            <div>
                                <span class="comment-user">${comment.user.name}</span>
                                <span class="comment-role">(${comment.user.role})</span>
                            </div>
                           
                            <span class="comment-date">${new Date(comment.dateTime).toLocaleString()}</span>
                        </div>
                        <div class="comment-body">
                            <p>${comment.text}</p>
                        </div>
                    </div>
                `;
            });

            $('#commentsContainer').html(htmlContent);
            

        },
        error: function (errorMessage) {
            toastr.error(errorMessage.responseText);
        }
    });
}
function AddNewsComment(pieceOfNewsID) {

    
    var text = $('#comment-text-area').val();

    if (text) {
        $("#comment-text-area").css("border-color", "black");

        const userID = localStorage.getItem("userId");

        var comment = {

            pieceOfNews: {
                id: pieceOfNewsID
            },
            user: {
                id: userID
            },
            text: text
        }

        setLoading(true);
        $.ajax({
            url: "/CommentNews/AddNewsComment",
            data: JSON.stringify(comment), //convierte la variable estudiante en tipo json
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {

                toastr.options.positionClass = 'toast-bottom-right';
                toastr.success('Comentario publicado con éxito');

                LoadNewsComments(pieceOfNewsID);
                setLoading(false);


            },
            error: function (errorMessage) {
                console.log(errorMessage.responseText);
                setLoading(false);

            }
        });
    } else {

        toastr.options.positionClass = 'toast-bottom-right';
        toastr.error('Por favor rellene todos los campos');

        $("#comment-text-area").css("border-color", "red");

    }
  

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
