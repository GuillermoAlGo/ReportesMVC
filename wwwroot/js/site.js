// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $("#personasGrid tbody tr").dblclick(function () {
        var id = $(this).data("id");
        window.location.href = "/Persona/Edita/" + id;
    });
});
