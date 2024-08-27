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
