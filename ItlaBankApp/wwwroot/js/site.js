// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.getElementById("roleSelect").addEventListener("change", function () {
    var roleSelect = document.getElementById("roleSelect");
    var amountField = document.getElementById("amountField");

    if (roleSelect.value === "Client") {
        amountField.querySelector("input").removeAttribute("disabled");
    } else {
        amountField.querySelector("input").setAttribute("disabled", "disabled");
    }
});