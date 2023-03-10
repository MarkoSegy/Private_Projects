// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Alle 3 Sekunden =>
//setInterval(click, 3000);
//function click() {
//    $("#btnAdd2").click();
//    alert("Automatic button click after 3 seconds.");
//}
var button1 = document.getElementById('clicker');
setTimeout(function () {
    button1.click();
}, 0)

var button2 = document.getElementById('LogOutAgain');
setTimeout(function () {
    button2.click();
}, 0)

document.addEventListener("DOMContentLoaded", function (event) {
    document.querySelectorAll('img').forEach(function (img) {
        img.onerror = function () { this.style.display = 'none'; };
    })
});

/*HOME ANIMATION THREE.JS*/