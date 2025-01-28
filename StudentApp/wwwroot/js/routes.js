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

    // Limpia completamente el contenido anterior
    mainContent.innerHTML = "";

    // Llama al backend para obtener la vista parcial
    fetch(`/${section}`)
        .then(response => {
            if (!response.ok) {
                throw new Error(`Error al cargar la sección: ${response.status}`);
            }
            return response.text();
        })
        .then(html => {
            // Crea un contenedor temporal para parsear el HTML
            const tempDiv = document.createElement('div');
            tempDiv.innerHTML = html;

            // Selecciona solo el contenido de la sección relevante
            const sectionContent = tempDiv.querySelector("#main-content").innerHTML;

            // Inserta solo el contenido de la sección en el contenedor principal
            mainContent.innerHTML = sectionContent;

            // Verifica si es la sección "news" y llama a LoadNewsItems si es necesario
            if (section === "news") {
                LoadNewsItems();
            }
        })
        .catch(error => {
            console.error(error);
            mainContent.innerHTML = `<p>Error: No se pudo cargar la sección "${section}".</p>`;
        });
}

