// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


document.addEventListener("DOMContentLoaded", function () {
    // Seleccionar todos los elementos de mensaje
    var messages = document.querySelectorAll(".temp-message");

    // Ocultar los mensajes después de 5 segundos
    setTimeout(function () {
        messages.forEach(function (message) {
            message.style.display = "none"; // Ocultar con display: none;
        });
    }, 5000); // 5000 ms = 5 segundos
});

function filtrarTabla() {
    var input, filtro, tabla, filas, celdas, texto;
    input = document.getElementById("inputBusqueda");
    filtro = input.value.toUpperCase();
    tabla = document.querySelector(".table");
    filas = tabla.getElementsByTagName("tbody")[0].getElementsByTagName("tr");

    for (var i = 0; i < filas.length; i++) {
        celdas = filas[i].getElementsByTagName("td");
        var mostrar = false;

        for (var j = 0; j < celdas.length; j++) {
            texto = celdas[j].textContent || celdas[j].innerText;
            if (texto.toUpperCase().indexOf(filtro) > -1) {
                mostrar = true;
                break;
            }
        }

        filas[i].style.display = mostrar ? "" : "none";

    }
}
//para calcular la fecha de inicio y de fin en el buscador 
// Obtener los elementos
const fechaInicio = document.getElementById('FechaInicio');
const meses = document.getElementById('Meses');
const fechaFin = document.getElementById('FechaFin');

// Actualizar la fecha de fin cuando se ingresa la fecha de inicio y los meses
meses.addEventListener('input', function () {
    if (fechaInicio.value && meses.value) {
        const fechaInicioDate = new Date(fechaInicio.value);
        const mesesValue = parseInt(meses.value);

        // Calcular la fecha de fin sumando los meses a la fecha de inicio
        fechaInicioDate.setMonth(fechaInicioDate.getMonth() + mesesValue);

        // Formatear la fecha de fin en formato yyyy-mm-dd
        const year = fechaInicioDate.getFullYear();
        const month = String(fechaInicioDate.getMonth() + 1).padStart(2, '0');
        const day = String(fechaInicioDate.getDate()).padStart(2, '0');

        fechaFin.value = `${year}-${month}-${day}`;
    }
});

// Actualizar los meses cuando se ingresa la fecha de inicio y la fecha de fin
fechaFin.addEventListener('input', function () {
    if (fechaInicio.value && fechaFin.value) {
        const fechaInicioDate = new Date(fechaInicio.value);
        const fechaFinDate = new Date(fechaFin.value);

        // Calcular la diferencia en meses
        let diferenciaMeses = (fechaFinDate.getFullYear() - fechaInicioDate.getFullYear()) * 12;
        diferenciaMeses += fechaFinDate.getMonth() - fechaInicioDate.getMonth();

        // Ajustar si la fecha de fin es antes del día de la fecha de inicio
        if (fechaFinDate.getDate() < fechaInicioDate.getDate()) {
            diferenciaMeses--;
        }

        meses.value = diferenciaMeses;
    }
});

    
await Html.RenderPartialAsync("_ValidationScriptsPartial");


