// Intercepta clics en los enlaces del header
document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll("a[data-section]").forEach(link => {
        link.addEventListener("click", (event) => {
            event.preventDefault(); // Evita el comportamiento predeterminado
            const section = link.getAttribute("data-section");
            history.pushState(null, "", `/${section}`); // Cambia la URL sin recargar
            loadSection(section); // Carga la sección en el contenedor
        });
    });

    // Maneja navegación directa a una URL
    window.addEventListener("popstate", () => {
        const section = location.pathname.substring(1); // Obtiene el segmento de la URL
        if (section) {
            loadSection(section); // Carga la sección correspondiente
        }
    });

    // Carga la sección inicial basada en la URL
    const initialSection = location.pathname.substring(1);
    if (initialSection) {
        loadSection(initialSection);
    }
});

function loadSection(section) {
    const mainContent = document.getElementById("main-content");

    mainContent.innerHTML = "";

    fetch(`/${section}`)
        .then(response => {
            if (!response.ok) {
                throw new Error(`Error al cargar la sección: ${response.status}`);
            }
            return response.text();
        })
        .then(html => {
            const tempDiv = document.createElement('div');
            tempDiv.innerHTML = html;

            const sectionContent = tempDiv.querySelector("#main-content").innerHTML;

            mainContent.innerHTML = sectionContent;

            if (section === "news") {
                LoadNewsItems();
            }
            if (section === "appointment") {
                GetAppointments();
                GetCourses();
                
            } 
	    if (section === "profile") {
                GetUserData();
            }
        })
        .catch(error => {
            console.error(error);
            mainContent.innerHTML = `<p>Error: No se pudo cargar la sección "${section}".</p>`;
        });
}

