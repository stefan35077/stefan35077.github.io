document.addEventListener("DOMContentLoaded", function () {
    const dropMenu = document.getElementById("dropMenu");
    const dropContent = document.getElementById("dropContent");

    dropMenu.addEventListener("click", function () {
        dropContent.classList.toggle("show");
    });


    window.addEventListener("click", function (event) {
        if (!event.target.matches("#dropMenu") && !event.target.matches(".dropButton")) {
            dropContent.classList.remove("show");
        }
    });
});

