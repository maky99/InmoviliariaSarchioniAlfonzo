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
const dias = document.getElementById('Dias');
const fechaFin = document.getElementById('FechaFin');

// Actualizar la fecha de fin cuando se ingresa la fecha de inicio y los días
dias.addEventListener('input', function () {
    if (fechaInicio.value && dias.value) {
        const fechaInicioDate = new Date(fechaInicio.value);
        const diasValue = parseInt(dias.value);

        // Calcular la fecha de fin sumando los días a la fecha de inicio
        fechaInicioDate.setDate(fechaInicioDate.getDate() + diasValue);

        // Formatear la fecha de fin en formato yyyy-mm-dd
        const year = fechaInicioDate.getFullYear();
        const month = String(fechaInicioDate.getMonth() + 1).padStart(2, '0');
        const day = String(fechaInicioDate.getDate()).padStart(2, '0');

        fechaFin.value = `${year}-${month}-${day}`;
    }
});

// Actualizar los días cuando se ingresa la fecha de inicio y la fecha de fin
fechaFin.addEventListener('input', function () {
    if (fechaInicio.value && fechaFin.value) {
        const fechaInicioDate = new Date(fechaInicio.value);
        const fechaFinDate = new Date(fechaFin.value);

        // Calcular la diferencia de días
        const diferenciaDias = Math.ceil((fechaFinDate - fechaInicioDate) / (1000 * 60 * 60 * 24));

        dias.value = diferenciaDias;
    }
});

