// Intercepta clics en los enlaces del header
document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll("a[data-section]").forEach(link => {
        link.addEventListener("click", (event) => {
            event.preventDefault(); // Evita el comportamiento predeterminado
            const section = link.getAttribute("data-section");
            history.pushState(null, "", `/${section}`); // Cambia la URL sin recargar
            loadSection(section); // Carga la secci�n en el contenedor
        });
    });

    // Maneja navegaci�n directa a una URL
    window.addEventListener("popstate", () => {
        const section = location.pathname.substring(1); // Obtiene el segmento de la URL
        if (section) {
            loadSection(section); // Carga la secci�n correspondiente
        }
    });

    // Carga la secci�n inicial basada en la URL
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
                throw new Error(`Error al cargar la secci�n: ${response.status}`);
            }
            return response.text();
        })
        .then(html => {
            // Crea un contenedor temporal para parsear el HTML
            const tempDiv = document.createElement('div');
            tempDiv.innerHTML = html;

            // Selecciona solo el contenido de la secci�n relevante
            const sectionContent = tempDiv.querySelector("#main-content").innerHTML;

            // Inserta solo el contenido de la secci�n en el contenedor principal
            mainContent.innerHTML = sectionContent;

            // Verifica si es la secci�n "news" y llama a LoadNewsItems si es necesario
            if (section === "news") {
                LoadNewsItems();
            }
        })
        .catch(error => {
            console.error(error);
            mainContent.innerHTML = `<p>Error: No se pudo cargar la secci�n "${section}".</p>`;
        });
}

