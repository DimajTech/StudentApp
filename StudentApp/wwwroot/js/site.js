﻿$(document).ready(() => {
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
            console.log(errorMessage.responseText);
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
