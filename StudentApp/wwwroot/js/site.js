$(document).ready(() => {
    $(document).on('submit', '#contact-form', function (event) {

        event.preventDefault();

        AuthenticateUser();
    });
});
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

